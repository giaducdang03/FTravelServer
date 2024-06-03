using AutoMapper;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;

        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public async Task<AccountModel> CreateAccount(AccountModel account)
        {
            try
            {
                var data = await _accountRepo.GetAllUser();
                var checkExist = data.Where(x => x.Email.Equals(account.Email));

                if (checkExist.Any())
                {
                    return null;
                }

                var map = _mapper.Map<User>(account);
                var createAccount = await _accountRepo.CreateAccount(map);
                var resutl = _mapper.Map<AccountModel>(createAccount);
                return resutl;  
            }catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetAccountInfoByEmail(string email)
        {
            var data = await _accountRepo.GetUserInfoByEmail(email);
            return data;
        }

        public async Task<List<AccountModel>> GetAllUserAscyn()
        {
            try
            {
                var account = await _accountRepo.GetAllUser();
                var map = _mapper.Map<List<AccountModel>>(account);
                return map;
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);

            }
        }


    }
}
