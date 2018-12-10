using System.Globalization;
using System.Threading.Tasks;

namespace MetaWearRPC
{
	public static class Global
	{
		/// <summary>
		/// Server port. 
		/// Make sure to unblock this in your router firewall if you want to allow external connections.
		/// </summary>
		public const int ServerPort = 28550;

		/// <summary>
		/// Convert a Mac Address of Type "F6:E9:DD:B4:CF:4A" to one of type F6E9DDB4CF4A.
		/// </summary>
		public static ulong MacFromString(string pMac)
		{
			return ulong.Parse(pMac.Replace(":", ""), NumberStyles.HexNumber);
		}

		/// <summary>
		/// Convert a Mac Address of Type F6E9DDB4CF4A to one of type "F6:E9:DD:B4:CF:4A".
		/// </summary>
		public static string MacToString(ulong pMac)
		{
			return pMac.ToString("X").Insert(2, ":").Insert(5, ":").Insert(8, ":").Insert(11, ":").Insert(14, ":");
		}

		/// <summary>
		/// Run a task having a return value synchronously.
		/// </summary>
		public static T RunSynchronously<T>(this Task<T> task)
		{
			Task.Run(() => task.Wait());
			return task.Result;
		}
	}
}
