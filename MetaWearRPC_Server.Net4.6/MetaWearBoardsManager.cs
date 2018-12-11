using MbientLab.MetaWear;
using MbientLab.MetaWear.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Bluetooth;

namespace MetaWearRPC
{
	/// <summary>
	/// Manages a pool of MetaWear boards.
	/// The list of desired MetaWear boards stays always connected to the application.
	/// </summary>
	public sealed class MetaWearBoardsManager : IDisposable
	{
		private BLEScanner _bleScanner;
		private HashSet<ulong> _desiredMWBoards;
		private Dictionary<ulong, Tuple<IMetaWearBoard, BluetoothLEDevice>> _connectedMWBoards;

		/// <summary>
		/// Return the MetaWear board with the given mac address.
		/// </summary>
		/// <param name="pMacAdress"></param>
		/// <returns>null if the MetaWear board is not detected by a BLEAdapter on this computer. 
		/// Otherwise returns a fully initialized MetaWear board</returns>
		public IMetaWearBoard GetBoard(ulong pMacAdress)
		{
			if(_connectedMWBoards.TryGetValue(pMacAdress, out Tuple<IMetaWearBoard, BluetoothLEDevice> board))
			{
				return board.Item1;
			}
			return null;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="pConfigFilePath">A file containing a list of desired MetaWear boards' mac addresses.</param>
		/// Example of file content:
		/// F6:E9:DD:B4:CF:4A
		/// D2:80:93:BC:8C:FD
		/// DF:16:4D:D1:5D:58
		/// C2:48:ED:96:3B:74
		public MetaWearBoardsManager(string pConfigFilePath)
		{
			pConfigFilePath = System.IO.Path.GetFullPath(pConfigFilePath);
			string content = System.IO.File.ReadAllText(pConfigFilePath);
			List<string> macs = content.Split('\n').ToList();
			macs.RemoveAll(mac => string.IsNullOrEmpty(mac));
			_Init(macs);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="pMWBoardsMacAdresses">The Mac Adresses of the MetaWear boards we want to manage.</param>
		public MetaWearBoardsManager(List<string> pMWBoardsMacAdresses)
		{
			_Init(pMWBoardsMacAdresses);
		}

		public void Dispose()
		{
			_bleScanner.StopScanning();
			_bleScanner.BLEDeviceFound -= _OnBLEDeviceFound;

			foreach (var board in _connectedMWBoards.Values)
			{
				board.Item2.ConnectionStatusChanged -= _OnConnectionStatusChanged;
				_CloseBoard(board.Item1, board.Item2, true);
			}
			_connectedMWBoards.Clear();
		}

		#region Private Methods
		private void _Init(List<string> pMWBoardsMacAdresses)
		{
			_connectedMWBoards = new Dictionary<ulong, Tuple<IMetaWearBoard, BluetoothLEDevice>>();

			_desiredMWBoards = new HashSet<ulong>();
			foreach (string mac in pMWBoardsMacAdresses)
			{
				_desiredMWBoards.Add(Global.MacFromString(mac));
			}

			// Only the desired MetaWear boards are allowed to be scanned.
			_bleScanner = new BLEScanner(_desiredMWBoards);
			_bleScanner.BLEDeviceFound += _OnBLEDeviceFound;
			_bleScanner.StartScanning();
		}

		private async void _OnBLEDeviceFound(BluetoothLEDevice pBLEDevice)
		{
			if (!_connectedMWBoards.Keys.Contains(pBLEDevice.BluetoothAddress))
			{
				IMetaWearBoard mwBoard = MbientLab.MetaWear.Win10.Application.GetMetaWearBoard(pBLEDevice);
				await mwBoard.InitializeAsync();
				if (mwBoard.IsConnected)
				{
					Console.WriteLine(string.Format("[MetaWearBoardsManager] MetaWear board {0} {1}.",
						Global.MacToString(pBLEDevice.BluetoothAddress), pBLEDevice.ConnectionStatus));

					_connectedMWBoards.Add(pBLEDevice.BluetoothAddress, new Tuple<IMetaWearBoard, BluetoothLEDevice>(mwBoard, pBLEDevice));
					pBLEDevice.ConnectionStatusChanged += _OnConnectionStatusChanged;
				}
			}

			// Check for restarting Scan.
			_CheckForScanning();
		}

		private void _OnConnectionStatusChanged(BluetoothLEDevice sender, object args)
		{
			if(sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
			{
				if (_connectedMWBoards.TryGetValue(sender.BluetoothAddress, out Tuple<IMetaWearBoard, BluetoothLEDevice> board))
				{
					Console.WriteLine(string.Format("[MetaWearBoardsManager] MetaWear board {0} {1}.",
						Global.MacToString(sender.BluetoothAddress), sender.ConnectionStatus));

					_connectedMWBoards.Remove(sender.BluetoothAddress);
					_CloseBoard(board.Item1, board.Item2, false);
				}

				// Check for restarting Scan.
				_CheckForScanning();
			}
		}

		private void _CheckForScanning()
		{
			if (!_desiredMWBoards.SetEquals(_connectedMWBoards.Keys))
			{
				Console.WriteLine("[MetaWearBoardsManager] Some MetaWear boards missed. Restarting BLE scan...");
				_bleScanner.StartScanning();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("[MetaWearBoardsManager] All MetaWear boards connected.");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		private async void _CloseBoard(IMetaWearBoard pBoard, BluetoothLEDevice pBLEDevice, bool pTearDown)
		{
			if (!pBoard.InMetaBootMode)
			{
				if(pTearDown)
				{
					pBoard.TearDown();
				}
				await pBoard.GetModule<IDebug>().DisconnectAsync();
			}

			if(pTearDown)
			{
				MbientLab.MetaWear.Win10.Application.RemoveMetaWearBoard(pBLEDevice, true);
			}
		}
		#endregion
	}
}