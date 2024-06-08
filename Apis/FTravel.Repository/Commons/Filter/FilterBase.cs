using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FTravel.Repository.Commons.Filter
{
    public class FilterBase
    {
        [FromQuery(Name = "search")]
        public string? Search { get; set; }

        [FromQuery(Name = "sort-by")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "dir")]
        public string? Dir { get; set; }
    }
}
