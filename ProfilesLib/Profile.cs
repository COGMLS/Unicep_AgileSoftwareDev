using System;
using System.IO;

using AutoStartLib;

using DirectoryAux;

namespace ProfilesLib
{
	public class Profile
	{
		private string ProfileName;

		readonly private string AutoStartAppsBase = "Auto Start Apps";
		readonly private string ProfilesBase = "Profiles";
		readonly private string ProfileFolderExt = ".profile";
		readonly private string ProfileInternal_Init = "Init";
		readonly private string ProfileInternal_Estatistics = "PerfAnalyze";

		private string LocalAppDataEnv;

		private bool ProfileDirExist;

		Profile(string profileName)
		{
			this.ProfileName = profileName;
			this.LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		}

		//Load profile, selected by the name.
		public int LoadProfile(string ProfileName)
		{
			string ProfileDir = this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase + "\\" + ProfileName + this.ProfileFolderExt;
			
			//Verify if Profile directory exist.
			if(Directory.Exists(ProfileDir))
			{
				//Get the Initialization files.
				string[] ProfileInitFilesList = Directory.GetFiles(ProfileDir);
				//Get the Metrics files.
				string[] ProfileMetricsFilesList = Directory.GetFiles(ProfileDir);

				if(ProfileInitFilesList.Length > 0)
				{
					InitializationList ProfileInitList = new InitializationList(ProfileInitFilesList.Length);

					for(int i = 0; i < ProfileInitFilesList.Length; i++)
                    {
						string[] LoadedProgramConfigs = File.ReadAllLines(ProfileInitFilesList[i]);

						string ProgramName = null;
						string CmdLine = null;
						string Args = null;
						string WorkingDir = null;
						StartWindowStyle windowStyle = StartWindowStyle.NORMAL;
						CommonTypes.StartPriority Priority = CommonTypes.StartPriority.NORMAL;
						int WaitTime = 0;

						for (int j = 0; j < LoadedProgramConfigs.Length; j++)
						{

							if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.PROGRAMNAME, ref ProfileInitFilesList[i]))
							{
								ProgramName = ProfileConfig.LoadConfigString(ProfileConfig.ConfigTags.PROGRAMNAME, ref ProfileInitFilesList[i]);
							}
							else if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.COMMANDLINE, ref ProfileInitFilesList[i]))
							{
								CmdLine = ProfileConfig.LoadConfigString(ProfileConfig.ConfigTags.COMMANDLINE, ref ProfileInitFilesList[i]);
							}
							else if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.ARGS, ref ProfileInitFilesList[i]))
							{
								Args = ProfileConfig.LoadConfigString(ProfileConfig.ConfigTags.ARGS, ref ProfileInitFilesList[i]);
							}
							else if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.WORKINGDIR, ref ProfileInitFilesList[i]))
							{
								WorkingDir = ProfileConfig.LoadConfigString(ProfileConfig.ConfigTags.WORKINGDIR, ref ProfileInitFilesList[i]);
							}
							else if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.WINDOWSTYLE, ref ProfileInitFilesList[i]))
							{
								windowStyle = ProfileConfig.LoadConfigWinStyle(ProfileConfig.ConfigTags.WINDOWSTYLE, ref ProfileInitFilesList[i]);
							}
							else if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.PRIORITY, ref ProfileInitFilesList[i]))
							{
								Priority = ProfileConfig.LoadConfigPriority(ProfileConfig.ConfigTags.PRIORITY, ref ProfileInitFilesList[i]);
							}
							else if (ProfileConfig.IsConfigTag(ProfileConfig.ConfigTags.WAITTIME, ref ProfileInitFilesList[i]))
							{
								WaitTime = ProfileConfig.LoadConfigInt(ProfileConfig.ConfigTags.WAITTIME, ref ProfileInitFilesList[i]);
							}
						}

						if (WorkingDir == null)
						{
							ProfileInitList.Add2Init(ProgramName, CmdLine, Args, WaitTime);
						}
						else
                        {
							ProfileInitList.Add2Init(ProgramName, CmdLine, Args, WaitTime, WorkingDir, Priority, windowStyle);
                        }
					}

					return 0;
				}
				else
				{
					//Profile dosn't have files to be loaded.

					return -1;
				}
			}
			else
			{
				//Profile dosn't exist.

				return -2;
			}
		}

		//Save the profile
		public void SaveProfile(string ProfileName, bool OverWrite)
		{

		}

		//Verify the profiles available
		string[] GetProfiles()
		{
			string ProfileDir = this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase;

			if (AuxiliarProfileManager.ProfileDirExist())
            {
				int NumProfiles = AuxiliarProfileManager.GetProfilesList(ProfileDir).Length;

            }
			else
            {

            }


			string[] ProfilesList = new string[10];



			return ProfilesList;
		}


	}

	public static class AuxiliarProfileManager
    {
		readonly static private string AutoStartAppsBase = "Auto Start Apps";
		readonly static private string ProfilesBase = "Profiles";
		readonly static private string ProfileFolderExt = ".profile";

		//Check the profile directory
		public static bool ProfileDirExist()
        {
			string LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string ProfilesBaseDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase;

			//Verify if Profile directory exist.
			if (Directory.Exists(ProfilesBaseDir))
			{
				return true;
			}
			else
            {
				return false;
            }
		}

		//Get the Enumerated profiles available.
		public static string[] GetProfilesList(string Path)
        {
			//Verify if Profile directory exist.
			if (Directory.Exists(Path))
			{
				//Get the number of profiles.
				string[] ProfilePreLoad = Directory.GetDirectories(Path);

				if (ProfilePreLoad.Length > 0)
				{
					//Get the files list with a full paths.
					string[] ProfilesList = Directory.GetDirectories(Path);
					int PathCut = Path.Length + 1;

					//Verify if the FilesList has some file(s) to treat.
					if (ProfilesList.Length != 0)
					{
						string[] FinalFilesList = new string[ProfilesList.Length];

						for (int i = 0; i < ProfilesList.Length; i++)
						{
							FinalFilesList[i] = ProfilesList[i].Substring(PathCut);
						}

						//Return the names list array.
						return FinalFilesList;
					}
					else
					{
						//Return a NULL value in case a zeroed list.
						return null;
					}
				}
				else
				{
					//Profile dosn't have profiles to be loaded.
					return null;
				}
			}
			else
			{
				//Profile base folder dosn't exist.
				return null;
			}
		}

		//Get the number of profiles available for the user
		public static int NumProfiles(string Path)
        {
			var EnumDirs = Directory.EnumerateDirectories(Path);

			int NumProfiles = 0;

            foreach (var item in EnumDirs)
            {
				if(item.EndsWith(".profile"))
                {
					NumProfiles++;
                }
            }

			return NumProfiles;
        }

		//Create base profile directory with calling the directory class and handles with exceptions. If create with sucess the return will be a NULL value.
		static System.Exception CreateProfileRepos()
        {
			string LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string ProfilesBaseDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase;

			try
			{
				Directory.CreateDirectory(ProfilesBaseDir);
				return null;
			}
			catch (Exception exception)
			{
				return exception;
				throw;
			}
		}
    }

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
			PROGRAMNAME,	//String
			COMMANDLINE,	//String
			ARGS,			//String
			WORKINGDIR,		//String
			WINDOWSTYLE,	//Enum
			PRIORITY,		//Enum
			WAITTIME		//Int
        }

		public static bool IsConfigTag(ConfigTags CfgTag, ref string ConfigString)
        {
			if(ConfigString.StartsWith(ConfigNames[(int)CfgTag]))
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
			if(CfgTag == ConfigTags.PROGRAMNAME || CfgTag == ConfigTags.COMMANDLINE || CfgTag == ConfigTags.ARGS || CfgTag == ConfigTags.WORKINGDIR)
            {
				obj = LoadConfigString(CfgTag, ref ConfigString);

				return 0;
            }
			else if(CfgTag == ConfigTags.WINDOWSTYLE)
            {
				obj = LoadConfigWinStyle(CfgTag, ref ConfigString);

				return 0;
            }
			else if(CfgTag == ConfigTags.PRIORITY)
            {
				obj = LoadConfigPriority(CfgTag, ref ConfigString);

				return 0;
            }
			else if(CfgTag == ConfigTags.WAITTIME)
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

			if(t == "-1")
            {
				return StartWindowStyle.NOWINDOW;
            }
			else if(t == "0")
            {
				return StartWindowStyle.NORMAL;
            }
			else if(t == "1")
            {
				return StartWindowStyle.HIDDEN;
            }
			else if(t == "2")
            {
				return StartWindowStyle.MINIMIZED;
            }
			else if(t == "3")
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

			if(t == "0")
            {
				return CommonTypes.StartPriority.LOW;
            }
			else if(t == "1")
            {
				return CommonTypes.StartPriority.NORMAL;
            }
			else if(t == "2")
            {
				return CommonTypes.StartPriority.HIGH;
            }
			else
            {
				return CommonTypes.StartPriority.NORMAL;
            }
		}
	}
}
