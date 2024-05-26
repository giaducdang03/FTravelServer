using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;
        private readonly IClaimsService _claimsService;

        public WalletsController(IWalletService walletService,
            IClaimsService claimsService,
            ITransactionService transactionService)
        {
            _walletService = walletService;
            _transactionService = transactionService;
            _claimsService = claimsService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllWallets([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _walletService.GetAllWalletsAsync(paginationParameter);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No wallets"
                    });
                }
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
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
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Not found wallet"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("transaction/{walletId}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> GetTransactionsWallet(int walletId, [FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _transactionService.GetTransactionsByWalletIdAsync(walletId, paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No transaction"
                    });
                }
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
