//System Libs
using System;
using System.IO;

namespace ProfilesLib
{
	public static class AuxiliarProfileManager
	{
		//Constants to control the profile directory
		readonly static private string LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		readonly static private string AutoStartAppsBase = "Auto Start Apps";
		readonly static private string ProfilesBase = "Profiles";
		readonly static private string ProfileFolderExt = ".profile";
		readonly static private string ProfileInternal_Init = "Init";
		readonly static private string ProfileInternal_Estatistics = "PerfAnalyze";

		//Check the profile base directory
		public static bool ProfileBaseDirExist()
		{
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
		public static string[] GetProfiles()
		{
			string ProfilesDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase;

			//Will simulate a safe pointer
			string[] ProfilesList = null;

			if (ProfileBaseDirExist())
			{
				ProfilesList = GetProfilesList(ProfilesDir);

				if (ProfilesList != null)
				{
					int NumProfiles = ProfilesList.Length;

					string[] FinalProfilesList = new string[NumProfiles];

					for (int i = 0; i < NumProfiles; i++)
					{
						FinalProfilesList[i] = (NumProfiles + 1).ToString() + " - " + ProfilesList[i];
					}

					ProfilesList = FinalProfilesList;
				}
			}

			return ProfilesList;
		}

		//Verify the profiles available
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
					//Get the number of characters to remove from the full path + '\' to return only the names.
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

		//Create base profile directory with calling the directory class and handles with exceptions.
		public static int CreateProfileRepos()
		{
			string ProfilesBaseDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase;

			try
			{
				Directory.CreateDirectory(ProfilesBaseDir);
				return 0;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return -1;
				throw;
			}
		}

		//Create a new profile
		public static int CreateNewProfile(string ProfileName)
        {
			int ProfileReposStatus = 0;
			if(!ProfileBaseDirExist())
            {
				ProfileReposStatus = CreateProfileRepos();
            }

			if(ProfileReposStatus == 0)
            {
				string ProfileFinalPath = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase + "\\" + ProfileName + ProfileFolderExt;
				if(!Directory.Exists(ProfileFinalPath))
                {
                    try
                    {
						Directory.CreateDirectory(ProfileFinalPath);
						return 0;		//Profile created with success.
                    }
                    catch (Exception e)
                    {
						Console.WriteLine(e.Message);
						return -1;		//Fail to create the profile container.
                        throw;
                    }
					
                }
				return -2;	//The path to the new profile already exist.
            }
			return -3;		//Wasn't possible create the Profile Repository.
        }

		//Remove a existant profile
		public static int RemoveProfile(string ProfileName)
        {
			if (!ProfileBaseDirExist())
			{
				return -3;      //Profile Repository dosn't exist.
			}
			else
			{
				string ProfileFinalPath = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase + "\\" + ProfileName + ProfileFolderExt;
				if (Directory.Exists(ProfileFinalPath))
				{
					try
					{
						Directory.Delete(ProfileFinalPath, true);
						return 0;       //Profile removed with success.
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						return -1;      //Fail to remove the profile container.
						throw;
					}

				}
				return -2;  //The path to the profile dosn't exist.
			}	
		}
	}
}
