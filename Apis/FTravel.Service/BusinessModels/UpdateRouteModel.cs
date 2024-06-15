using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class UpdateRouteModel
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "start-point-id")]
        public int StartPoint { get; set; }
        [FromQuery(Name = "end-point-id")]
        public int EndPoint { get; set; }
        [FromQuery(Name = "status")]
        public string Status { get; set; }
        [FromQuery(Name = "bus-company-id")]
        public int BusCompanyId { get; set; }
    }
}
