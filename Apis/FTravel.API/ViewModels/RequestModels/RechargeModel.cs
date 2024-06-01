using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class RechargeModel
    {
        [Required]
        public int RechargeAmount { get; set; } = 0;

        [Required]
        public string PaymentMethod { get; set; } = "VNPAY";
    }
}
