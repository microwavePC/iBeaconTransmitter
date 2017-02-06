using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
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
		string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		/// <summary>
		/// iBeaconの情報を保持するインスタンス
		/// </summary>
		iBeacon _ibeacon;

		/// <summary>
		/// iBeaconのUUID
		/// </summary>
		string _uuid;
		public string Uuid
		{
			get { return _uuid; }
			set { SetProperty(ref _uuid, value); }
		}

		/// <summary>
		/// iBeaconのMajor値
		/// </summary>
		string _major;
		public string Major
		{
			get { return _major; }
			set { SetProperty(ref _major, value); }
		}

		/// <summary>
		/// iBeaconのMinor値
		/// </summary>
		string _minor;
		public string Minor
		{
			get { return _minor; }
			set { SetProperty(ref _minor, value); }
		}

		/// <summary>
		/// 発信/停止ボタンの表示文字列
		/// </summary>
		string _buttonTitle;
		public string ButtonTitle
		{
			get { return _buttonTitle; }
			set { SetProperty(ref _buttonTitle, value); }
		}

		/// <summary>
		/// iBeaconの設定値（UUID、Major、Minor）の編集可否
		/// </summary>
		bool _canEditBeaconProperties;
		public bool CanEditBeaconProperties
		{
			get { return _canEditBeaconProperties; }
			set { SetProperty(ref _canEditBeaconProperties, value); }
		}

		/// <summary>
		/// iBeacon発信処理を持つサービス
		/// </summary>
		readonly IiBeaconTransmitService _iBeaconTransmitService;

		/// <summary>
		/// ダイアログ表示処理を扱うサービス
		/// </summary>
		readonly IPageDialogService _pageDialogService;

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
			TransmitStartStopCommand = new DelegateCommand(changeTransmitStatus);

			// TransmitStartStopCommandコマンドの実処理をchangeTransmitStatusメソッドに設定しつつ、
			// 実行可否をcanExecuteTransmitStartStopCommandで制御する。
			// canExecuteTransmitStartStopCommandの戻り値がfalseの場合はコマンド実行不可。
			/*
			TransmitStartStopCommand =
				new DelegateCommand(changeTransmitStatus, canExecuteTransmitStartStopCommand);
			*/
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
				Title = (string)parameters["title"] + " and Prism";

			// 発信/停止ボタンの表記を更新する。
			ButtonTitle = Const.STR_TRANSMIT_START;

			// iBeacon情報の変更可否を更新する。
			CanEditBeaconProperties = true;

			// iBeaconの情報を保持するためのインスタンスを作成する。
			_ibeacon = new iBeacon();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// 発信/停止ボタンの実処理
		/// </summary>
		void changeTransmitStatus()
		{
			// UUID、Major、Minorの入力が正しいかをチェックする。
			string errorMsgForBeaconInfo;
			if (iBeacon.IsValidInput(Uuid, Major, Minor, out errorMsgForBeaconInfo))
			{
				// 入力が正しい場合、iBeaconのインスタンスに各情報を設定する。
				_ibeacon.Uuid = Guid.Parse(Uuid);
				_ibeacon.Major = ushort.Parse(Major);
				_ibeacon.Minor = ushort.Parse(Minor);
			}
			else
			{
				// 入力が正しくない場合、エラーメッセージを表示する。
				// TODO: エラーダイアログ表示処理
				_pageDialogService.DisplayAlertAsync("エラー", errorMsgForBeaconInfo, "OK");
				return;
			}

			if (ButtonTitle == Const.STR_TRANSMIT_START)
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

		//*
		/// <summary>
		/// 発信/停止ボタンの呼び出し可否の制御
		/// ここで設定された条件がtrueとなった場合のみボタンの処理が続行可能になる。
		/// </summary>
		/// <returns><c>true</c>, 発信コマンド実行可能, <c>false</c> 発信コマンド実行不可</returns>
		/*
		bool canExecuteTransmitStartStopCommand()
		{
			// チェック処理
		}
		*/

		#endregion
	}
}

