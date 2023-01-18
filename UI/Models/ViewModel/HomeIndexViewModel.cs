namespace UI.Models.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<BikeViewModel> Bikes { get; set; }
        public string Search { get; set; }
        public List<StationDensityViewModel> ChartModel { get; set; }
    }
}
