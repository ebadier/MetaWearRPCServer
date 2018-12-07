using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TNT;

namespace MetaWearRPC
{
	class MetaWearRPC_Server
	{
		// BLEDevicesManager Test.
		public static List<string> boardMacs = new List<string>()
		{
			"F6:E9:DD:B4:CF:4A",
			"D2:80:93:BC:8C:FD",
			"DF:16:4D:D1:5D:58",
			"C2:48:ED:96:3B:74"
		};

		static void Main(string[] args)
		{
			try
			{
				using (BLEDevicesManager bleDeviceManager = new BLEDevicesManager(boardMacs))
				{
					while (true)
					{
						ConsoleKey key = Console.ReadKey().Key;
						if (key == ConsoleKey.Escape)
						{
							break;
						}
						Thread.Sleep(100);
					}
				}
				Console.WriteLine("[MetaWearRPC_Server] Server closing...");
				Thread.Sleep(2000);
			}
			catch (Exception e)
			{
				Console.WriteLine("[MetaWearRPC_Server] Error : " + e.Message);
			}
		}

		// RPC Server Test.
		//public const string testMacStr = "F6:E9:DD:B4:CF:4A";
		//static void Main(string[] args)
		//{
		//	try
		//	{
		//		using (var rpcServer = TntBuilder
		//			.UseContract<IMetaWearContract, MetaWearContract>()
		//			.CreateTcpServer(IPAddress.Loopback, Global.ServerPort))
		//		{
		//			rpcServer.AfterConnect += RpcServer_AfterConnect;

		//			rpcServer.IsListening = true;
		//			Console.WriteLine("[MetaWearRPC_Server] Server listening to clients...");
		//			Console.WriteLine("[MetaWearRPC_Server] Press Esc to exit...");

		//			using (var rpcCient = TntBuilder
		//				.UseContract<IMetaWearContract>()
		//				.CreateTcpClientConnection(IPAddress.Loopback, Global.ServerPort))

		//				while (true)
		//				{
		//					ConsoleKey key = Console.ReadKey().Key;

		//					if (key == ConsoleKey.Escape)
		//					{
		//						break;
		//					}
		//					else if(key == ConsoleKey.NumPad0)
		//					{
		//						rpcCient.Contract.CloseBoard(Global.MacFromString(testMacStr));
		//						Console.WriteLine("[MetaWearRPC_Server] Client close board.");
		//					}
		//					else if (key == ConsoleKey.NumPad1)
		//					{
		//						rpcCient.Contract.InitBoard(Global.MacFromString(testMacStr));
		//						Console.WriteLine("[MetaWearRPC_Server] Client init board.");
		//					}
		//					else if (key == ConsoleKey.NumPad2)
		//					{
		//						rpcCient.Contract.StartMotor(Global.MacFromString(testMacStr), 1000, 100.0f);
		//						Console.WriteLine("[MetaWearRPC_Server] Client sent vibration.");
		//					}
		//				}

		//			Console.WriteLine("[MetaWearRPC_Server] Server closing...");
		//			Thread.Sleep(2000);
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		Console.WriteLine("[MetaWearRPC_Server] Error : " + e.Message);
		//	}
		//}

		//private static void RpcServer_AfterConnect(object arg1, TNT.Api.IConnection<IMetaWearContract, TNT.Tcp.TcpChannel> arg2)
		//{
		//	Console.WriteLine("[MetaWearRPC_Server] Client connected : " + arg2.Channel.LocalEndpointName);
		//}
	}
}
