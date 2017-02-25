using System;
using Foundation;
using iBeaconTransmitter.Model.iOS;
using iBeaconTransmitter.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(ResourceUtility))]
namespace iBeaconTransmitter.Model.iOS
{
	public class ResourceUtility : IResourceUtility
	{
		private string _iOSResourceRootPath = NSBundle.MainBundle.BundleUrl.ToString();

		public string GetResourceRootPath()
		{
			return _iOSResourceRootPath;
		}
	}
}
