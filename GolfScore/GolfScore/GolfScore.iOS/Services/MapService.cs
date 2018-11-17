using GolfScore.iOS.Services;
using TeeScore.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(MapService))]
namespace GolfScore.iOS.Services
{
    public class MapService : IMapService
    {
        public bool IsLocationPermissionGranted => true;
    }
}