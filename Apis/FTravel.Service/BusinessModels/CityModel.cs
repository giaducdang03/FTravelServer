using FTravel.Repository.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class CityModel : BaseEntity
    {
        public string Name { get; set; } = "";
        public int Code { get; set; }
    }
}
