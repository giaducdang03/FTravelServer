namespace FTravel.API.ViewModels.ResponseModels
{
    public class LoginResponseModel : ResponseModel
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
