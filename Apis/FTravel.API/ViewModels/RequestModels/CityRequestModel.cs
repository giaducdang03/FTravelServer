namespace FTravel.API.ViewModels.RequestModels
{
    public class CityRequestModel
    {
        public string? UnsignName { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
