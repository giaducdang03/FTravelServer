using FTravel.Service.Enums;
using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CreateNotificationModel
    {
        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Message { get; set; } = "";

        public RoleEnums? RoleEnums { get; set; }

        public List<int>? UserIds {  get; set; } = new List<int>();
    }
}
