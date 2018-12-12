using System;
using System.Collections.Generic;
using Windows.Devices.Bluetooth.Advertisement;

namespace MetaWearRPC
{
	/// <summary>
	/// Scan for Bluetooth LE devices around and advertise when some are found.
	/// </summary>
	public sealed class BLEScanner
    {
		/// <summary>
		/// Called when a BLE device is found.
		/// </summary>
		public Action<BluetoothLEAdvertisementReceivedEventArgs> BLEDeviceFound;

		/// <summary>
		/// Act as a filter to only advertise for allowed BLE devices.
		/// </summary>
		private HashSet<ulong> _allowedBleDevices;

		/// <summary>
		/// The scanner.
		/// </summary>
		private BluetoothLEAdvertisementWatcher _bleWatcher;

		/// <summary>
		/// Start the Scan.
		/// </summary>
		public void StartScanning()
		{
			_bleWatcher.Start();
			//Console.WriteLine("[BLEScanner] Scanning started.");
		}

		/// <summary>
		/// Stop the Scan.
		/// </summary>
		public void StopScanning()
		{
			_bleWatcher.Stop();
			//Console.WriteLine("[BLEScanner] Scanning stopped.");
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="pAllowedBLEDevices">Act as a filter to only advertise for allowed BLE devices (null = "no filter")</param>
		public BLEScanner(HashSet<ulong> pAllowedBLEDevices = null)
        {
			_allowedBleDevices = pAllowedBLEDevices;

			// Create Bluetooth Listener
			_bleWatcher = new BluetoothLEAdvertisementWatcher();

			_bleWatcher.ScanningMode = BluetoothLEScanningMode.Active;

			// Only activate the watcher when we're recieving values >= -80
			_bleWatcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;

			// Stop watching if the value drops below -90 (user walked away)
			_bleWatcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;

			// Register callback for when we see an advertisements
			_bleWatcher.Received += _OnAdvertisementReceived;

			// Wait 5 seconds to make sure the device is really out of range
			_bleWatcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
			_bleWatcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);
        }

		private void _OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
		{
			// Filter just the allowed devices.
			if ( (_allowedBleDevices == null) || _allowedBleDevices.Contains(eventArgs.BluetoothAddress) )
			{
				Console.WriteLine(string.Format("[BLEScanner] BLE device found {0} : {1}.", eventArgs.Advertisement.LocalName, Global.MacToString(eventArgs.BluetoothAddress)));

				if (BLEDeviceFound != null)
				{
					BLEDeviceFound(eventArgs);
				}
			}
		}
    }
}