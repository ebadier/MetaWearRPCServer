using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TNT;

namespace MetaWearRPC
{
	class MetaWearRPC_Server
	{
		static void Main(string[] args)
		{
			//_MetaWearBoardsManagerTest(args);
			_ServerMain(args);
		}

		private static void _ServerMain(string[] args)
		{
			// Test.
			//ulong testMac = Global.MacFromString("F6:E9:DD:B4:CF:4A");

			try
			{
				using (MetaWearBoardsManager mwBoardsManager = new MetaWearBoardsManager(args[0]))
				{
					MetaWearContract rpcContract = new MetaWearContract();
					rpcContract.Init(mwBoardsManager);

					using (var rpcServer = TntBuilder
					.UseContract<IMetaWearContract>(rpcContract)
					.CreateTcpServer(IPAddress.Loopback, Global.ServerPort))
					{
						rpcServer.AfterConnect += RpcServer_AfterConnect;
						rpcServer.IsListening = true;
						Console.WriteLine("[MetaWearRPC_Server] Server listening to clients...");
						Console.WriteLine("[MetaWearRPC_Server] Press Esc to exit...");

						// Test.
						//using (var rpcCient = TntBuilder
						//	.UseContract<IMetaWearContract>()
						//	.CreateTcpClientConnection(IPAddress.Loopback, Global.ServerPort))
						//{

							while (true)
							{
								ConsoleKey key = Console.ReadKey().Key;

								if (key == ConsoleKey.Escape)
								{
									break;
								}
								//else if (key == ConsoleKey.NumPad0) // Test.
								//{
								//	rpcCient.Contract.StartMotor(testMac, 1000, 100.0f);
								//	Console.WriteLine("[MetaWearRPC_Server] Client sent vibration.");
								//}
								Thread.Sleep(100);
							}
						//}
					}
				}
				Console.WriteLine("[MetaWearRPC_Server] Server closing...");
				Thread.Sleep(2000);
			}
			catch (Exception e)
			{
				Console.WriteLine("[MetaWearRPC_Server] Error : " + e.Message);
				Console.ReadKey();
			}
		}

		private static void RpcServer_AfterConnect(object arg1, TNT.Api.IConnection<IMetaWearContract, TNT.Tcp.TcpChannel> arg2)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("[MetaWearRPC_Server] Client connected : " + arg2.Channel.LocalEndpointName);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		private static void _MetaWearBoardsManagerTest(string[] args)
		{
			List<string> boardMacs = new List<string>()
			{
				"F6:E9:DD:B4:CF:4A",
				"D2:80:93:BC:8C:FD",
				"DF:16:4D:D1:5D:58",
				"C2:48:ED:96:3B:74"
			};

			try
			{

				//using (MetaWearBoardsManager mwBoardsManager = new MetaWearBoardsManager(boardMacs))
				using (MetaWearBoardsManager mwBoardsManager = new MetaWearBoardsManager(args[0]))
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
				Console.ReadKey();
			}
		}
	}
}