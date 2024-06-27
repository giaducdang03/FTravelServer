using FTravel.Repository.EntityModels;
using FTravel.Service.Enums;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class OrderDetailModel : BaseEntity
    {
        public string Type { get; set; }
        public string ServiceName { get; set; }
        public string SeatCode { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string TripName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        public string BusCompanyName { get; set; }

        public DateTime TripStartDate { get; set; }
        public DateTime TripEndDate { get; set; }

    }
}
