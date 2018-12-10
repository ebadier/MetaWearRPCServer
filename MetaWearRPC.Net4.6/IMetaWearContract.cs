using TNT;

namespace MetaWearRPC
{
	/// <summary>
	/// Interface (contract) for RPC client-server interaction.
	/// </summary>
	public interface IMetaWearContract
	{
		/// <summary>
		/// Return a description of the MetaWearBoard with the given mac address.
		/// </summary>
		[TntMessage(1)]
		string GetBoardModel(ulong pMacAdress);

		/// <summary>
		/// Return the battery level of the MetaWearBoard with the given mac address.
		/// </summary>
		[TntMessage(2)]
		byte GetBatteryLevel(ulong pMacAdress);

		/// <summary>
		/// Start pulsing a motor on the MetaWearBoard with the given mac address.
		/// </summary>
		/// <param name="pDurationMs">How long to run the motor, in milliseconds (ms)</param>
		/// <param name="pIntensity">Strength of the motor [0.0f ; 100.0f]</param>
		[TntMessage(3)]
		void StartMotor(ulong pMacAdress, ushort pDurationMs, float pIntensity);

		/// <summary>
		/// Start pulsing a buzzer on the MetaWearBoard with the given mac address.
		/// </summary>
		/// <param name="pDurationMs">How long to run the buzzer, in milliseconds (ms)</param>
		[TntMessage(4)]
		void StartBuzzer(ulong pMacAdress, ushort pDurationMs);
	}
}
