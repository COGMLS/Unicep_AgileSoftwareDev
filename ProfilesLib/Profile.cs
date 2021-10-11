using System;
using System.IO;

using AutoStartLib;

namespace ProfilesLib
{
	public class Profile
	{
		private string ProfileName;

		protected string AutoStartAppsBase = "Auto Start Apps";
		protected string ProfilesBase = "Profiles";
		protected string ProfileFolderID = ".profile";
		protected string ProfileInternal_Init = "Init";
		protected string ProfileInternal_Estatistics = "PerfAnalyze";

		Profile(string profileName)
		{
			this.ProfileName = profileName;
		}

		public void LoadProfile(string ProfileName)
		{
			string LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string ProfileDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase + "\\" + ProfileName + ProfileFolderID;

			if(Directory.Exists(ProfileDir))
			{
				string[] ProfilePreLoad = Directory.GetFiles(ProfileDir);

				if(ProfilePreLoad.Length > 0)
				{
					int InitSize = 0;
					for(int i = 0; i < ProfilePreLoad.Length; i++)
					{
						if(ProfilePreLoad[i].EndsWith(ProfileFolderID))
						{
							InitSize++;
						}
					}

					InitializationList ProfileInitList = new InitializationList(InitSize);

					for(int i = 0; i < InitSize; i++)
					{
						File.OpenRead(ProfileDir + "\\" + ProfilePreLoad[i]);
						

					}
				}
				else
				{
					//Profile dosn't have files to be loaded.
				}
			}
			else
			{
				//Profile dosn't exist.
			}
		}
		public void SaveProfile(string ProfileName)
		{

		}


	}
}
