using System;
using System.IO;
using SnailaxDOTNET;

namespace SnailaxNETTester
{
	class Program
	{
		static void Main(string[] args)
		{
			SnailaxLevel sn = new SnailaxLevel("testlevel");
			sn.PlaceTileAtGridPosition(0,10,"obj_wall");
			sn.PlaceTileAtGridPosition(0, 9, "obj_spike_permanent");
			sn.PlaceTileAtGridPosition(1, 10, "obj_wall");
			sn.PlaceTileAtGridPosition(1, 9, "obj_playerspawn");
			sn.PlaceTileAtGridPosition(2, 10, "obj_wall");
			Console.WriteLine("Below is the generation test, it should generate a pretty generic level.");
			Console.WriteLine(sn.Serialize());
		}
	}
}
