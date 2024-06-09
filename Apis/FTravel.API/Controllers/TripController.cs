using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;


        public TripController(ITripService tripService)
        {
            _tripService = tripService;

        }

        [HttpGet]
        //[Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> GetAllTripStatusOpening([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _tripService.GetAllTripAsync(paginationParameter);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No trips was found"
                    });
                }


                else
                {
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
        //[Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> GetTripDetailByIdStatusOpening(int id)
        {
            try
            {
                var result = await _tripService.GetTripByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No trip was found"
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
        [HttpPost()]
        [Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> AddTrip([FromBody] CreateTripModel tripModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!Enum.TryParse(typeof(TripStatus), tripModel.Status, true, out _))
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = $"Invalid status value. Allowed values are: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}."
                    });
                }

                var result = await _tripService.CreateTripAsync(tripModel);
                if (result)
                {
                    return Ok("Trip created successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create trip.");
                }
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
        [HttpPut("{id}")]
        [Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> UpdateTrip(int id, UpdateTripModel tripModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _tripService.UpdateTripAsync(id, tripModel);
                if (result)
                {
                    return Ok("Trip updated successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update trip.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
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
        [HttpPut("{id}/status")]
        //[Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> UpdateTripStatus(int id, string status)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _tripService.UpdateTripStatusAsync(id, status);
                if (result)
                {
                    return Ok("Trip status updated successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update trip status.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
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
        [HttpPut("{id}/cancel")]
        //[Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> Cancelrip(int id, string status)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _tripService.CancelTripAsync(id, status);
                if (result)
                {
                    return Ok("Trip delete successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete trip.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
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

