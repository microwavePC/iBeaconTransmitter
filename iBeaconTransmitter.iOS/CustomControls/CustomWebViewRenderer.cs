using iBeaconTransmitter;
using iBeaconTransmitter.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace iBeaconTransmitter.iOS
{
	/// <summary>
	/// CustomWebViewのレンダラー（iOS用）
	/// </summary>
	public class CustomWebViewRenderer : WebViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			// 背景を透過させる。
			this.BackgroundColor = UIColor.Clear;
		}
	}
}
