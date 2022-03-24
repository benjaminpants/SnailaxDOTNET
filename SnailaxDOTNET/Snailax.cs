using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SnailaxDOTNET
{

	public class SnailaxLevel
	{

		public string Name = "my_epic_level";

		public SquidProperties SquidProperties;

		public byte OriginalVersion;

		public Themes Theme = Themes.Default;

		public Songs Song = Songs.JumpAndDie;

		public float RoomMultiplierX = 1f;

		public float RoomMultiplierY = 1f;

		public List<Tile> Tiles = new List<Tile>();

		public int RoomSizeX
		{
			get
			{
				return (int)Math.Round(1920f * RoomMultiplierX);
			}
		}

		public int RoomSizeY
		{
			get
			{
				return (int)Math.Round(1080f * RoomMultiplierY);
			}
		}


		public override string ToString()
		{
			return Name + " - " + Tiles.Count;
		}


		public SnailaxLevel(string name, List<Tile> tiles)
		{
			Name = name;
			Tiles = tiles;
		}

		public SnailaxLevel(string name)
		{
			Name = name;
		}

		public SnailaxLevel(string name, string leveldata)
		{
			Name = name;
			leveldata = leveldata.Replace("\r", string.Empty);
			string[] all_data_sections = leveldata.Split(new char[] { '-' }, 2);
			string header = all_data_sections[0];
			string[] header_data = header.Split('\n');
			byte version_id = byte.Parse(header_data[0]);
			OriginalVersion = version_id;

			if (OriginalVersion > Snailax.LatestLevelFormat)
			{
				throw new NotImplementedException("Latest supported format version is: " + Snailax.LatestLevelFormat + " however, this file is version " + OriginalVersion + "!");
			}

			SquidProperties sp = new SquidProperties(0f,0f,0f,0f,0f,false,0f,1f,0f,0f);

			if (version_id == 1)
			{
				version_id = 2; //the currents settings are already right
			}
			else
			{
				sp.GroundSpikeProbability = float.Parse(header_data[1]) * 100f;
				sp.WallSpikeProbablity = float.Parse(header_data[2]) * 100f;
				sp.CeilingSpikeProbability = float.Parse(header_data[3]) * 100f;
				sp.AirSpikeProbability = float.Parse(header_data[4]) * 100f;
				sp.BulletProbablity = float.Parse(header_data[5]) * 100f;
				sp.CompletelyDisabled = int.Parse(header_data[6]) == 1;
				sp.IceSpikeFallProbability = float.Parse(header_data[7]) * 100f;
				sp.FireworksProbability = float.Parse(header_data[8]) * 100f;
				sp.ConveyerBeltChangeProbability = float.Parse(header_data[9]) * 100f;
				sp.LaserProbability = float.Parse(header_data[10]) * 100f;
			}
			if (version_id == 2)
			{
				version_id = 3;
			}
			else
			{
				RoomMultiplierX = float.Parse(header_data[11]);
				RoomMultiplierY = float.Parse(header_data[12]);

			}
			if (version_id == 3)
			{
				version_id = 4;
			}
			else
			{
				Song = (Songs)int.Parse(header_data[13]);
				Theme = (Themes)int.Parse(header_data[14]);
			}
			all_data_sections[1] = all_data_sections[1].Replace("\r",string.Empty);
			string[] level_data = all_data_sections[1].Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < level_data.Length; i++)
			{
				Tiles.Add(new Tile(level_data[i]));
			}

			SquidProperties = sp;
		}
		
		public Tile GetTileAtPosition(int x, int y, string filter = "")
		{
			for (int i = 0; i < Tiles.Count; i++)
			{
				if (Tiles[i].x == x && Tiles[i].y == y)
				{
					if (filter == "")
					{
						return Tiles[i];
					}
					else
					{
						if (Tiles[i].Object == filter)
						{
							return Tiles[i];
						}
					}
				}
			}
			return null;
		}

		public Tile GetTileAtGridPosition(int x, int y, string filter = "")
		{
			int[] offset = Snailax.GetObjectOffset(filter);
			return GetTileAtPosition((x * 60) + offset[0], (y * 60) + offset[1], filter);
		}

		public Tile PlaceTileAtPosition(int x, int y, string name)
		{
			Tile tile = new Tile(name, x, y);
			Tiles.Add(tile);

			return tile;
		}

		public Tile PlaceTileAtGridPosition(int x, int y, string name)
		{
			int[] offset = Snailax.GetObjectOffset(name);
			return PlaceTileAtPosition((x * 60) + offset[0], (y * 60) + offset[1], name);
		}


		public string Serialize()
		{
			StringBuilder stb = new StringBuilder();
			stb.AppendLine(Snailax.LatestLevelFormat.ToString());
			stb.AppendLine((SquidProperties.GroundSpikeProbability / 100f).ToString());
			stb.AppendLine((SquidProperties.WallSpikeProbablity / 100f).ToString());
			stb.AppendLine((SquidProperties.CeilingSpikeProbability / 100f).ToString());
			stb.AppendLine((SquidProperties.AirSpikeProbability / 100f).ToString());
			stb.AppendLine((SquidProperties.BulletProbablity / 100f).ToString());
			stb.AppendLine(SquidProperties.CompletelyDisabled ? "1" : "0");
			stb.AppendLine((SquidProperties.IceSpikeFallProbability / 100f).ToString());
			stb.AppendLine((SquidProperties.FireworksProbability / 100f).ToString());
			stb.AppendLine((SquidProperties.ConveyerBeltChangeProbability / 100f).ToString());
			stb.AppendLine((SquidProperties.LaserProbability / 100f).ToString());
			stb.AppendLine(RoomMultiplierX.ToString());
			stb.AppendLine(RoomMultiplierY.ToString());
			stb.AppendLine(((int)Song).ToString());
			stb.AppendLine(((int)Theme).ToString());
			stb.AppendLine("-");
			for (int i = 0; i < Tiles.Count; i++)
			{
				stb.AppendLine(Tiles[i].Serialize());
			}

			return stb.ToString();
		}
	}

	public static class Snailax
	{
		public static bool ParseRotation(string obj)
		{
			return (obj == "obj_squasher" || obj == "obj_fish" || obj == "obj_underwater_current");
		}

		public static int[] GetObjectOffset(string objectid)
		{
			if (ObjectOffsets.ContainsKey(objectid))
			{
				return ObjectOffsets[objectid];
			}
			return new int[] { 0, 0 };
		}

		public static Dictionary<string, int[]> ObjectOffsets = new Dictionary<string, int[]> {
			{ "obj_evil_snail", new int[] {30, 58} },
			{ "obj_underwater_current", new int[] {20, 25} },
			{ "obj_spike_permanent", new int[] { 30, 60 } },
			{ "obj_ice_spike", new int[] { 30, 60 } },
			{ "obj_playerspawn", new int[] { 30, 30 } },
			{ "obj_fish", new int[] { 30, 30 } },
			{ "obj_drone_piranha_spawner", new int[] { 30, 30 } },
			{ "obj_jellyfish", new int[] { 30, 30 } },
			{ "obj_squasher", new int[] { 30, 30 } },
			{ "obj_bomb_spawner", new int[] { 30, 30 } },
			{ "obj_gun", new int[] { 30, 30 } },
			{ "obj_enemy", new int[] { 30, 30 } },
			{ "obj_bubble", new int[] { 30, 30 } },
			{ "obj_drone_spawner", new int[] { 30, 30 } },
			{ "obj_sh_enemy_spawnpoint_normy", new int[] { 30, 30 } },
			{ "obj_exploration_point", new int[] { 30, 30 } },
			{ "obj_protector", new int[] { 30, 30 } },
			{ "obj_uplifter", new int[] { 30, 30 } },
			{ "obj_speedbooster", new int[] { 35, 35 } },
			{ "obj_sh_gun", new int[] { 30, 60 } },
			{ "obj_sh_gun2", new int[] { 30, 60 } },
			{ "obj_sh_gun3", new int[] { 30, 60 } },
			{ "obj_sh_gun4", new int[] { 30, 60 } },
		};

		public const byte LatestLevelFormat = 4;
	}


	public class Tile
	{
		public string Object = "obj_wall";
		public int x = 0;
		public int y = 0;
		public float rotation = 0f;

		public Tile(string toparse)
		{
			Console.WriteLine(toparse);
			string[] name_and_data = toparse.Split(':');
			string[] splits = name_and_data[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			Object = name_and_data[0];
			x = int.Parse(splits[0]);
			y = int.Parse(splits[1]);
			if (Snailax.ParseRotation(Object))
			{
				rotation = float.Parse(splits[2]);
			}
		}

		public string Serialize()
		{
			return Object + ":" + x.ToString() + "," + y.ToString() + "," + (Snailax.ParseRotation(Object) ? rotation.ToString() + "," : "");
		}

		public Tile(string objectname, int X, int Y)
		{
			Object = objectname;
			x = X;
			y = Y;
		}
	}
}
