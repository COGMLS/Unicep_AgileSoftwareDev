using System;
using System.IO;

using AutoStartLib;
using MetricsLib;
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
			else
            {
				this.ProfileContainer = this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase + "\\" + ProfileName + this.ProfileFolderExt;

				try
				{
					Directory.CreateDirectory(this.ProfileContainer);
					Directory.CreateDirectory(this.ProfileContainer + "\\" + this.ProfileInternal_Init);
					Directory.CreateDirectory(this.ProfileContainer + "\\" + this.ProfileInternal_Estatistics);

					this.ProfileDirExist = Directory.Exists(this.LocalAppDataEnv + "\\" + this.AutoStartAppsBase + "\\" + this.ProfilesBase + "\\" + ProfileName + this.ProfileFolderExt);
				}
				catch (Exception)
                {
					this.ProfileDirExist = false;
					throw;
                }
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
					//Organizes the initialization list
					this.ProfileInitList.PrepareInitList2Start();
					//Profile loaded with success.
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
		public void SaveProfile()
        {
			if (this.ProfileDirExist)
			{
				//Check if some old file isn't necessary any more
				ChkSavFiles();

				//Loop that start from 0 to the variable HStartIndex to avoid get null values, in cases with a empty objects.
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

					LoadedProgramConfigs = this.ProfileInitList.ExpObjInfo(i);

					//Saves the object configuration's to a file with the program's name will be init.
					File.WriteAllLines(this.ProfileContainer + "\\" + this.ProfileInternal_Init + "\\" + LoadedProgramConfigs[0], LoadedProgramConfigs);

					//Save the metrics file:
					float[] MetricsList = this.ProfileInitList.ExpObjMetrics(i);
					Metrics.SaveMetrics(ref this.ProfileContainer, ref LoadedProgramConfigs[0], ref MetricsList);
				}
			}
        }

		//Check saved files
		private void ChkSavFiles()
        {
			//Get the paths arrays to the saved init. files.
			string[] OldInitFilesPaths = Directory.GetFiles(this.ProfileContainer + "\\" + this.ProfileInternal_Init);
			//Get only the names from saved init. files.
			string[] OldInitFiles = DirectoryTreatment.GetFilesName(this.ProfileContainer + "\\" + this.ProfileInternal_Init);
			//Get the names for new files.
			string[] NewInitFiles = this.ProfileInitList.ExpObjList();

			//In case the list isn't empty.
			if (OldInitFiles != null)
			{
				for (int i = 0; i < OldInitFiles.Length; i++)
				{
					//The old file will be deleted?
					bool WillDelete = true;

					//Check if the new files list has the same name, if don't the file will be deleted to avoid initialize unwanted applications.
					for (int j = 0; j < NewInitFiles.Length; j++)
					{
						//In case to preserve the file, the old one will be overwrited.
						if (OldInitFiles[i] == NewInitFiles[j])
						{
							WillDelete = false;
							break;
						}
					}

					//If the old file will be deleted
					if (WillDelete == true)
					{
						try
						{
							//Delete the init. file
							File.Delete(OldInitFilesPaths[i]);
							//Delete the metrics file.
							File.Delete(this.ProfileContainer + "\\" + this.ProfileInternal_Estatistics + "\\" + OldInitFiles[i]);
						}
						catch (Exception)
						{
							throw;
						}
					}
				}
			}
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
