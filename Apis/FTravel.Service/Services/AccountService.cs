using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using FTravel.Service.Utils.Email;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FTravel.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepo,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICustomerRepository customerRepository,
            IWalletRepository walletRepository,
            IMailService mailService,
            IMapper mapper)
        {
            _accountRepo = accountRepo;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _customerRepository = customerRepository;
            _walletRepository = walletRepository;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task<bool> CreateAccountAsync(CreateAccountModel model)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {

                    User newUser = _mapper.Map<User>(model);
                    newUser.Status = UserStatus.ACTIVE.ToString();
                    newUser.UnsignFullName = StringUtils.ConvertToUnSign(model.FullName);

                    var existUser = await _userRepository.GetUserByEmailAsync(model.Email);

                    if (existUser != null)
                    {
                        throw new Exception("Tài khoản đã tồn tại.");
                    }

                    // generate password
                    string password = PasswordUtils.GeneratePassword();

                    // hash password
                    newUser.PasswordHash = PasswordUtils.HashPassword(password);

                    var role = await _roleRepository.GetRoleByName(model.Role.ToString());
                    if (role == null)
                    {
                        Role newRole = new Role
                        {
                            Name = model.Role.ToString()
                        };
                        await _roleRepository.AddAsync(newRole);
                        role = newRole;
                    }

                    newUser.RoleId = role.Id;

                    await _userRepository.AddAsync(newUser);

                    if (role.Name == RoleEnums.CUSTOMER.ToString())
                    {
                        var existCustomer = await _customerRepository.GetCustomerByEmailAsync(model.Email);
                        if (existCustomer == null)
                        {
                            Customer newCustomer = _mapper.Map<Customer>(newUser);
                            newCustomer.Id = 0;

                            // add wallet
                            Wallet customerWallet = new Wallet
                            {
                                AccountBalance = 0,
                                Status = WalletStatus.ACTIVE.ToString(),
                            };
                            newCustomer.Wallet = customerWallet;

                            await _customerRepository.AddAsync(newCustomer);

                        }
                    }

                    // send email password
                    MailRequest passwordEmail = new MailRequest()
                    {
                        ToEmail = model.Email,
                        Subject = "FTravel Welcome",
                        Body = EmailCreateAccount.EmailSendCreateAccount(model.Email, password)
                    };

                    await _mailService.SendEmailAsync(passwordEmail);

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAccountAsync(int id, string currentEmail)
        {
            var account = await _accountRepo.GetByIdAsync(id);
            if (account != null)
            {
                // check current user
                if (account.Email == currentEmail)
                {
                    throw new Exception("Tài khoản đang đăng nhập. Không thể xóa.");
                }

                // check confirm email
                if (account.ConfirmEmail == true)
                {
                    account.Status = UserStatus.BANNED.ToString();
                    await _accountRepo.SoftDeleteAsync(account);
                    return true;
                }
                else
                {
                    using (var transaction = await _userRepository.BeginTransactionAsync())
                    {
                        try
                        {
                            var accountRole = await _roleRepository.GetByIdAsync(account.RoleId.Value);
                            if (accountRole != null)
                            {
                                // if customer delete customer and wallet
                                if (accountRole.Name == RoleEnums.CUSTOMER.ToString())
                                {
                                    var customer = await _customerRepository.GetCustomerByEmailAsync(account.Email);
                                    if (customer != null)
                                    {
                                        await _walletRepository.PermanentDeletedAsync(customer.Wallet);
                                        await _customerRepository.PermanentDeletedAsync(customer);
                                        await _accountRepo.PermanentDeletedAsync(account);

                                        await transaction.CommitAsync();
                                        return true;
                                    }
                                }
                                else
                                {
                                    await _accountRepo.PermanentDeletedAsync(account);
                                    await transaction.CommitAsync();
                                    return true;
                                }
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                        
                }
            }
            throw new Exception("Tài khoản không tồn tại.");
        }

        //public async Task<AccountModel> CreateAccount(AccountModel account)
        //{
        //    try
        //    {
        //        var data = await _accountRepo.GetAllUser();
        //        var checkExist = data.Where(x => x.Email.Equals(account.Email));

        //        if (checkExist.Any())
        //        {
        //            return null;
        //        }

        //        var map = _mapper.Map<User>(account);
        //        var createAccount = await _accountRepo.CreateAccount(map);
        //        var resutl = _mapper.Map<AccountModel>(createAccount);
        //        return resutl;  
        //    }catch (Exception ex) {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<User> GetAccountInfoByEmail(string email)
        {
            var data = await _accountRepo.GetUserInfoByEmail(email);
            return data;
        }

        public async Task<User> GetAccountInfoById(int id)
        {
            var data = await _accountRepo.GetUserInfoById(id);
            return data;
        }

        public async Task<Pagination<AccountModel>> GetAllUsersAsync(PaginationParameter paginationParameter)
        {
            var users = await _accountRepo.GetAllUserAccount(paginationParameter);
            var accountModels = _mapper.Map<List<AccountModel>>(users);
            return new Pagination<AccountModel>(accountModels,
                users.TotalCount,
                users.CurrentPage,
                users.PageSize);
        }


        public async Task<bool> UpdateFcmTokenAsync(string email, string fcmToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null && !fcmToken.IsNullOrEmpty())
            {
                if (user.Fcmtoken != fcmToken) 
                {
                    user.Fcmtoken = fcmToken;
                    var result = await _userRepository.UpdateAsync(user);
                    return true ? result > 0 : false;
                }
            }
            return false;
        }

        //public async Task<List<AccountModel>> GetAllUserAscyn()
        //{
        //    try
        //    {
        //        var account = await _accountRepo.GetAllUser();
        //        var map = _mapper.Map<List<AccountModel>>(account);
        //        return map;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);

        //    }
        //}
        public async Task<bool> UpdateAccount(int id, UpdateAccountModel accountModel)
        {
            try
            {
                var existAccount = await _accountRepo.GetByIdAsync(id);
                if (existAccount == null)
                {
                    return false;
                }
                if (!DateTime.TryParseExact(accountModel.Dob, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dob))
                {
                    //error fomat date time
                    return false;
                }
                _mapper.Map(accountModel, existAccount);
                await _accountRepo.UpdateAsync(existAccount);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Fail to update service {ex.Message}");
                return false;
            }
        }

    }
}
