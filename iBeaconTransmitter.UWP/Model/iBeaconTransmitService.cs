using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Advertisement;
using Xamarin.Forms;
using iBeaconTransmitter.Common;
using iBeaconTransmitter.Model;
using iBeaconTransmitter.UWP.Model;

[assembly: Dependency(typeof(iBeaconTransmitService))]
namespace iBeaconTransmitter.UWP.Model
{
    public class iBeaconTransmitService : IiBeaconTransmitService
    {
        #region Variables

        /// <summary>
		/// BLEの発信を制御するクラスのインスタンス
		/// </summary>
        private BluetoothLEAdvertisementPublisher blePublisher = new BluetoothLEAdvertisementPublisher();

        #endregion

        #region Public methods

        /// <summary>
        /// iBeacon発信開始処理（引数で渡されたiBeaconの定義に従い発信）
        /// </summary>
        /// <param name="ibeacon">iBeaconの定義</param>
        public void StartTransmission(iBeacon ibeacon)
        {
            this.StartTransmission(ibeacon.Uuid, ibeacon.Major, ibeacon.Minor, ibeacon.TxPower);
        }

        /// <summary>
        /// iBeacon発信開始処理（UUID、Major、Minorを直指定して発信）
        /// </summary>
        /// <param name="uuid">UUID</param>
        /// <param name="major">Major</param>
        /// <param name="minor">Minor</param>
        public void StartTransmission(Guid uuid, ushort major, ushort minor, sbyte txPower)
        {
            // BLE発信用のデータを格納する箱を作成
            BluetoothLEManufacturerData bleManufacturerData = new BluetoothLEManufacturerData()
            {
                CompanyId = Const.COMPANY_CODE_APPLE
            };

            // データを格納できるようにするため、UUID、Major、Minor、TxPowerをbyteに変換する。
            // TxPowerはオフセットをかけてから変換する。
            byte[] uuidByteArray = uuid.ToByteArray();
            byte[] majorByteArray = BitConverter.GetBytes(major);
            byte[] minorByteArray = BitConverter.GetBytes(minor);
            byte txPowerByte = (byte)((int)txPower + 256);

            // UUID、Major、Minorを、iBeaconのフォーマットに準拠する形に梱包する。
            byte[] ibeaconAdvertisementDataArray = new byte[] {
                // iBeaconと識別するための固定値
                0x02, 0x15,
                // UUID
                uuidByteArray[0], uuidByteArray[1], uuidByteArray[2], uuidByteArray[3],
                uuidByteArray[4], uuidByteArray[5], uuidByteArray[6], uuidByteArray[7],
                uuidByteArray[8], uuidByteArray[9], uuidByteArray[10], uuidByteArray[11],
                uuidByteArray[12], uuidByteArray[13], uuidByteArray[14], uuidByteArray[15],
                // Major
                majorByteArray[0], majorByteArray[1],
                // Minor
                minorByteArray[0], minorByteArray[1],
                // TxPower
                txPowerByte
            };

            // データをBLEで発信する。
            bleManufacturerData.Data = ibeaconAdvertisementDataArray.AsBuffer();
            blePublisher.Advertisement.ManufacturerData.Add(bleManufacturerData);
            blePublisher.Start();
        }

        /// <summary>
        /// iBeaconの発信を停止する処理
        /// </summary>
        public void StopTransmission()
        {
            blePublisher.Stop();
        }

        #endregion
    }
}
