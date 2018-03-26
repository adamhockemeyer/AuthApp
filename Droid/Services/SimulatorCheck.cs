using System;
using Android.OS;

using AuthApp.Services;

namespace AuthApp.Droid.Services
{
    public class SimulatorCheck : ISimulatorCheck
    {
        public bool CheckIfSimulator()
        {
            if(Build.Fingerprint != null)
            {
                return Build.Fingerprint.Contains("vbox") || Build.Fingerprint.Contains("generic");
            }

            return false;
        }
    }
}
