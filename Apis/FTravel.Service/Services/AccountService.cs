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

        public async Task<bool> UpdateAccount(UpdateAccountModel accountModel)
        {
            var existAccount = await _accountRepo.GetByIdAsync(accountModel.AccountId);

            if (existAccount != null) 
            {
                var newUnsignName = StringUtils.ConvertToUnSign(accountModel.FullName);
                var accountRole = await _roleRepository.GetByIdAsync(existAccount.RoleId.Value);
                if (accountRole != null) 
                { 
                    // update account
                    existAccount.FullName = accountModel.FullName;
                    existAccount.UnsignFullName = newUnsignName;
                    existAccount.PhoneNumber = accountModel.PhoneNumber;
                    existAccount.Dob = accountModel.Dob;
                    existAccount.Address = accountModel.Address;
                    existAccount.Gender = accountModel.Gender;
                    

                    if (accountRole.Name == RoleEnums.CUSTOMER.ToString())
                    {
                        using (var updatedTransaction = await _accountRepo.BeginTransactionAsync())
                        {
                            try
                            {
                                var existCustomer = await _customerRepository.GetCustomerByEmailAsync(existAccount.Email);
                                if (existCustomer != null) 
                                {
                                    // update customer
                                    existCustomer.FullName = accountModel.FullName;
                                    existCustomer.UnsignFullName = newUnsignName;
                                    existCustomer.PhoneNumber = accountModel.PhoneNumber;
                                    existCustomer.Dob = accountModel.Dob;
                                    existCustomer.Address = accountModel.Address;
                                    existCustomer.Gender = accountModel.Gender;

                                    await _customerRepository.UpdateAsync(existCustomer);
                                    await _accountRepo.UpdateAsync(existAccount);

                                    await updatedTransaction.CommitAsync();
                                    return true;
                                }
                            }
                            catch
                            {
                                await updatedTransaction.RollbackAsync();
                                throw;
                            }
                        }
                    } 
                    else
                    {
                        await _accountRepo.UpdateAsync(existAccount);
                        return true;
                    }
                }
                return false;
            } 
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
        }

    }
}
