using iBeaconTransmitter.Model;
using iBeaconTransmitter.UWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(iBeaconTransmitService))]
namespace iBeaconTransmitter.UWP.Model
{
    public class iBeaconTransmitService : IiBeaconTransmitService
    {
        public void StartTransmission(iBeacon ibeacon)
        {
            throw new NotImplementedException();
        }

        public void StartTransmission(Guid uuid, ushort major, ushort minor, sbyte txPower)
        {
            throw new NotImplementedException();
        }

        public void StopTransmission()
        {
            throw new NotImplementedException();
        }
    }
}
