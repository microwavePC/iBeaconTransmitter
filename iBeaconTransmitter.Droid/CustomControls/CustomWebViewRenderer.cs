using iBeaconTransmitter;
using iBeaconTransmitter.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace iBeaconTransmitter.Droid
{
	/// <summary>
	/// CustomWebViewのレンダラー（Android用）
	/// </summary>
	public class CustomWebViewRenderer : WebViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
		{
			base.OnElementChanged(e);

			// 背景を透過させる。
			this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}
