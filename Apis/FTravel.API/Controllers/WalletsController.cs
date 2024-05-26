using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IClaimsService _claimsService;

        public WalletsController(IWalletService walletService, IClaimsService claimsService)
        {
            _walletService = walletService;
            _claimsService = claimsService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllWallets()
        {
            try
            {
                return Ok(await _walletService.GetAllWalletsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> GetWalletCustomer()
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _walletService.GetWalletByEmailAsync(email);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("transaction/{walletId}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> GetTransactionsWallet(int walletId)
        {
            try
            {
                var result = await _walletService.GetTransactionsByWalletIdAsync(walletId);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
