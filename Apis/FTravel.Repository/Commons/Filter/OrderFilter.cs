using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Commons.Filter
{
    public class OrderFilter : FilterBase
    {
        [FromQuery(Name = "bus-company-name")]
        public string? BusCompanyName { get; set; }
    }
}
