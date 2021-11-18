using System;

namespace AutoStartupConsole
{
	public static class CliClass
	{
        public enum CommandLineAvailable : int
        {
			LOADPROFILE,
			NEWPROFILE,
			REMOVEPROFILE,
			LISTPROFILE,
			ADD2PROFILE,
			ADDCSV2PROFILE,
			ADMINCONFIG,
			ADMINPASS
        }

		public static readonly int[] CL_WEIGHT_VALUES = {1, 2, 4, 8, 16, 32, 64, 128};

		private static readonly string[] CL_ARRAY = {
			"--LoadProfile",		//[0] Load profile (for all users)
			"--NewProfile",			//[1] Create a new profile (needs --AdminPass)
			"--RemoveProfile",		//[2] Remove and delete a profile (needs --AdminPass)
			"--ListProfiles",		//[3] List the profiles available for the user account (for all users)
			"--Add2Profile",		//[4] Adds a new application to start initialization list (needs --AdminPass)
			"--AddCsv2Profile",		//[5] Adds a CSV file with each line as a app entry to add in init. list. The sequence of values needs be ProgramName, CmdLine, Args, WaitTime, WorkingDir*, Priority2Start, WindowStyle
			"--AdminConfig",		//[6] Changes the AdminPass, if AdminPass isn't setted, the AdminPass will be avoided (needs --AdminPass)
			"--AdminPass",			//[7] Administrator Password
		};

		/** Sum of return value available:
		 * ----------------------------------------------------
		 * LoadProfile - 1
		 * NewProfile - 2 (Needs AdminPass) -> 130
		 * RemoveProfile - 4 (Needs AdminPass) -> 132
		 * ListProfile - 8
		 * Add2Profile - 16 (Needs AdminPass) -> 144
		 * AddCsv2Profile - 32 (Needs AdminPass) -> 160
		 * AdminConfig - 64 (Needs AdminPass) -> 192
		 * AdminPass - 128 (Needs be used with another parameter)
		 */

		//Converts the enumerator to a integer value
		private static int Enum2Int(CommandLineAvailable IndexE)
        {
            switch (IndexE)
            {
                case CommandLineAvailable.LOADPROFILE:
					return 0;
                case CommandLineAvailable.NEWPROFILE:
					return 1;
                case CommandLineAvailable.REMOVEPROFILE:
					return 2;
                case CommandLineAvailable.LISTPROFILE:
					return 3;
                case CommandLineAvailable.ADD2PROFILE:
					return 4;
                case CommandLineAvailable.ADDCSV2PROFILE:
					return 5;
                case CommandLineAvailable.ADMINCONFIG:
					return 6;
                case CommandLineAvailable.ADMINPASS:
					return 7;
				default:
					return 0;
            }
        }

		//Converts the integer to a Enum value
		private static CommandLineAvailable Int2Enum(int IndexI)
        {
			switch (IndexI)
            {
                case 0:
					return CommandLineAvailable.LOADPROFILE;
                case 1:
					return CommandLineAvailable.NEWPROFILE;
                case 2:
					return CommandLineAvailable.REMOVEPROFILE;
                case 3:
					return CommandLineAvailable.LISTPROFILE;
                case 4:
					return CommandLineAvailable.ADD2PROFILE;
                case 5:
					return CommandLineAvailable.ADDCSV2PROFILE;
                case 6:
					return CommandLineAvailable.ADMINCONFIG;
                case 7:
					return CommandLineAvailable.ADMINPASS;
				default:
					return CommandLineAvailable.LOADPROFILE;
            }
        }

		//Get the Command Line Weight for the respectly command
		private static int GetClWeight(CommandLineAvailable IndexE)
        {
			return 2 ^ Enum2Int(IndexE);
        }

		//Verify the arguments is using some of the commands available
		private static int CheckClWeights(ref string[] Args, CommandLineAvailable IndexE)
		{
			//Search the command argument array for the correct command line
			for (int i = 0; i < Args.Length; i++)
			{
				//Check if is the correct command.
				if (Args[i] == CL_ARRAY[Enum2Int(IndexE)])
				{
					return GetClWeight(IndexE);
				}
			}
			return 0;
		}

		//Verify the command line and attributes the weights for each parameter that are present
		public static int VerifyCmdLine(ref string[] Args)
        {
			int ClWeightSum = 0;
			//Search the command argument array for the correct command line
			for (int i = 0; i < Args.Length; i++)
			{
				ClWeightSum += CheckClWeights(ref Args, Int2Enum(i));
			}
			return ClWeightSum;
		}

		//Verify if the command line has some problem with the parameters
		public static bool IsCmdLineOk(int ClWeightSum, bool IsAdminPassSetted)
        {
            switch (ClWeightSum)
            {
				case 0:			//No parameters used
					return false;	
				case 1:			//--LoadProfile
					return true;	
				case 2:			//--NewProfile (Ok if dosn't have a password setted)
					if(IsAdminPassSetted)
                    {
						return true;
                    }
					else
                    {
                        return false;
                    }
				case 4:			//--RemoveProfile (Ok if dosn't have a password setted)
					if (IsAdminPassSetted)
                    {
						return true;
					}
					else
					{
						return false;
					}
				case 8:			//--ListProfile
					return true;
				case 16:		//--Add2Profile (Ok if dosn't have a password setted)
					if (IsAdminPassSetted)
					{
						return true;
					}
					else
					{
						return false;
					}
				case 32:		//--AddCsv2Profile (Ok if dosn't have a password setted)
					if (IsAdminPassSetted)
					{
						return true;
					}
					else
					{
						return false;
					}
				case 64:		//--AdminConfig
					return false;
				case 128:		//--AdminPass
					return false;
				case 130:		//--NewProfile --AdminPass
					return true;
				case 132:		//--RemoveProfile --AdminPass
					return true;
				case 144:		//--Add2Profile --AdminPass
					return true;
				case 160:		//--AddCsv2Profile --AdminPass
					return true;
				case 192:		//--AdminConfig --AdminPass
					return true;
				default:		//Any other combination will be considered not available to work
					return false;
            }
        }

		//Function destinated to extract the value from the arguments
		public static string GetClValue(ref string[] Args, CommandLineAvailable IndexE, ref bool IsCmdLineOk, ref int ClWeightSum)
        {
			string ArgumentValue = null;

			//Search the command argument array for the correct command line
			for (int i = 0; i < Args.Length; i++)
			{
				//Check if is the correct command.
				if (Args[i] == CL_ARRAY[Enum2Int(IndexE)])
                {
					//Check if the command will receave a value or not, using the ClWeightSum and IsCmdLineOk control variables
					if ((ClWeightSum >= 130 || (ClWeightSum >= 1 && ClWeightSum <= 32)) && IsCmdLineOk == true)
					{
						//Verify if has another element to be readed and loaded to the return variable
						if (i + 1 < Args.Length)
						{
							//Verify if dosn't start with '-' a argument the next position
							if (!Args[i + 1].StartsWith('-'))
							{
								return Args[i + 1];
							}
						}
					}
				}
			}
			//In case wasn't possible to finded and/or loaded the argument
			return ArgumentValue;
		}
	}
}
