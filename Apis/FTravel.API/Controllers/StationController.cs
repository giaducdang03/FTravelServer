﻿using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/stations")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        //[HttpGet("stationList")]
        //public async Task<IActionResult> GetAllUser()
        //{
        //    try
        //    {
        //        var user = await _stationService.GetAllStationService();
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = 400,
        //            Message = ex.Message
        //        });
        //    }

        //}


        [HttpGet]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> GetAllStation([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _stationService.GetAllStationService(paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Station is empty"
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
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> GetStationDetailById(int id)
        {
            try
            {
                var data = await _stationService.GetStationServiceDetailById(id);
                if (id == null)
                {
                    return BadRequest();
                }
                return Ok(data);
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
        

        [HttpPost]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> CreateStationController(CreateStationModel stationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _stationService.CreateStationService(stationModel.Name, stationModel.BusCompanyId);
                    return Ok(data);
                }
                return ValidationProblem(ModelState);
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
    }
}
