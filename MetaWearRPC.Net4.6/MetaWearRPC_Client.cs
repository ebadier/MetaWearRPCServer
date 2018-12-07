using System.Net;
using TNT;
using TNT.Api;

namespace MetaWearRPC
{
	/// <summary>
	/// Connect to a MetaWearRPC_Server to control some MetaWear's Boards.
	/// The MetaWearRPC_Server is responsible for handling the bluetooth connections that doesn't work with Unity3D on Windows platforms.
	/// https://mbientlab.com/community/discussion/2601/unity-works-perfectly-with-metawear-sensor-on-first-run-hangs-on-2nd-run
	/// Usage: 
	/// 1) Connect()
	/// 2) You can safely call IMetaWearContract's Methods between Connect() and Disconnect().
	/// 3) Disconnect()
	/// </summary>
	public sealed class MetaWearRPC_Client : IMetaWearContract
	{
		/// <summary>
		/// The client.
		/// </summary>
		private IConnection<IMetaWearContract, TNT.Tcp.TcpChannel> _client;

		/// <summary>
		/// Return wether the client is currently connected to the RPC Server.
		/// </summary>
		public bool IsConnected { get { return (_client != null) && _client.Channel.IsConnected; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		public MetaWearRPC_Client()
		{
			_client = null;
		}

		/// <summary>
		/// Connect to the RPC Server.
		/// </summary>
		public void Connect(string pServerIPAddress)
		{
			IPAddress ip = IPAddress.Parse(pServerIPAddress);
			_client = TntBuilder.UseContract<IMetaWearContract>().CreateTcpClientConnection(ip, Global.ServerPort);
		}

		/// <summary>
		/// Disconnect from the RPC Server.
		/// </summary>
		public void Disconnect()
		{
			if (_client != null)
			{
				_client.Dispose();
				_client = null;
			}
		}

		#region IMetaWearContract
		public void InitBoard(ulong pMacAdress)
		{
			_client.Contract.InitBoard(pMacAdress);
		}

		public void CloseBoard(ulong pMacAdress)
		{
			_client.Contract.CloseBoard(pMacAdress);
		}

		public string GetBoardModel(ulong pMacAdress)
		{
			return _client.Contract.GetBoardModel(pMacAdress);
		}

		public byte GetBatteryLevel(ulong pMacAdress)
		{
			return _client.Contract.GetBatteryLevel(pMacAdress);
		}

		public void StartMotor(ulong pMacAdress, ushort pDurationMs, float pIntensity)
		{
			_client.Contract.StartMotor(pMacAdress, pDurationMs, pIntensity);
		}

		public void StartBuzzer(ulong pMacAdress, ushort pDurationMs)
		{
			_client.Contract.StartBuzzer(pMacAdress, pDurationMs);
		}
		#endregion
	}
}
