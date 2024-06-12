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
    public class CityModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [FromQuery(Name = "name")]
        public string Name { get; set; } = null!;

        [FromQuery(Name = "code")]
        public int Code { get; set; }
    }
}
