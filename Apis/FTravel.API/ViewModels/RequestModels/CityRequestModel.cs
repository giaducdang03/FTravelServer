using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CityRequestModel
    {
        [Required]
        [FromQuery(Name = "id")]
        public int CityId { get; set; }
       
        [MaxLength(100)]
		[FromQuery(Name = "name")]
		public string Name { get; set; } = null!;

		[MaxLength(100)]
		[FromQuery(Name = "unsign-name")]
		public string UnsignName { get; set; } = null!;
		[MaxLength(100)]
		[FromQuery(Name = "code")]
		public string Code { get; set; } = null!;
	}
}
