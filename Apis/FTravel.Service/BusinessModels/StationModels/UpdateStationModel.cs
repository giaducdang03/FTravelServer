using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.StationModels
{
    public class UpdateStationModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BusCompanyId { get; set; }

        public CommonStatus Status { get; set; }

    }
}
