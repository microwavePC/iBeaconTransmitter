using System;

namespace iBeaconTransmitter.Model
{
	public interface IiBeaconTransmitService
	{
        bool TransmissionSupported();
		void StartTransmission(iBeacon ibeacon);
		void StartTransmission(Guid uuid, ushort major, ushort minor, sbyte txPower);
		void StopTransmission();
	}
}
