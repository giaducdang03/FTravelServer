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
using System;
using System.Collections.Generic;
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
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepo,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICustomerRepository customerRepository,
            IMailService mailService,
            IMapper mapper)
        {
            _accountRepo = accountRepo;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _customerRepository = customerRepository;
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
                        throw new Exception("Account already exists.");
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

        public async Task<Pagination<AccountModel>> GetAllUserAccountService(PaginationParameter paginationParameter)
        {
            var users = await _accountRepo.GetAllUserAccount(paginationParameter);
            if (!users.Any())
            {
                return null;
            }

            var accountModels = _mapper.Map<List<AccountModel>>(users);
            foreach (var accountModel in accountModels)
            {
                accountModel.Id = accountModel.Id; // Ánh xạ giá trị Id từ UserId
            }

            return new Pagination<AccountModel>(accountModels,
                users.TotalCount,
                users.CurrentPage,
                users.PageSize);
        }

        public Task<List<AccountModel>> GetAllUserAscyn()
        {
            throw new NotImplementedException();
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


    }
}
