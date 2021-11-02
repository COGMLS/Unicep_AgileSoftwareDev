//App libs:
using AutoStartLib;

namespace ProfilesLib
{
	public static class ProfileConfig
	{
		readonly private static string[] ConfigNames =
		{
			"NAME=",
			"CMD=",
			"ARGS=",
			"WORKDIR=",
			"WINSTYLE=",
			"PRIORITY=",
			"WAIT="
		};

		public enum ConfigTags : int
		{
			PROGRAMNAME,    //String
			COMMANDLINE,    //String
			ARGS,           //String
			WORKINGDIR,     //String
			WINDOWSTYLE,    //Enum
			PRIORITY,       //Enum
			WAITTIME        //Int
		}

		//Check if the line readed has a valid configuration tag.
		public static bool IsConfigTag(ConfigTags CfgTag, ref string ConfigString)
		{
			if (ConfigString.StartsWith(ConfigNames[(int)CfgTag]))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//Load the respectly config
		public static int LoadConfig(ConfigTags CfgTag, ref string ConfigString, ref object obj)
		{
			if (CfgTag == ConfigTags.PROGRAMNAME || CfgTag == ConfigTags.COMMANDLINE || CfgTag == ConfigTags.ARGS || CfgTag == ConfigTags.WORKINGDIR)
			{
				obj = LoadConfigString(CfgTag, ref ConfigString);

				return 0;
			}
			else if (CfgTag == ConfigTags.WINDOWSTYLE)
			{
				obj = LoadConfigWinStyle(CfgTag, ref ConfigString);

				return 0;
			}
			else if (CfgTag == ConfigTags.PRIORITY)
			{
				obj = LoadConfigPriority(CfgTag, ref ConfigString);

				return 0;
			}
			else if (CfgTag == ConfigTags.WAITTIME)
			{
				obj = LoadConfigInt(CfgTag, ref ConfigString);

				return 0;
			}
			else
			{
				obj = null;

				return -1;
			}
		}

		//Load and treat correctly the string value
		public static string LoadConfigString(ConfigTags CfgTag, ref string ConfigString)
		{
			return ConfigString.Substring(ConfigNames[(int)CfgTag].Length);
		}

		//Load and treat correctly the StartWindowStyle Enum value
		public static StartWindowStyle LoadConfigWinStyle(ConfigTags CfgTag, ref string ConfigString)
		{
			string t = ConfigString.Substring(ConfigNames[(int)CfgTag].Length);

			if (t == "-1")
			{
				return StartWindowStyle.NOWINDOW;
			}
			else if (t == "0")
			{
				return StartWindowStyle.NORMAL;
			}
			else if (t == "1")
			{
				return StartWindowStyle.HIDDEN;
			}
			else if (t == "2")
			{
				return StartWindowStyle.MINIMIZED;
			}
			else if (t == "3")
			{
				return StartWindowStyle.MAXIMIZED;
			}
			else
			{
				return StartWindowStyle.NORMAL;
			}
		}

		//Load and treat correctly the integer value
		public static int LoadConfigInt(ConfigTags CfgTag, ref string ConfigString)
		{
			return int.Parse(ConfigString.Substring(ConfigNames[(int)CfgTag].Length));
		}

		//Load and treat correctly the StartPriority Enum value
		public static CommonTypes.StartPriority LoadConfigPriority(ConfigTags CfgTag, ref string ConfigString)
		{
			string t = ConfigString.Substring(ConfigNames[(int)CfgTag].Length);

			if (t == "0")
			{
				return CommonTypes.StartPriority.LOW;
			}
			else if (t == "1")
			{
				return CommonTypes.StartPriority.NORMAL;
			}
			else if (t == "2")
			{
				return CommonTypes.StartPriority.HIGH;
			}
			else
			{
				return CommonTypes.StartPriority.NORMAL;
			}
		}

		//Converts the StartWindowStyle Enum to string
		public static string ConvertStartWindowStyle(StartWindowStyle WindowStyleE)
        {
			if (WindowStyleE == StartWindowStyle.NOWINDOW)
			{
				return "-1";
			}
			else if (WindowStyleE == StartWindowStyle.NORMAL)
			{
				return "0";
			}
			else if (WindowStyleE == StartWindowStyle.HIDDEN)
			{
				return "1";
			}
			else if (WindowStyleE == StartWindowStyle.MINIMIZED)
			{
				return "2";
			}
			else if (WindowStyleE == StartWindowStyle.MAXIMIZED)
			{
				return "3";
			}
			else
			{
				return null;
			}
		}

		//Converts the Priority Enum to string.
		public static string ConvertPriority(CommonTypes.StartPriority PriorityE)
        {
			if (PriorityE == CommonTypes.StartPriority.LOW)
			{
				return "0";
			}
			else if (PriorityE == CommonTypes.StartPriority.NORMAL)
			{
				return "1";
			}
			else if (PriorityE == CommonTypes.StartPriority.HIGH)
			{
				return "2";
			}
			else
			{
				return null;
			}
        }
	}
}
