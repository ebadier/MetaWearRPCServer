using MbientLab.MetaWear;
using MbientLab.MetaWear.Peripheral;

namespace MetaWearRPC
{
	/// <summary>
	/// Implementation of the IMetaWearContract.
	/// </summary>
	public sealed class MetaWearContract : IMetaWearContract
	{
		private MetaWearBoardsManager _mwBoardsManager;

		public MetaWearContract()
		{
			_mwBoardsManager = null;
		}

		public void Init(MetaWearBoardsManager pMetaWearBoardsManager)
		{
			_mwBoardsManager = pMetaWearBoardsManager;
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
