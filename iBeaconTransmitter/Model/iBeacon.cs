using System;

namespace iBeaconTransmitter
{
	/// <summary>
	/// iBeaconの情報を保持するクラス
	/// </summary>
	public class iBeacon
	{
		#region Constants

		/// <summary>
		/// Majorのデフォルト値
		/// </summary>
		public const ushort DEFAULT_MAJOR = 0;

		/// <summary>
		/// Minorのデフォルト値
		/// </summary>
		public const ushort DEFAULT_MINOR = 0;

		/// <summary>
		/// TxPowerのデフォルト値
		/// </summary>
		public const sbyte DEFAULT_TXPOWER = -59;

		#endregion

		#region Valiables

		/// <summary>
		/// iBeaconのUUID
		/// </summary>
		/// <value>iBeaconのUUID</value>
		public Guid Uuid { get; set; }

		/// <summary>
		/// iBeaconのMajor
		/// </summary>
		/// <value>iBeaconのMajor</value>
		public ushort Major { get; set; }

		/// <summary>
		/// iBeaconのMinor
		/// </summary>
		/// <value>iBeaconのMinor</value>
		public ushort Minor { get; set; }

		/// <summary>
		/// iBeaconのTxPower
		/// </summary>
		/// <value>iBeaconのTxPower</value>
		public sbyte TxPower { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// UUID、Major、Minorの全てが初期値のiBeaconとしてインスタンスを生成
		/// </summary>
		public iBeacon()
		{
			this.Uuid = Guid.Empty;
			this.Major = DEFAULT_MAJOR;
			this.Minor = DEFAULT_MINOR;
			this.TxPower = DEFAULT_TXPOWER;
		}

		/// <summary>
		/// 引数で指定されたUUID、Major、Minor、TxPowerを持つiBeaconとしてインスタンスを生成
		/// </summary>
		/// <param name="uuid">iBeaconのUUID</param>
		/// <param name="major">Major値</param>
		/// <param name="minor">Minor値</param>
		public iBeacon(Guid uuid, ushort major, ushort minor, sbyte txPower)
		{
			this.Uuid = uuid;
			this.Major = major;
			this.Minor = minor;
			this.TxPower = txPower;
		}

        #endregion

        #region Static methods

        /// <summary>
        /// 引数で渡されたUUID、Major、Minorの各文字列が
        /// ビーコンの設定値としてのフォーマットに即しているかをチェックする処理
        /// </summary>
        /// <returns>
        /// <c>true</c>, UUID、Major、Minorが全てフォーマットに即している場合, 
        /// <c>false</c> UUID、Major、Minorのどれかが誤ったフォーマットとなっている場合.
        /// </returns>
        /// <param name="uuid">UUID</param>
        /// <param name="major">Major</param>
        /// <param name="minor">Minor</param>
        /// <param name="errorMsg">チェック結果のメッセージを格納する文字列</param>
        public static bool IsValidInput(string uuid, string major, string minor, out string errorMsg)
		{
			// UUID、Major、Minorそれぞれの判定結果を変数に取得する。
			bool isValidUuid = IsValidInputForUuid(uuid);
			bool isValidMajor = IsValidInputForMajorOrMinor(major);
			bool isValidMinor = IsValidInputForMajorOrMinor(minor);

			// チェック結果のメッセージを空白で初期化する。
			errorMsg = string.Empty;

			if (isValidUuid && isValidMajor && isValidMinor)
			{
				// UUID、Major、Minorの全てがフォーマットに即しているなら、trueを返して処理を終了する。
				return true;
			}

			// UUID、Major、Minorに不正なものがある場合、不正に応じたエラーメッセージをerrMsgに積み上げる。
			if (!isValidUuid)
			{
				// UUIDが不正であった場合
				errorMsg += "UUIDのフォーマットが正しくありません。";
			}
			if (!isValidMajor)
			{
				// Majorが不正であった場合
				if (errorMsg != string.Empty)
				{
					errorMsg += Environment.NewLine;
				}
				errorMsg += "Majorに0〜65535の整数値が入力されていません。";
			}
			if (!isValidMinor)
			{
				// Minorが不正であった場合
				if (errorMsg != string.Empty)
				{
					errorMsg += Environment.NewLine;
				}
				errorMsg += "Minorに0〜65535の整数値が入力されていません。";
			}
			return false;
		}

		/// <summary>
		/// 引数で渡された文字列がUUIDとして正しいフォーマットになっているかどうかを判定する処理
		/// </summary>
		/// <returns>
		/// <c>true</c>, UUIDとして正しいフォーマットになっている場合, 
		/// <c>false</c> UUIDのフォーマットになっていない場合.
		/// </returns>
		/// <param name="strUuid">判定対象の文字列</param>
		public static bool IsValidInputForUuid(string strUuid)
		{
			Guid uuid;
			bool checkResult = Guid.TryParse(strUuid, out uuid);
			return checkResult;
		}

		/// <summary>
		/// 引数で渡された文字列がMajorまたはMinorとして正しいフォーマットになっているかどうかを判定する処理
		/// </summary>
		/// <returns>
		/// <c>true</c>, MajorやMinorとして正しいフォーマットになっている場合, 
		/// <c>false</c> MajorやMinorのフォーマットになっていない場合.
		/// </returns>
		/// <param name="strInputValue">判定対象の文字列</param>
		public static bool IsValidInputForMajorOrMinor(string strInputValue)
		{
			ushort parsedValue;
			bool checkResult = ushort.TryParse(strInputValue, out parsedValue);
			return checkResult;
		}

        #endregion
    }
}
