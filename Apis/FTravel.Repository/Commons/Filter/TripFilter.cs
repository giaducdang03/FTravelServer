using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Commons.Filter
{
    public class TripFilter : FilterBase
    {
        [FromQuery(Name = "trip-status")]
        public string? TripStatus { get; set; }
        [FromQuery(Name = "trip-start-date")]
        public DateTime? StartDate { get; set; }
        [FromQuery(Name = "trip-start-point")]
        public string StartPoint { get; set; }
        [FromQuery(Name = "trip-end-point")]
        public string EndPoint {  get; set; }

    }
}
