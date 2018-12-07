using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Bluetooth;

namespace MetaWearRPC
{
	/// <summary>
	/// Manages a pool of BLE devices.
	/// Ensure a list of desired BLE devices stay always connected to the application.
	/// </summary>
	public sealed class BLEDevicesManager : IDisposable
	{
		private BLEScanner _bleScanner;
		private HashSet<ulong> _desiredBLEDevices;

		private List<ulong> _connectedBLEDAddresses;
		private List<BluetoothLEDevice> _connectedBLEDevices;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="pConfigFilePath">A file containing a list of desired BLE mac addresses.</param>
		/// Example of file content:
		/// F6:E9:DD:B4:CF:4A
		/// D2:80:93:BC:8C:FD
		/// DF:16:4D:D1:5D:58
		/// C2:48:ED:96:3B:74
		public BLEDevicesManager(string pConfigFilePath)
		{
			if(System.IO.File.Exists(pConfigFilePath))
			{
				string content = System.IO.File.ReadAllText(pConfigFilePath);
				List<string> macs = content.Split('\n').ToList();
				macs.RemoveAll(mac => string.IsNullOrEmpty(mac));
				_Init(macs);
			}
			else
			{
				throw new System.IO.FileNotFoundException("File not found : " + pConfigFilePath);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="pBLEDevicesMacAdresses">The Mac Adresses of the BLEDevices we want to manage.</param>
		public BLEDevicesManager(List<string> pBLEDevicesMacAdresses)
		{
			_Init(pBLEDevicesMacAdresses);
		}

		public void Dispose()
		{
			_bleScanner.StopScanning();
		}

		private void _Init(List<string> pBLEDevicesMacAdresses)
		{
			_connectedBLEDAddresses = new List<ulong>();
			_connectedBLEDevices = new List<BluetoothLEDevice>();

			_desiredBLEDevices = new HashSet<ulong>();
			foreach (string mac in pBLEDevicesMacAdresses)
			{
				_desiredBLEDevices.Add(Global.MacFromString(mac));
			}

			// Only the desired BLE devices are allowed to be scanned.
			_bleScanner = new BLEScanner(_desiredBLEDevices);
			_bleScanner.BLEDeviceFound += _OnBLEDeviceFound;
			_bleScanner.StartScanning();
		}

		private void _OnBLEDeviceFound(BluetoothLEDevice pBLEDevice)
		{
			Console.WriteLine(string.Format("[BLEDevicesManager] BLE device found: BT_ADDR:{0} ; NAME:{1} ; STATUS:{2}", 
				pBLEDevice.BluetoothAddress, pBLEDevice.DeviceInformation.Name, pBLEDevice.ConnectionStatus));

			if (!_connectedBLEDAddresses.Contains(pBLEDevice.BluetoothAddress))
			{
				_connectedBLEDAddresses.Add(pBLEDevice.BluetoothAddress);
				_connectedBLEDevices.Add(pBLEDevice);
				pBLEDevice.ConnectionStatusChanged += _OnConnectionStatusChanged;
			}

			// Check for stopping Scan.
			if(_desiredBLEDevices.SetEquals(_connectedBLEDAddresses))
			{
				Console.WriteLine("[BLEDevicesManager] All BLE devices found. Stopping scan...");
				_bleScanner.StopScanning();
			}
		}

		private void _OnConnectionStatusChanged(BluetoothLEDevice sender, object args)
		{
			if(sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
			{
				int index = _connectedBLEDAddresses.IndexOf(sender.BluetoothAddress);
				if(index != -1)
				{
					_connectedBLEDAddresses.RemoveAt(index);
					_connectedBLEDevices.RemoveAt(index);
				}

				// Check for restarting Scan.
				if (!_desiredBLEDevices.SetEquals(_connectedBLEDAddresses))
				{
					Console.WriteLine("[BLEDevicesManager] Some BLE devices missed. Restarting scan...");
					_bleScanner.StartScanning();
				}
			}
		}
	}
}
