using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/ticket-type")]
    [ApiController]
    public class TicketTypeController : ControllerBase
    {
        private readonly ITicketTypeService _ticketTypeService;

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [HttpGet]
        [Authorize(Roles = "BUSCOMPANY,ADMIN")]
        public async Task<IActionResult> GetAllTicketType([FromQuery] PaginationParameter paginationParameter, int? routeId)
        {
            try
            {
                var result = await _ticketTypeService.GetAllTicketType(paginationParameter, routeId);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy danh sách loại vé"
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

        [HttpGet("{id}")]
        [Authorize(Roles = "BUSCOMPANY,ADMIN")]
        public async Task<IActionResult> GetTicketTypeDetailById(int id)
        {
            try
            {
                var result = await _ticketTypeService.GetTicketTypeById(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy loại vé này"
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

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateTicketType(CreateTicketTypeModel ticketTypeModel)
        {
            try
            {
                if (ticketTypeModel == null)
                {
                    return BadRequest("Xảy ra lỗi khi tạo loại vé mới");
                }

                var data = await _ticketTypeService.CreateTicketTypeAsync(ticketTypeModel);

                return Ok("Tạo loại vé mới thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateTicketType(int id, UpdateTicketTypeModel ticketTypeModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isUpdated = await _ticketTypeService.UpdateTicketTypeAsync(id, ticketTypeModel);

                if (isUpdated)
                {
                    // Return a success response
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Cập nhật loại vé thành công"
                    });
                }
                else
                {
                    // Return a not found response if the service was not updated successfully
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Cập nhật loại vé thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                // Return a bad request response for any other exceptions
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }

}
