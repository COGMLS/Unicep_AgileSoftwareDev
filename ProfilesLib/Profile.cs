using System;
using System.IO;

using AutoStartLib;

using DirectoryAux;

namespace ProfilesLib
{
	public class Profile
	{
		//Profile name
		private string ProfileName;

		//Constants to control the profile directory
		readonly private string AutoStartAppsBase = "Auto Start Apps";
		readonly private string ProfilesBase = "Profiles";
		readonly private string ProfileFolderExt = ".profile";
		readonly private string ProfileInternal_Init = "Init";
		readonly private string ProfileInternal_Estatistics = "PerfAnalyze";

		//Directory to AppData and Profile folder:
		private string LocalAppDataEnv;
		private string ProfileContainer;

		//Variable control.
		private bool ProfileDirExist;

		//Initialization List:
		InitializationList ProfileInitList = null;

		Profile(string profileName)
		{
			this.ProfileName = profileName;
			this.LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			this.ProfileDirExist = Directory.Exists(this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase + "\\" + ProfileName + this.ProfileFolderExt);
		}

		//Load profile, selected by the name.
		public int LoadProfile(string ProfileName)
		{
			//Verify if Profile directory exist.
			if(this.ProfileDirExist)
			{
				//Get the Initialization files.
				string[] ProfileInitFilesList = Directory.GetFiles(this.ProfileContainer + "\\" + this.ProfileInternal_Init);
				//Get the Metrics files.
				string[] ProfileMetricsFilesList = Directory.GetFiles(this.ProfileContainer + "\\" + this.ProfileInternal_Estatistics);

				if(ProfileInitFilesList.Length > 0)
				{
					//Initilize the ProfileInitList
					ProfileInitList = new InitializationList(ProfileInitFilesList.Length);

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
							//Gets the configurations saved, searching for the right tags.
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

						//If the working directory is null
						if (WorkingDir == null)
						{
							_ = ProfileInitList.Add2Init(ProgramName, CmdLine, Args, WaitTime, Priority, windowStyle);
						}
						else
						{
							_ = ProfileInitList.Add2Init(ProgramName, CmdLine, Args, WaitTime, WorkingDir, Priority, windowStyle);
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
		public void SaveProfile(bool OverWrite)
        {
			string ProfileInitConfig = this.ProfileContainer + "\\" + this.ProfileInternal_Init;

			for (int i = 0; i < this.ProfileInitList.GetInitSize(); i++)
            {
				string[] LoadedProgramConfigs;

				//string ProgramName = null;
				//string CmdLine = null;
				//string Args = null;
				//string WorkingDir = null;
				//StartWindowStyle windowStyle = StartWindowStyle.NORMAL;
				//CommonTypes.StartPriority Priority = CommonTypes.StartPriority.NORMAL;
				//int WaitTime = 0;

				
				
			}
        }
		public void SaveProfile(string ProfileName, bool OverWrite)
		{

		}

		//Verify the profiles available
		string[] GetProfiles()
		{
			string ProfilesDir = this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase;

			if (AuxiliarProfileManager.ProfileDirExist())
			{
				int NumProfiles = AuxiliarProfileManager.GetProfilesList(ProfilesDir).Length;

			}
			else
			{

			}


			string[] ProfilesList = new string[10];



			return ProfilesList;
		}
	}
}
