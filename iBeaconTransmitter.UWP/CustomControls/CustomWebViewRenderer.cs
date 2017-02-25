using iBeaconTransmitter;
using iBeaconTransmitter.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace iBeaconTransmitter.UWP
{
	/// <summary>
	/// CustomWebViewのレンダラー（UWP用）
	/// </summary>
	public class CustomWebViewRenderer : WebViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			// 背景を透過させる。
			//this.BackgroundColor = UIColor.Clear;
		}
	}
}
