using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CityRequestModel
    {
       
       
        [MaxLength(100)]
		[FromQuery(Name = "name")]
		public string Name { get; set; } = null!;

		[FromQuery(Name = "code")]
		public int Code { get; set; }
	}
}
