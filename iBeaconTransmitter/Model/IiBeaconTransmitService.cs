using System;

namespace iBeaconTransmitter.Model
{
	/// <summary>
	/// iBeacon発信用のサービス
	/// </summary>
	public interface IiBeaconTransmitService
	{
        bool TransmissionSupported();
		void StartTransmission(iBeacon ibeacon);
		void StartTransmission(Guid uuid, ushort major, ushort minor, sbyte txPower);
		void StopTransmission();
	}
}
