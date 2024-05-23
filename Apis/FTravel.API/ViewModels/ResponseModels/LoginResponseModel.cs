namespace FTravel.API.ViewModels.ResponseModels
{
    public class LoginResponseModel
    {
        public string Message { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
