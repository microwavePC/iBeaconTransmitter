namespace iBeaconTransmitter.Common
{
	/// <summary>
	/// 共通定数定義クラス
	/// </summary>
	public static class Const
	{
		/// <summary>
		/// 発信/停止ボタンの表記『発信』
		/// </summary>
		public const string STR_TRANSMIT_START = "発信";

		/// <summary>
		/// 発信/停止ボタンの表記『停止』
		/// </summary>
		public const string STR_TRANSMIT_STOP = "停止";

		/// <summary>
		/// エラーダイアログのタイトル
		/// </summary>
		public const string STR_DIALOG_TITLE_ERROR = "エラー";

        /// <summary>
        /// エラーダイアログのメッセージ（BLEの発信ができない場合）
        /// </summary>
        public const string STR_DIALOG_MSG_CANNOT_TRANSMIT = "この端末はBluetooth Low Energyの発信に対応していないか、Bluetooth機能がオフにされています。";

		/// <summary>
		/// エラーダイアログのOKボタンの表記
		/// </summary>
		public const string STR_DIALOG_BUTTON_OK = "OK";

		/// <summary>
		/// Appleが使用しているiBeaconの会社コード
		/// </summary>
		public const byte COMPANY_CODE_APPLE = 0x004C;
	}
}
