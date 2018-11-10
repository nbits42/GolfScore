using GolfScore.Contracts;
using GolfScore.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MapService))]
namespace GolfScore.iOS.Services
{
    public class MapService : IMapService
    {
        public bool IsLocationPermissionGranted => true;
    }
}