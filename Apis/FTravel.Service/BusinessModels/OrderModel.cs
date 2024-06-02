using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class OrderModel
    {
        public int Amount { get; set; }

        public string Description { get; set; } = "";
    }
}
