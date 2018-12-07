using MbientLab.MetaWear;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Peripheral;
using System;
using System.Collections.Generic;
//using Windows.Devices.Bluetooth;

namespace MetaWearRPC
{
	/// <summary>
	/// Implementation of the IMetaWearContract.
	/// </summary>
	public sealed class MetaWearContract : IMetaWearContract, IDisposable
	{
		//private Dictionary<ulong, Tuple<IMetaWearBoard, BluetoothLEDevice>> _boards;
		private Dictionary<string, IMetaWearBoard> _boards;

		public MetaWearContract()
		{
			//_boards = new Dictionary<ulong, Tuple<IMetaWearBoard, BluetoothLEDevice>>();
			_boards = new Dictionary<string, IMetaWearBoard>();
		}

		public void Dispose()
		{
			foreach(var board in _boards)
			{
				CloseBoard(board.Key);
			}
		}

		private IMetaWearBoard _GetBoard(string pMacAdress)
		{
			return MbientLab.MetaWear.NetStandard.Application.GetMetaWearBoard(pMacAdress);
		}

		//private Tuple<IMetaWearBoard, BluetoothLEDevice> _GetBoard(ulong pMacAdress)
		//{
		//	if (_boards.TryGetValue(pMacAdress, out var value))
		//	{
		//		return value;
		//	}

		//	BluetoothLEDevice ble = BluetoothLEDevice.FromBluetoothAddressAsync(pMacAdress).AsTask().RunSynchronously<BluetoothLEDevice>();
		//	IMetaWearBoard metaWear = MbientLab.MetaWear.Win10.Application.GetMetaWearBoard(ble);
		//	metaWear.OnUnexpectedDisconnect = () =>
		//	{
		//		Console.WriteLine("[MetaWearContract] Unexpected disconnection on board " + pMacAdress);
		//		CloseBoard(pMacAdress);
		//		InitBoard(pMacAdress);
		//	};
		//	var board = Tuple.Create<IMetaWearBoard, BluetoothLEDevice>(metaWear, ble);
		//	_boards.Add(pMacAdress, board);
		//	return board;
		//}

		public void InitBoard(string pMacAdress)
		{
			Console.WriteLine("[MetaWearContract] Initializing board " + pMacAdress + "...");
			//_GetBoard(pMacAdress).Item1.InitializeAsync().Wait();
			_GetBoard(pMacAdress).InitializeAsync().Wait();
			Console.WriteLine("[MetaWearContract] Board " + pMacAdress + " initialized.");
		}

		public void CloseBoard(string pMacAdress)
		{
			Console.WriteLine("[MetaWearContract] Closing board " + pMacAdress + "...");
			var board = _GetBoard(pMacAdress);
			//if (!board.Item1.InMetaBootMode)
			//{
			//	board.Item1.TearDown();
			//	board.Item1.GetModule<IDebug>().DisconnectAsync().Wait();
			//}
			//MbientLab.MetaWear.Win10.Application.RemoveMetaWearBoard(board.Item2, true);
			if(! board.InMetaBootMode)
			{
				board.TearDown();
				board.GetModule<IDebug>().DisconnectAsync().Wait();
			}

			_boards.Remove(pMacAdress);
			Console.WriteLine("[MetaWearContract] Board " + pMacAdress + " closed.");
		}

		public string GetBoardModel(string pMacAdress)
		{
			var board = _GetBoard(pMacAdress);
			//if(! board.Item1.InMetaBootMode)
			//{
			//	return board.Item1.ModelString;
			//}
			if(! board.InMetaBootMode)
			{
				return board.ModelString;
			}
			return string.Empty;
		}

		public byte GetBatteryLevel(string pMacAdress)
		{
			var board = _GetBoard(pMacAdress);
			//if(! board.Item1.InMetaBootMode)
			//{
			//	return board.Item1.ReadBatteryLevelAsync().RunSynchronously<byte>();
			//}
			if(! board.InMetaBootMode)
			{
				return board.ReadBatteryLevelAsync().RunSynchronously<byte>();
			}
			return 0;
		}

		public void StartMotor(string pMacAdress, ushort pDurationMs, float pIntensity)
		{
			//IHaptic haptic = _GetBoard(pMacAdress).Item1.GetModule<IHaptic>();
			IHaptic haptic = _GetBoard(pMacAdress).GetModule<IHaptic>();
			if (haptic != null)
			{
				haptic.StartMotor(pDurationMs, pIntensity);
			}
		}

		public void StartBuzzer(string pMacAdress, ushort pDurationMs)
		{
			//IHaptic haptic = _GetBoard(pMacAdress).Item1.GetModule<IHaptic>();
			IHaptic haptic = _GetBoard(pMacAdress).GetModule<IHaptic>();
			if (haptic != null)
			{
				haptic.StartBuzzer(pDurationMs);
			}
		}
	}
}
