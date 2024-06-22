using FTravel.Service.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.RouteModels
{
    public class UpdateRouteModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }

        public CommonStatus Status { get; set; }
        public int BusCompanyId { get; set; }
    }
}
