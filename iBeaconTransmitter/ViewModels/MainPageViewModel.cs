using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Windows.Input;
using iBeaconTransmitter.Model;
using iBeaconTransmitter.Common;
using System.Diagnostics;
using Prism.Services;

namespace iBeaconTransmitter.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		#region Variables

		/// <summary>
		/// アプリのタイトル
		/// </summary>
		private string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		/// <summary>
		/// iBeaconの情報を保持するインスタンス
		/// </summary>
		private iBeacon _ibeacon;

		/// <summary>
		/// iBeaconのUUID
		/// </summary>
		private string _uuid;
		public string Uuid
		{
			get { return _uuid; }
			set { SetProperty(ref _uuid, value); }
		}

		/// <summary>
		/// iBeaconのMajor値
		/// </summary>
		private string _major;
		public string Major
		{
			get { return _major; }
			set { SetProperty(ref _major, value); }
		}

		/// <summary>
		/// iBeaconのMinor値
		/// </summary>
		private string _minor;
		public string Minor
		{
			get { return _minor; }
			set { SetProperty(ref _minor, value); }
		}

		/// <summary>
		/// iBeaconのTxPower
		/// </summary>
		private sbyte _txPower;
		public sbyte TxPower
		{
			get { return _txPower; }
			set { SetProperty(ref _txPower, value);}
		}

		/// <summary>
		/// 発信/停止ボタンの表示文字列と、ボタンの状態を保持するbool変数
		/// </summary>
		private string _buttonTitle;
		private bool _buttonTitleIsTransmitStart;
		public string ButtonTitle
		{
			get { return _buttonTitle; }
			set
			{
				SetProperty(ref _buttonTitle, value);
				if (value == Const.STR_TRANSMIT_START)
				{
					_buttonTitleIsTransmitStart = true;
				}
				else
				{
					_buttonTitleIsTransmitStart = false;
				}
			}
		}

		/// <summary>
		/// iBeaconの設定値（UUID、Major、Minor）の編集可否
		/// </summary>
		private bool _canEditBeaconProperties;
		public bool CanEditBeaconProperties
		{
			get { return _canEditBeaconProperties; }
			set { SetProperty(ref _canEditBeaconProperties, value); }
		}

		/// <summary>
		/// iBeacon発信処理を持つサービス
		/// </summary>
		private readonly IiBeaconTransmitService _iBeaconTransmitService;

		/// <summary>
		/// ダイアログ表示処理を扱うサービス
		/// </summary>
		private readonly IPageDialogService _pageDialogService;

		/// <summary>
		/// iBeacon発信/停止を制御するコマンド
		/// </summary>
		public ICommand TransmitStartStopCommand { get; }

		#endregion

		#region Constructor

		/// <summary>
		/// iBeacon発信処理メイン画面のコンストラクタ
		/// </summary>
		/// <param name="ibeaconTransmitService">iBeacon発信処理を持つモデル</param>
		public MainPageViewModel(IiBeaconTransmitService ibeaconTransmitService, IPageDialogService pageDialogService)
		{
			// ダイアログ表示処理を扱うサービスをViewModelのクラスに保持する。
			_pageDialogService = pageDialogService;

			// iBeacon発信処理を持つモデルをViewModelのクラスに保持する。
			_iBeaconTransmitService = ibeaconTransmitService;

			// TransmitStartStopCommandコマンドの実処理をchangeTransmitStatusメソッドに設定する。
			//TransmitStartStopCommand = new DelegateCommand(changeTransmitStatus);

			// TransmitStartStopCommandコマンドの実処理をchangeTransmitStatusメソッドに設定しつつ、
			// 実行可否をcanExecuteTransmitStartStopCommandで制御する。
			// canExecuteTransmitStartStopCommandの戻り値がfalseの場合はコマンド実行不可。
			TransmitStartStopCommand =
				new DelegateCommand(changeTransmitStatus, canExecuteTransmitStartStopCommand);
		}

		#endregion

		#region Public methods

		/// <summary>
		/// メインページ表示開始時に実行される処理
		/// </summary>
		/// <param name="parameters">App.xaml.csのOnInitializedで設定されたパラメーター</param>
		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		/// <summary>
		/// メインページ表示完了後に実行される処理
		/// </summary>
		/// <param name="parameters">App.xaml.csのOnInitializedで設定されたパラメーター</param>
		public void OnNavigatedTo(NavigationParameters parameters)
		{
			// アプリのタイトルを取得する。
			if (parameters.ContainsKey("title"))
			{
				Title = (string)parameters["title"] + " and Prism";
			}

			// TxPowerの初期値を設定する。
			TxPower = iBeacon.DEFAULT_TXPOWER;

			// 発信/停止ボタンの表記を更新する。
			ButtonTitle = Const.STR_TRANSMIT_START;

			// iBeacon情報の変更を可能にする。
			CanEditBeaconProperties = true;

			// iBeaconの情報を保持するためのインスタンスを作成する。
			_ibeacon = new iBeacon();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// 発信/停止ボタンの実処理
		/// </summary>
		private void changeTransmitStatus()
		{
			// UUID、Major、Minorの入力が正しいかをチェックする。
			string errorMsgForBeaconInfo;
			if (iBeacon.IsValidInput(Uuid, Major, Minor, out errorMsgForBeaconInfo))
			{
				// 入力が正しい場合、iBeaconのインスタンスに各情報を設定する。
				_ibeacon.Uuid = Guid.Parse(Uuid);
				_ibeacon.Major = ushort.Parse(Major);
				_ibeacon.Minor = ushort.Parse(Minor);
				_ibeacon.TxPower = TxPower;
			}
			else
			{
				// 入力が正しくない場合、エラーメッセージを表示する。
				_pageDialogService.DisplayAlertAsync(Const.STR_DIALOG_TITLE_ERROR,
				                                     errorMsgForBeaconInfo,
				                                     Const.STR_DIALOG_BUTTON_OK);
				return;
			}

			if (_buttonTitleIsTransmitStart)
			{
				// iBeaconの発信を開始する。
				try
				{
					_iBeaconTransmitService.StartTransmission(_ibeacon);

					// 発信/停止ボタンの表記を「停止」に変更し、ビーコン情報入力欄を利用不可にする。
					ButtonTitle = Const.STR_TRANSMIT_STOP;
					CanEditBeaconProperties = false;
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
			}
			else
			{
				// iBeaconを停止する。
				try
				{
					_iBeaconTransmitService.StopTransmission();

					// 発信/停止ボタンの表記を「発信」に変更し、ビーコン情報入力欄を利用可能にする。
					ButtonTitle = Const.STR_TRANSMIT_START;
					CanEditBeaconProperties = true;
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
			}
		}
        
		/// <summary>
		/// 発信/停止ボタンの呼び出し可否の制御
		/// ここで設定された条件がtrueとなった場合のみボタンの処理が続行可能になる。
		/// </summary>
		/// <returns><c>true</c>, 発信コマンド実行可能, <c>false</c> 発信コマンド実行不可</returns>
		private bool canExecuteTransmitStartStopCommand()
		{
            // 端末がBLEの発信に対応しているかどうかをチェックする。
            if (!_iBeaconTransmitService.TransmissionSupported())
            {
                // BLEの発信ができない旨のエラーダイアログを表示する。
                _pageDialogService.DisplayAlertAsync(Const.STR_DIALOG_TITLE_ERROR,
                                                     Const.STR_DIALOG_MSG_CANNOT_TRANSMIT,
                                                     Const.STR_DIALOG_BUTTON_OK);
                // iBeacon情報の変更ができないようにする。
                CanEditBeaconProperties = false;
                return false;
            }

            return true;
        }

		#endregion
	}
}

