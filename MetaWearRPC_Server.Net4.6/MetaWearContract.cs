using MbientLab.MetaWear;
using MbientLab.MetaWear.Peripheral;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MetaWearRPC
{
	/// <summary>
	/// Implementation of the IMetaWearContract.
	/// </summary>
	public sealed class MetaWearContract : IMetaWearContract
	{
		private MetaWearBoardsManager _mwBoardsManager;
		private CancellationTokenSource _motorPatternCancellationToken;

		public MetaWearContract(MetaWearBoardsManager pMetaWearBoardsManager)
		{
			_mwBoardsManager = pMetaWearBoardsManager;
			_motorPatternCancellationToken = null;
		}

		public string GetBoardModel(ulong pMacAdress)
		{
			IMetaWearBoard board = _mwBoardsManager.GetBoard(pMacAdress);
			if ( (board != null) && !board.InMetaBootMode)
			{
				return board.ModelString;
			}
			return string.Empty;
		}

		public byte GetBatteryLevel(ulong pMacAdress)
		{
			IMetaWearBoard board = _mwBoardsManager.GetBoard(pMacAdress);
			if ((board != null) && !board.InMetaBootMode)
			{
				return board.ReadBatteryLevelAsync().RunSynchronously<byte>();
			}
			return 0;
		}

		public void StartMotor(ulong pMacAdress, ushort pDurationMs, float pIntensity)
		{
			IMetaWearBoard board = _mwBoardsManager.GetBoard(pMacAdress);
			if (board != null)
			{
				IHaptic haptic = board.GetModule<IHaptic>();
				if (haptic != null)
				{
					haptic.StartMotor(pDurationMs, pIntensity);
				}
			}
		}

		public void StartMotorPattern(ulong pMacAdress, ushort pDurationMs, float pIntensity, ushort pSleepMs, int pPatternIterations)
		{
			// Cancel the Previous Task if Any.
			if(_motorPatternCancellationToken != null)
			{
				_motorPatternCancellationToken.Cancel();
			}

			// Run the new Task.
			_motorPatternCancellationToken = new CancellationTokenSource();
			Task.Run(() =>
			{
				IMetaWearBoard board = _mwBoardsManager.GetBoard(pMacAdress);
				if (board != null)
				{
					IHaptic haptic = board.GetModule<IHaptic>();
					if (haptic != null)
					{
						// Set 120ms as the minimal duration between Bluetooth' stream of successives commands.
						// Below this value, pattern can be broken by pushing too frequently on the BLE connection.
						// Better solution should be to implement the vibration pattern directly on the MetaWearBoard.
						int sleep = Math.Max((int)pSleepMs, 120) + pDurationMs;
						while (pPatternIterations-- > 0)
						{
							haptic.StartMotor(pDurationMs, pIntensity);
							Thread.Sleep(sleep);
						}
					}
				}
			}, _motorPatternCancellationToken.Token);
		}

		public void StartBuzzer(ulong pMacAdress, ushort pDurationMs)
		{
			IMetaWearBoard board = _mwBoardsManager.GetBoard(pMacAdress);
			if (board != null)
			{
				IHaptic haptic = board.GetModule<IHaptic>();
				if (haptic != null)
				{
					haptic.StartBuzzer(pDurationMs);
				}
			}
		}
	}
}
