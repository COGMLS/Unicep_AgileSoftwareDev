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

		//Constructor for Profile class, to determinate the basics for the profile.
		Profile(string profileName)
		{
			this.ProfileName = profileName;
			this.LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			this.ProfileDirExist = Directory.Exists(this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase + "\\" + ProfileName + this.ProfileFolderExt);

			if(this.ProfileDirExist)
            {
				this.ProfileContainer = this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase + "\\" + ProfileName + this.ProfileFolderExt;
			}
		}

		//Load profile, selected by the name.
		public int LoadProfile()
		{
			//Verify if Profile directory exist.
			if(this.ProfileDirExist)
			{
				//Get the Initialization files.
				string[] ProfileInitFilesList = Directory.GetFiles(this.ProfileContainer + "\\" + this.ProfileInternal_Init);

				if(ProfileInitFilesList.Length > 0)
				{
					//Initilize the ProfileInitList
					this.ProfileInitList = new InitializationList(ProfileInitFilesList.Length);

					//Loop to read all initialization files.
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

						//Loop to read all lines and treat correctly.
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

						//Add to the Initialization List:
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

					this.ProfileInitList.PrepareInitList2Start();

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

		//Start applications:
		public void StartAppList()
        {
			//Loop to start each object.
			for(int i = 0; i < this.ProfileInitList.GetInitSize(); i++)
            {
				this.ProfileInitList.StartObj(i);
            }
        }
	}
}
