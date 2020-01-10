using System;
using System.Threading.Tasks;
using Plugin.Geolocator;

namespace Fleet_App.Common.Services
{
    public class GeolocatorService : IGeolocatorService
    {
        public double GRXX { get; set; }

        public double GRYY { get; set; }

        public async Task GetLocationAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location = await locator.GetPositionAsync();
                GRXX = location.Latitude;
                GRYY = location.Longitude;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
