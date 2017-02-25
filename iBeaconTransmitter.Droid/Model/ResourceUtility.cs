using iBeaconTransmitter.Model.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ResourceUtility))]
namespace iBeaconTransmitter.Model.Droid
{
	public class ResourceUtility : IResourceUtility
	{
		private const string ANDROID_RESOURCE_ROOT_PATH = "file:///android_asset/";

		public string GetResourceRootPath()
		{
			return ANDROID_RESOURCE_ROOT_PATH;
		}
	}
}
