using MbientLab.MetaWear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace MetaWearRPC
{
	public sealed class ScanConfig
	{
		internal int Duration { get; }
		internal List<Guid> ServiceUuids { get; }

		public ScanConfig(int duration = 10000, List<Guid> serviceUuids = null)
		{
			Duration = duration;
			ServiceUuids = serviceUuids == null ? new List<Guid>(new Guid[] { Constants.METAWEAR_GATT_SERVICE }) : serviceUuids;
		}
	}

	/// <summary>
	/// Page that scans for and shows nearby Bluetooth LE devices.
	/// </summary>
	public sealed class BTLEScanner
	{
		private BluetoothLEAdvertisementWatcher _btleWatcher;
		private HashSet<ulong> _seenDevices = new HashSet<ulong>();
		private HashSet<BluetoothLEDevice> _pairedDevices = new HashSet<BluetoothLEDevice>();
		private ScanConfig _config = new ScanConfig();
		private Timer _timer;

		public BTLEScanner()
		{
			_btleWatcher = new BluetoothLEAdvertisementWatcher
			{
				ScanningMode = BluetoothLEScanningMode.Active
			};

			_btleWatcher.Received += async (w, btAdv) =>
			{
				//if (!_seenDevices.Contains(btAdv.BluetoothAddress) &&
				//		_config.ServiceUuids.Aggregate(true, (acc, e) => acc & btAdv.Advertisement.ServiceUuids.Contains(e)))
				{
					_seenDevices.Add(btAdv.BluetoothAddress);
					Console.WriteLine("[BTLEScanner] BTLE device seen : " + btAdv.BluetoothAddress);
					var device = await BluetoothLEDevice.FromBluetoothAddressAsync(btAdv.BluetoothAddress);
					if (device != null)
					{
						_pairedDevices.Add(device);
						Console.WriteLine("[BTLEScanner] BTLE device paired : " + device.ToString());
						//await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => pairedDevices.Items.Add(device));
					}
				}
			};
		}

		/// <summary>
		/// Callback for the refresh button which populates the devices list
		/// </summary>
		public void RefreshDevices()
		{
			if (_timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
			_btleWatcher.Stop();

			var connected = _pairedDevices.Where(e => (e as BluetoothLEDevice).ConnectionStatus == BluetoothConnectionStatus.Connected);

			_seenDevices.Clear();
			_pairedDevices.Clear();

			foreach (var it in connected)
			{
				_seenDevices.Add((it as BluetoothLEDevice).BluetoothAddress);
				_pairedDevices.Add(it);
			}

			_btleWatcher.Start();
			_timer = new Timer(e => _btleWatcher.Stop(), null, _config.Duration, Timeout.Infinite);
		}

		/// <summary>
		/// Callback for the devices list which navigates to the <see cref="DeviceSetup"/> page with the selected device
		/// </summary>
		//private async void pairedDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
		//{
		//	btleWatcher.Stop();
		//	var item = ((ListView)sender).SelectedItem as BluetoothLEDevice;

		//	if (item != null)
		//	{
		//		ContentDialog initPopup = new ContentDialog()
		//		{
		//			Title = "Initializing API",
		//			Content = "Please wait while the app initializes the API"
		//		};

		//		initPopup.ShowAsync();
		//		var board = MbientLab.MetaWear.Win10.Application.GetMetaWearBoard(item);
		//		await board.InitializeAsync();
		//		initPopup.Hide();

		//		Frame.Navigate(config.NextPageType, item);
		//	}
		//}
	}
}
