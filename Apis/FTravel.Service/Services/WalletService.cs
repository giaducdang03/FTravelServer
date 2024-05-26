﻿using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
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
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public WalletService(IWalletRepository walletRepository,
            ICustomerRepository customerRepository, IMapper mapper)
        {
            _walletRepository = walletRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        //public async Task<List<WalletModel>> GetAllWalletsAsync()
        //{
        //    var wallets = await _walletRepository.GetAllAsync();
        //    var walletModels = _mapper.Map<List<WalletModel>>(wallets);
        //    return walletModels;
        //}

        public async Task<Pagination<WalletModel>> GetAllWalletsAsync(PaginationParameter paginationParameter)
        {
            var wallets = await _walletRepository.ToPagination(paginationParameter);
            var customerIds = wallets.Select(w => w.CustomerId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var customers = await _customerRepository.GetCustomersByIdsAsync(customerIds);

            var walletModels = wallets.Select(wallet =>
            {
                var customer = customers.FirstOrDefault(c => c.Id == wallet.CustomerId);
                var walletModel = _mapper.Map<WalletModel>(wallet);
                if (customer != null)
                {
                    walletModel.CustomerName = customer.FullName;
                }
                return walletModel;
            }).ToList();

            return new Pagination<WalletModel>(walletModels, 
                wallets.TotalCount,
                wallets.CurrentPage,
                wallets.PageSize);
        }

        public async Task<WalletModel> GetWalletByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer != null)
            {
                var customerWallet = await _walletRepository.GetWalletByCustomerId(customer.Id);
                WalletModel walletModel = _mapper.Map<WalletModel>(customerWallet);
                walletModel.CustomerName = customer.FullName;
                return walletModel;
            }
            return null;
        }
    }
}
