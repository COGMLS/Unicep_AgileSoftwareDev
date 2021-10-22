//System Libs
using System;
using System.IO;

namespace ProfilesLib
{
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
				if (item.EndsWith(".profile"))
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
}
