using System;
using System.Threading;

namespace MetaWearRPC
{
	class MetaWearRPC_Server2
	{
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("[MetaWearRPC_Server2] Scanning for BTLE devices...");
				Console.WriteLine("[MetaWearRPC_Server2] Press Esc to exit...");

				BTLEScanner scanner = new BTLEScanner();

				while (true)
				{
					ConsoleKey key = Console.ReadKey().Key;

					if (key == ConsoleKey.Escape)
					{
						break;
					}
					else if (key == ConsoleKey.NumPad0)
					{
						Console.WriteLine("[MetaWearRPC_Server2] Refreshing BTLE devices...");
						scanner.RefreshDevices();
					}
				}

				Console.WriteLine("[MetaWearRPC_Server2] Server closing...");
				Thread.Sleep(2000);
			}
			catch (Exception e)
			{
				Console.WriteLine("[MetaWearRPC_Server2] Error : " + e.Message);
			}
		}
	}
}
