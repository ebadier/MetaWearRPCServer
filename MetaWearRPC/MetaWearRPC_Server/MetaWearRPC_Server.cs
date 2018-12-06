using System;
using System.Net;
using System.Threading;
using TNT;

namespace MetaWearRPC
{
	//public class Unmanaged
	//{
	//	public enum RpcAuthnLevel
	//	{
	//		Default = 0,
	//		None = 1,
	//		Connect = 2,
	//		Call = 3,
	//		Pkt = 4,
	//		PktIntegrity = 5,
	//		PktPrivacy = 6
	//	}

	//	public enum RpcImpLevel
	//	{
	//		Default = 0,
	//		Anonymous = 1,
	//		Identify = 2,
	//		Impersonate = 3,
	//		Delegate = 4
	//	}

	//	public enum EoAuthnCap
	//	{
	//		None = 0x00,
	//		MutualAuth = 0x01,
	//		StaticCloaking = 0x20,
	//		DynamicCloaking = 0x40,
	//		AnyAuthority = 0x80,
	//		MakeFullSIC = 0x100,
	//		Default = 0x800,
	//		SecureRefs = 0x02,
	//		AccessControl = 0x04,
	//		AppID = 0x08,
	//		Dynamic = 0x10,
	//		RequireFullSIC = 0x200,
	//		AutoImpersonate = 0x400,
	//		NoCustomMarshal = 0x2000,
	//		DisableAAA = 0x1000
	//	}

	//	[System.Runtime.InteropServices.DllImport("ole32.dll")]
	//	public static extern int CoInitializeSecurity(IntPtr pVoid, int cAuthSvc, IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level,
	//	RpcImpLevel impers, IntPtr pAuthList, EoAuthnCap dwCapabilities, IntPtr pReserved3);
	//}

	class MetaWearRPC_Server
	{
		public const string testMacStr = "F6:E9:DD:B4:CF:4A";

		static void Main(string[] args)
		{
			try
			{
				//Unmanaged.CoInitializeSecurity(IntPtr.Zero, -1, IntPtr.Zero, IntPtr.Zero, Unmanaged.RpcAuthnLevel.Default, Unmanaged.RpcImpLevel.Identify, IntPtr.Zero, Unmanaged.EoAuthnCap.None, IntPtr.Zero);

				//ulong testMac = Global.MacFromString(testMacStr);

				using (var rpcServer = TntBuilder
					.UseContract<IMetaWearContract, MetaWearContract>()
					.CreateTcpServer(IPAddress.Loopback, Global.ServerPort))
				{
					rpcServer.AfterConnect += RpcServer_AfterConnect;

					rpcServer.IsListening = true;
					Console.WriteLine("[MetaWearRPC_Server] Server listening to clients...");
					Console.WriteLine("[MetaWearRPC_Server] Press Esc to exit...");

					using (var rpcCient = TntBuilder
						.UseContract<IMetaWearContract>()
						.CreateTcpClientConnection(IPAddress.Loopback, Global.ServerPort))

						while (true)
						{
							ConsoleKey key = Console.ReadKey().Key;

							if (key == ConsoleKey.Escape)
							{
								break;
							}
							else if(key == ConsoleKey.NumPad0)
							{
								rpcCient.Contract.CloseBoard(testMacStr);
								Console.WriteLine("[MetaWearRPC_Server] Client close board.");
							}
							else if (key == ConsoleKey.NumPad1)
							{
								rpcCient.Contract.InitBoard(testMacStr);
								Console.WriteLine("[MetaWearRPC_Server] Client init board.");
							}
							else if (key == ConsoleKey.NumPad2)
							{
								rpcCient.Contract.StartMotor(testMacStr, 1000, 100.0f);
								Console.WriteLine("[MetaWearRPC_Server] Client sent vibration.");
							}
						}

					Console.WriteLine("[MetaWearRPC_Server] Server closing...");
					Thread.Sleep(2000);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[MetaWearRPC_Server] Error : " + e.Message);
			}
		}

		private static void RpcServer_AfterConnect(object arg1, TNT.Api.IConnection<IMetaWearContract, TNT.Tcp.TcpChannel> arg2)
		{
			Console.WriteLine("[MetaWearRPC_Server] Client connected : " + arg2.Channel.LocalEndpointName);
		}
	}
}
