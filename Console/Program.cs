/* Project: Auto Startup Apps
 * ***********************************************
 * Authors:
 *          Matheus Lopes Silvati
 *          Caroline Paganelli Corrêa dos Santos
 * ***********************************************
 * Date: 2021/09/23
 * ***********************************************
 * 
 * **/

//System libs:
using System;
using System.IO;

//App libs:
using ConfigLib;
using ProfilesLib;
using AutoStartLib;

namespace AutoStartupConsole
{
	class ConsoleProgram
	{
		static void Main(string[] args)
		{
			/**Configuration sequence:
			 * ----------------------------
			 * 1. Create the config obj.
			 * 2. Check the cmd line weight sum.
			 * 3. Verify if the cmd line is ok.
			 * 4. Get the values of each variable.
			 * 5. Create the config obj, sending all args info.
			 * 6. 
			 */

			 Config AppCfg = new Config();

			if(args.Length > 1)
			{
				int CmdLineWeight = CliClass.VerifyCmdLine(ref args);
				bool CmdLineOk = CliClass.IsCmdLineOk(CmdLineWeight, AppCfg.IsAdminPassSetted());

                switch (CmdLineWeight)
                {
                    case 1:			//Load profile
					{
						UserFunctions.LoadProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						break;
					}
					case 2:			//NewProfile without AdminPass
					{
						UserFunctions.NewProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						break;
					}
					case 4:			//Remove profile without AdminPass
					{
						UserFunctions.RemoveProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						break;
					}
					case 8:         //List Profile
					{
						UserFunctions.ListProfilesF();
						break;
					}
					case 16:        //Add2Profile (Ok if dosn't have a password setted)
					{
						UserFunctions.Add2ProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						break;
					}
					case 32:        //AddCsv2Profile (Ok if dosn't have a password setted)
					{
						UserFunctions.AddCsv2Profile(ref args, ref CmdLineOk, ref CmdLineWeight);
						break;
					}
					case 130:       //--NewProfile --AdminPass
					{
						if (AppCfg.ApprovedAdminPass(CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADMINPASS, ref CmdLineOk, ref CmdLineWeight)))
						{
							UserFunctions.NewProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						}
						else
						{
							Console.WriteLine("Password not approved!");
						}
						break;
					}
					case 132:       //--RemoveProfile --AdminPass
					{
						if (AppCfg.ApprovedAdminPass(CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADMINPASS, ref CmdLineOk, ref CmdLineWeight)))
						{
							UserFunctions.RemoveProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						}
						else
						{
							Console.WriteLine("Password not approved!");
						}
						break;
					}
					case 144:       //--Add2Profile --AdminPass
					{
						if (AppCfg.ApprovedAdminPass(CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADMINPASS, ref CmdLineOk, ref CmdLineWeight)))
						{
							UserFunctions.Add2ProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
						}
						else
						{
							Console.WriteLine("Password not approved!");
						}
							break;
					}
					case 160:       //--AddCsv2Profile --AdminPass
					{
						if (AppCfg.ApprovedAdminPass(CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADMINPASS, ref CmdLineOk, ref CmdLineWeight)))
						{
							UserFunctions.AddCsv2Profile(ref args, ref CmdLineOk, ref CmdLineWeight);
						}
						else
						{
							Console.WriteLine("Password not approved!");
						}
						break;
					}
					case 192:       //--AdminConfig --AdminPass
					{
						UserFunctions.AdminConfigF(ref AppCfg, ref args, ref CmdLineOk, ref CmdLineWeight);
						break;
					}
					default:
                    {
						Console.WriteLine("Some arguments are invalid!");
						Console.WriteLine("Arguments used:\n");
						for (int i = 0; i < args.Length; i++)
						{
							Console.WriteLine(args[i]);
						}
						break;
                    }
                }
			}
			else
			{
				Console.WriteLine("Use the arguments to manipulate the Auto Start Apps.");
			}
		}
	}
}
