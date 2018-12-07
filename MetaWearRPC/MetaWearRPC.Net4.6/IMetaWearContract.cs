using TNT;

namespace MetaWearRPC
{
	/// <summary>
	/// Interface (contract) for RPC client-server interaction.
	/// </summary>
	public interface IMetaWearContract
	{
		/// <summary>
		/// Initialize the MetaWearBoard with the given mac address.
		/// </summary>
		[TntMessage(1)]
		void InitBoard(string pMacAdress);

		/// <summary>
		/// Close the MetaWearBoard with the given mac address.
		/// </summary>
		[TntMessage(2)]
		void CloseBoard(string pMacAdress);

		/// <summary>
		/// Return a description of the MetaWearBoard with the given mac address.
		/// </summary>
		[TntMessage(3)]
		string GetBoardModel(string pMacAdress);

		/// <summary>
		/// Return the battery level of the MetaWearBoard with the given mac address.
		/// </summary>
		[TntMessage(4)]
		byte GetBatteryLevel(string pMacAdress);

		/// <summary>
		/// Start pulsing a motor on the MetaWearBoard with the given mac address.
		/// </summary>
		/// <param name="pDurationMs">How long to run the motor, in milliseconds (ms)</param>
		/// <param name="pIntensity">Strength of the motor [0.0f ; 100.0f]</param>
		[TntMessage(5)]
		void StartMotor(string pMacAdress, ushort pDurationMs, float pIntensity);

		/// <summary>
		/// Start pulsing a buzzer on the MetaWearBoard with the given mac address.
		/// </summary>
		/// <param name="pDurationMs">How long to run the buzzer, in milliseconds (ms)</param>
		[TntMessage(6)]
		void StartBuzzer(string pMacAdress, ushort pDurationMs);
	}
}
