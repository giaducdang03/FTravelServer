using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FTravel.API.ViewModels.RequestModels
{
    public class RouteRequestModel
    {
        [Required]
        [FromQuery(Name = "id")]
        public int Id { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "start-point-id")]
        public int StartPoint {  get; set; }
        [FromQuery(Name = "end-point-id")]
        public int EndPoint { get; set; }
        [FromQuery(Name = "status")]
        public string Status {  get; set; }
        [FromQuery(Name = "bus-company-id")]
        public int BusCompanyId { get; set; }
    }
}
