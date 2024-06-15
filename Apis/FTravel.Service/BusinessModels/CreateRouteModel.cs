﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class CreateRouteModel
    {
        public string? UnsignName { get; set; }

        public string Name { get; set; } = null!;

        public string? StartPoint { get; set; }

        public string? EndPoint { get; set; }

        public string? Status { get; set; }

        public string? BusCompanyName { get; set; }
    }
}