using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private static readonly string[] CL_ARRAY = {
			"--LoadProfile",		//Load profile (for all users)
			"--NewProfile",			//Create a new profile (needs --AdminPass)
			"--RemoveProfile",		//Remove and delete a profile (needs --AdminPass)
			"--ListProfiles",		//List the profiles available for the user account (for all users)
			"--Add2Profile",		//Adds a new application to start initialization list (needs --AdminPass)
			"--AddCsv2Profile",		//Adds a CSV file with each line as a app entry to add in init. list. The sequence of values needs be ProgramName, CmdLine, Args, WaitTime, WorkingDir*, Priority2Start, WindowStyle
			"--AdminConfig",		//Changes the AdminPass, if AdminPass isn't setted, the AdminPass will be avoided (needs --AdminPass)
			"--AdminPass",			//Administrator Password
		};

		public static void VerifyCmdLine(ref string[] Args)
		{

		}
	}
}
