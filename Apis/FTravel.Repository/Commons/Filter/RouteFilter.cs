using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Commons.Filter
{
    public class RouteFilter : FilterBase
    {
        [FromQuery(Name = "route-name")]
        public string? RouteName {  get; set; }
        [FromQuery(Name = "start-point")]
        public int? StartPoint { get; set; }
        [FromQuery(Name = "end-point")]
        public int? EndPoint { get; set; }
    }
}
