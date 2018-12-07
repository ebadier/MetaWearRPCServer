using MbientLab.MetaWear;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Peripheral;
using System;
using System.Collections.Generic;
using Windows.Devices.Bluetooth;

namespace MetaWearRPC
{
	/// <summary>
	/// Implementation of the IMetaWearContract.
	/// </summary>
	public sealed class MetaWearContract : IMetaWearContract, IDisposable
	{
		private Dictionary<ulong, Tuple<IMetaWearBoard, BluetoothLEDevice>> _boards;

		public MetaWearContract()
		{
			_boards = new Dictionary<ulong, Tuple<IMetaWearBoard, BluetoothLEDevice>>();
		}

		public void Dispose()
		{
			foreach(var board in _boards)
			{
				CloseBoard(board.Key);
			}
		}

		private Tuple<IMetaWearBoard, BluetoothLEDevice> _GetBoard(ulong pMacAdress)
		{
			if (_boards.TryGetValue(pMacAdress, out var value))
			{
				return value;
			}

			BluetoothLEDevice ble = BluetoothLEDevice.FromBluetoothAddressAsync(pMacAdress).AsTask().RunSynchronously<BluetoothLEDevice>();
			IMetaWearBoard metaWear = MbientLab.MetaWear.Win10.Application.GetMetaWearBoard(ble);
			metaWear.OnUnexpectedDisconnect = () =>
			{
				Console.WriteLine("[MetaWearContract] Unexpected disconnection on board " + pMacAdress);
				CloseBoard(pMacAdress);
				InitBoard(pMacAdress);
			};
			var board = Tuple.Create<IMetaWearBoard, BluetoothLEDevice>(metaWear, ble);
			_boards.Add(pMacAdress, board);
			return board;
		}

		public void InitBoard(ulong pMacAdress)
		{
			Console.WriteLine("[MetaWearContract] Initializing board " + pMacAdress + "...");
			_GetBoard(pMacAdress).Item1.InitializeAsync().Wait();
			Console.WriteLine("[MetaWearContract] Board " + pMacAdress + " initialized.");
		}

		public void CloseBoard(ulong pMacAdress)
		{
			Console.WriteLine("[MetaWearContract] Closing board " + pMacAdress + "...");
			var board = _GetBoard(pMacAdress);
			if (!board.Item1.InMetaBootMode)
			{
				board.Item1.TearDown();
				board.Item1.GetModule<IDebug>().DisconnectAsync().Wait();
			}
			MbientLab.MetaWear.Win10.Application.RemoveMetaWearBoard(board.Item2, true);

			_boards.Remove(pMacAdress);
			Console.WriteLine("[MetaWearContract] Board " + pMacAdress + " closed.");
		}

		public string GetBoardModel(ulong pMacAdress)
		{
			var board = _GetBoard(pMacAdress);
			if (!board.Item1.InMetaBootMode)
			{
				return board.Item1.ModelString;
			}
			return string.Empty;
		}

		public byte GetBatteryLevel(ulong pMacAdress)
		{
			var board = _GetBoard(pMacAdress);
			if (!board.Item1.InMetaBootMode)
			{
				return board.Item1.ReadBatteryLevelAsync().RunSynchronously<byte>();
			}
			return 0;
		}

		public void StartMotor(ulong pMacAdress, ushort pDurationMs, float pIntensity)
		{
			IHaptic haptic = _GetBoard(pMacAdress).Item1.GetModule<IHaptic>();
			if (haptic != null)
			{
				haptic.StartMotor(pDurationMs, pIntensity);
			}
		}

		public void StartBuzzer(ulong pMacAdress, ushort pDurationMs)
		{
			IHaptic haptic = _GetBoard(pMacAdress).Item1.GetModule<IHaptic>();
			if (haptic != null)
			{
				haptic.StartBuzzer(pDurationMs);
			}
		}
	}
}
