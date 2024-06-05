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
        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public string? Dir { get; set; }
    }
}
