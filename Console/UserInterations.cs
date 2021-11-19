//System Libs:
using System;
using System.IO;

//App Libs:
using AutoStartLib;
using ProfilesLib;

namespace AutoStartupConsole
{
	public static class UserInterations
	{
		public static void GetAppEntriesTUI(ref Profile profile, ref int Entries, bool IsFromConsoleInterration = false)
		{
			_ = profile.InitializeInitList(Entries);
			
			if(IsFromConsoleInterration)
            {
				int Initreturn = profile.SetInitSize(Entries);

				if(Initreturn == 0)
                {
					Console.WriteLine("Initialization size changed with succefull!");
                }
				else if(Initreturn == 1)
                {
					Console.WriteLine("Keep size unchanged. Warning: This could overwrite some configurations!");
                }
				else if(Initreturn == -1)
                {
					Console.WriteLine("An invalid number was sended.");
					return;		//Cancell the operation.
                }
            }

			for (int i = 0; i < Entries && i < profile.GetInitSize();)
			{
				string ProgramName = null;
				string CmdLine = null;
				string Args = null;
				string WorkingDir = null;
				StartWindowStyle windowStyle = StartWindowStyle.NORMAL;
				CommonTypes.StartPriority Priority = CommonTypes.StartPriority.NORMAL;
				int WaitTime = 0;

				Console.WriteLine("Add the following informations:\nNOTE:To cancel this operation set Program Name as -1\n======================================");
				Console.Write("Program Name: ");
				ProgramName = Console.ReadLine();

				if (ProgramName == "-1")
				{
					break;
				}

				Console.Write("Command Line: ");
				CmdLine = Console.ReadLine();
				Console.Write("Arguments: ");
				Args = Console.ReadLine();
				Console.Write("Working Directory (leave it null to not use it): ");
				WorkingDir = Console.ReadLine();

				if (WorkingDir == "")
				{
					WorkingDir = null;
				}

				Console.Write("Window Style (-1 - No Window, 0 - Hide, 1 - Normal [Default], 2 - Minimized, 3 - Maximazed): ");
				string winStyle = Console.ReadLine();
				windowStyle = ProfileConfig.LoadConfigWinStyle(ProfileConfig.ConfigTags.WINDOWSTYLE, ref winStyle);
				Console.Write("Priority to start (0 - Low, 1 - Normal, 2 - High): ");
				string priority = Console.ReadLine();
				Priority = ProfileConfig.LoadConfigPriority(ProfileConfig.ConfigTags.PRIORITY, ref priority);
				Console.Write("Define the time to wait the initilization (seconds): ");
				string time = Console.ReadLine();
				_ = int.TryParse(time, out WaitTime);

				Console.Write("Confirm add to initilization list (y|n): ");
				string Confirm = Console.ReadLine();

				if (Confirm.ToLower() == "y")
				{
					profile.Add2StartList(ProgramName, CmdLine, Args, WorkingDir, windowStyle, Priority, WaitTime);
					i++;
				}
			}
		}

		public static void RemoveProfileInteration(ref string ProfileName)
        {
			Console.Write("Do you want remove the profile? (y|n): ");
			string UserEntry = Console.ReadLine();

			if(UserEntry.ToLower() == "y")
            {
				int result = AuxiliarProfileManager.RemoveProfile(ProfileName);
				if(result == 0)
                {
					Console.WriteLine("Profile deleted!");
                }
				else if(result == -1)
                {
					Console.WriteLine("Fail to remove the profile container.");
                }
				else if (result == -2)
				{
					Console.WriteLine("The path to the profile dosn't exist.");
				}
				else
				{
					Console.WriteLine("Profile Repository dosn't exist.");
				}
			}
			else
            {
				Console.WriteLine("Operation canceled.");
            }
        }

		//Add2Profile command
		public static CommonTypes.ImportList Add2ProfileCmd(ref string ProfileName, ref string[] Args, ref bool IsCmdLineOk, ref int ClWeightSum)
		{
			int Index = 0;
			string AddArg = CliClass.GetClValue(ref Args, CliClass.CommandLineAvailable.ADD2PROFILE, ref IsCmdLineOk, ref ClWeightSum);

			ProfileName = ExtractNextString(ref AddArg, ref Index);

			CommonTypes.ImportList ListStruct;

			ListStruct.ProgramName = ExtractNextString(ref AddArg, ref Index);
			ListStruct.CmdLine = ExtractNextString(ref AddArg, ref Index);
			ListStruct.Args = ExtractNextString(ref AddArg, ref Index);
			ListStruct.WaitTime = ExtractWaitTime(ref AddArg, ref Index);
			ListStruct.WorkingDir = ExtractNextString(ref AddArg, ref Index);
			ListStruct.Priority = ExtractPriority(ref AddArg, ref Index);
			ListStruct.windowStyle = ExtractWindowStyle(ref AddArg, ref Index);

			return ListStruct;
		}

		/**
		 *	string ProgramName = null;
			string CmdLine = null;
			string Args = null;
			string WorkingDir = null;
			StartWindowStyle windowStyle = StartWindowStyle.NORMAL;
			CommonTypes.StartPriority Priority = CommonTypes.StartPriority.NORMAL;
			int WaitTime = 0;
		 */

		private static string ExtractNextString(ref string StrArr, ref int Index)
        {
			string str = null;
			for (int i = 0; i < StrArr.Length; i++)
			{
				Index++;
				if (StrArr[i] == ',' || StrArr[i] == '\n')
				{
					break;
				}
				else
				{
					str += StrArr[i];
				}
			}

			if(str.ToLower() == "null")
            {
				return null;
            }

			return str;
		}

		public static void RemAppInteraction(ref Profile profileLoaded, ref string ProfileName, ref string ProgramName)
		{
			Console.Write("Do you want remove {0}, from profile {1}? (y|n): ", ProgramName, ProfileName);
			string UserEntry = Console.ReadLine();

			if (UserEntry.ToLower() == "y")
			{
				int result = profileLoaded.RemAppStartList(ProgramName);
				if (result == 0)
				{
					Console.WriteLine("App removed from {0}.", ProfileName);
				}
				else
				{
					Console.WriteLine("Fail to remove the app.");
				}
			}
			else
			{
				Console.WriteLine("Operation canceled.");
			}
		}

		public static void RemAppInteraction(ref string ProfileName, ref string ProgramName)
		{
			Console.Write("Do you want remove {0}, from profile {1}? (y|n): ", ProgramName, ProfileName);
			string UserEntry = Console.ReadLine();

			if (UserEntry.ToLower() == "y")
			{
				int result = AuxiliarProfileManager.RemoveAppFromProfile(ProfileName, ProgramName);
				if (result == 0)
				{
					Console.WriteLine("App removed from {0}.", ProfileName);
				}
				else if(result == -1)
				{
					Console.WriteLine("Fail to remove the app.");
				}
				else if (result == -2)
				{
					Console.WriteLine("The path to the profile dosn't exist.");
				}
				else
				{
					Console.WriteLine("Profile Repository dosn't exist.");
				}
			}
			else
			{
				Console.WriteLine("Operation canceled.");
			}
		}

		private static StartWindowStyle ExtractWindowStyle(ref string StrArr, ref int Index)
		{
			string str = null;
			for (int i = 0; i < StrArr.Length; i++)
			{
				Index++;
				if (StrArr[i] == ',' || StrArr[i] == '\n')
				{
					break;
				}
				else
				{
					str += StrArr[i];
				}
			}

			if (str == "-1")
			{
				return StartWindowStyle.NOWINDOW;
			}
			else if (str == "0")
			{
				return StartWindowStyle.NORMAL;
			}
			else if (str == "1")
			{
				return StartWindowStyle.HIDDEN;
			}
			else if (str == "2")
			{
				return StartWindowStyle.MINIMIZED;
			}
			else if (str == "3")
			{
				return StartWindowStyle.MAXIMIZED;
			}
			else
			{
				return StartWindowStyle.NORMAL;
			}
		}

		private static CommonTypes.StartPriority ExtractPriority(ref string StrArr, ref int Index)
		{
			string str = null;
			for (int i = 0; i < StrArr.Length; i++)
			{
				Index++;
				if (StrArr[i] == ',' || StrArr[i] == '\n')
				{
					break;
				}
				else
				{
					str += StrArr[i];
				}
			}

			if (str == "0")
			{
				return CommonTypes.StartPriority.LOW;
			}
			else if (str == "1")
			{
				return CommonTypes.StartPriority.NORMAL;
			}
			else if (str == "2")
			{
				return CommonTypes.StartPriority.HIGH;
			}
			else
			{
				return CommonTypes.StartPriority.NORMAL;
			}
		}

		private static int ExtractWaitTime(ref string StrArr, ref int Index)
		{
			string str = null;
			for (int i = 0; i < StrArr.Length; i++)
			{
				Index++;
				if (StrArr[i] == ',' || StrArr[i] == '\n')
				{
					break;
				}
				else
				{
					str += StrArr[i];
				}
			}

			int WaitTime = 0;

			if(int.TryParse(str, out WaitTime))
            {
				return WaitTime;
            }

			return WaitTime;
		}
	}
}
