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
			REMAPP,
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
			"--RemApp",				//[5] Removes a application registered in a profile (needs --AdminPass)
			"--AddCsv2Profile",		//[6] Adds a CSV file with each line as a app entry to add in init. list. The sequence of values needs be ProgramName, CmdLine, Args, WaitTime, WorkingDir*, Priority2Start, WindowStyle
			"--AdminConfig",		//[7] Changes the AdminPass, if AdminPass isn't setted, the AdminPass will be avoided (needs --AdminPass)
			"--AdminPass",			//[8] Administrator Password
		};

		/** Sum of return value available:
		 * ----------------------------------------------------
		 * LoadProfile - 1
		 * NewProfile - 2 (Needs AdminPass) -> 258
		 * RemoveProfile - 4 (Needs AdminPass) -> 260
		 * ListProfile - 8
		 * Add2Profile - 16 (Needs AdminPass) -> 272
		 * RemApp - 32 (Needs AdminPass) -> 288
		 * AddCsv2Profile - 64 (Needs AdminPass) -> 320
		 * AdminConfig - 128 (Needs AdminPass) -> 384
		 * AdminPass - 256 (Needs be used with another parameter)
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
				case CommandLineAvailable.REMAPP:
					return 5;
                case CommandLineAvailable.ADDCSV2PROFILE:
					return 6;
                case CommandLineAvailable.ADMINCONFIG:
					return 7;
                case CommandLineAvailable.ADMINPASS:
					return 8;
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
					return CommandLineAvailable.REMAPP;
				case 6:
					return CommandLineAvailable.ADDCSV2PROFILE;
                case 7:
					return CommandLineAvailable.ADMINCONFIG;
                case 8:
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

		//Verify if the command line has the --help -h or -? argument
		public static bool IsCommandHelp(ref string[] Args)
        {
			for (int i = 0; i < Args.Length; i++)
			{
				if (Args[i].ToLower() == "--help")
				{
					return true;
				}
				else if (Args[i].ToLower() == "-help")
				{
					return true;
				}
				else if (Args[i].ToLower() == "-?")
                {
					return true;
                }
				else if(Args[i].ToLower() == "-h")
                {
					return true;
                }
            }
			return false;
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
				case 32:        //--RemApp (Ok if dosn't have a password setted)
					if (IsAdminPassSetted)
					{
						return true;
					}
					else
					{
						return false;
					}
				case 64:		//--AddCsv2Profile (Ok if dosn't have a password setted)
					if (IsAdminPassSetted)
					{
						return true;
					}
					else
					{
						return false;
					}
				case 128:		//--AdminConfig
					return false;
				case 256:		//--AdminPass
					return false;
				case 258:		//--NewProfile --AdminPass
					return true;
				case 260:		//--RemoveProfile --AdminPass
					return true;
				case 272:		//--Add2Profile --AdminPass
					return true;
				case 288:		//--RemApp --AdminPass
					return true;
				case 320:		//--AddCsv2Profile --AdminPass
					return true;
				case 384:		//--AdminConfig --AdminPass
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
					if ((ClWeightSum >= 258 || (ClWeightSum >= 1 && ClWeightSum <= 64)) && IsCmdLineOk == true)
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
