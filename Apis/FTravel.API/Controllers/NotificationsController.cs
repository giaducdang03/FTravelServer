using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.Enums;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;

        public NotificationsController(INotificationService notificationService,
            IClaimsService claimsService) 
        {
            _notificationService = notificationService;
            _claimsService = claimsService;
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsByEmail(PaginationParameter paginationParameter)
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _notificationService.GetNotificationsByEmail(email, paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Notification is empty"
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
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetNotificationById(int id) 
        { 
            try
            {
                var result = await _notificationService.GetNotificationById(id);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Not found notification."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNotification(int accountId, Notification notification)
        {
            try
            {
                var result = await _notificationService.AddNotificationByUserId(accountId, notification);
                if (result == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Can not add notification."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpGet("mark-all-isread")]
        [Authorize]
        public async Task<IActionResult> MarkAllUserNotificationIsRead()
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _notificationService.MarkAllUserNotificationIsReadAsync(email);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Mark read all notifications successfully."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Can not mark read all notifications."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpGet("{notificationId}/mark-isread")]
        [Authorize]
        public async Task<IActionResult> MarkAllUserNotificationIsRead(int notificationId)
        {
            try
            {
                var result = await _notificationService.MarkNotificationIsReadById(notificationId);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Mark read notification successfully."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Can not mark read notification."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpPost("add-notification-for-roles")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddNotificationForRole(CreateNotificationModel createNotificationModel)
        {
            try
            {
                var newNoti = new Notification
                {
                    Title = createNotificationModel.Title,
                    Message = createNotificationModel.Message
                };
                var result = await _notificationService.AddNotificationByRoleAsync(createNotificationModel.RoleEnums.Value, newNoti);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Can not add notification."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpPost("add-notification-for-users")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddNotificationForUsers(CreateNotificationModel createNotificationModel)
        {
            try
            {
                var newNoti = new Notification
                {
                    Title = createNotificationModel.Title,
                    Message = createNotificationModel.Message
                };
                var result = await _notificationService.AddNotificationByListUser(createNotificationModel.UserIds, newNoti);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Can not add notification."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }
    }
}
