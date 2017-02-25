using iBeaconTransmitter.Model;
using iBeaconTransmitter.UWP.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(ResourceUtility))]
namespace iBeaconTransmitter.UWP.Model
{
    public class ResourceUtility : IResourceUtility
    {
        private string _uwpResourceRootPath = "ms-appx-web:///";

        public string GetResourceRootPath()
        {
            return _uwpResourceRootPath;
        }
    }
}
