/* Project: Auto Startup Apps
 * ***********************************************
 * Authors:
 *          Matheus Lopes Silvati | RA: 4200872
 *          Caroline Paganelli Corrêa dos Santos | 4200890
 * ***********************************************
 * Professor: Wesley Pecoraro
 * ***********************************************
 * Course: Computer Engineering
 * Discipline: P.I.M. - VI
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
			 * 1. Create the config obj and verify if has a admin password.
			 * 2. Check the cmd line weight sum.
			 * 3. Verify if the cmd line is ok.
			 * 4. Get the values of each variable.
			 * 5. Create the config obj, sending all args info.
			 * 6. In case the application don't have arguments, start the user interactive mode.
			 */

			 Config AppCfg = new Config();

			if(args.Length > 0)
			{
				int CmdLineWeight = CliClass.VerifyCmdLine(ref args);
				bool CmdLineOk = CliClass.IsCmdLineOk(CmdLineWeight, AppCfg.IsAdminPassSetted());

				if (CliClass.IsCommandHelp(ref args))
				{
					string[] HelpCommands =
					{
						"Command Line Help (Activated by -? -h --help):",
						"===========================================================",
						"All comands, except --RemApp use the sintax --Command <Value>",
						"To use --RemApp, use the sintax: --RemApp ProfileName,ProgramName",
						"===========================================================\n",
						"--LoadProfile - Load profile (for all users)",
						"--NewProfile - Create a new profile (needs --AdminPass)",
						"--RemoveProfile - Remove and delete a profile (needs --AdminPass)",
						"--ListProfiles - List the profiles available for the user account (for all users)",
						"--Add2Profile - Adds a new application to start initialization list (needs --AdminPass)",
						"--RemApp - Removes a application registered in a profile (needs --AdminPass)",
						"--AddCsv2Profile - Adds a CSV file with each line as a app entry to add in init. list. The sequence of values needs be ProgramName, CmdLine, Args, WaitTime, WorkingDir*, Priority2Start, WindowStyle",
						"--AdminConfig - Changes the AdminPass, if AdminPass isn't setted, the AdminPass will be avoided (needs --AdminPass)",
						"--AdminPass - Administrator Password"
					};

					foreach (var item in HelpCommands)
					{
						Console.WriteLine(item);
					}
				}
				else
				{
					switch (CmdLineWeight)
					{
						case 1:         //Load profile
						{
							UserFunctions.LoadProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
							break;
						}
						case 2:         //NewProfile without AdminPass
						{
							UserFunctions.NewProfileF(ref args, ref CmdLineOk, ref CmdLineWeight);
							break;
						}
						case 4:         //Remove profile without AdminPass
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
						case 32:        //RemApp (Ok if dosn't have a password setted)
						{
							UserFunctions.RemAppF(ref args, ref CmdLineOk, ref CmdLineWeight);
							break;
						}
						case 64:        //AddCsv2Profile (Ok if dosn't have a password setted)
						{
							UserFunctions.AddCsv2Profile(ref args, ref CmdLineOk, ref CmdLineWeight);
							break;
						}
						case 258:       //--NewProfile --AdminPass
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
						case 260:       //--RemoveProfile --AdminPass
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
						case 272:       //--Add2Profile --AdminPass
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
						case 288:       //--RemApp --AdminPass
						{
							if (AppCfg.ApprovedAdminPass(CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADMINPASS, ref CmdLineOk, ref CmdLineWeight)))
							{
								UserFunctions.RemAppF(ref args, ref CmdLineOk, ref CmdLineWeight);
							}
							else
							{
								Console.WriteLine("Password not approved!");
							}
							break;
						}
						case 320:       //--AddCsv2Profile --AdminPass
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
						case 384:       //--AdminConfig --AdminPass
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
			}
			else
			{
				//Variable to control the user interation loop.
				bool KeepLoop = true;

				//Will work like a "pointer" to the working profile.
				Profile profile = null;

                do
				{
					//Presentation
					Console.WriteLine("Auto Start Apps\n=========================================");
					Console.WriteLine("Autores: Matheus Lopes Silvati | RA: 4200872\nCaroline Paganelli Corrêa dos Santos | RA: 4200890");
					Console.WriteLine("Professor: Wesley Pecoraro");
					Console.WriteLine("Curso: Engenharia de Computação | Período: 6º - Noturno");
					Console.WriteLine("Disciplina: P.I.M. - IV");
					Console.WriteLine("=========================================");
					Console.WriteLine("Auto Start Apps: Interactive Mode");

					//Inform the selected profile.
					Console.WriteLine("\n======================================\nProfile selected: {0}\n======================================\n", AppCfg.GetProfileFocus());

					//Get the command from the user
					Console.Write("Enter with a command (help for more information): ");
					string UserEntry = Console.ReadLine();

                    switch (UserEntry.ToLower())
                    {
						case "getprofile":	//Select the profile to work.
                        {
							Console.Write("Enter with the Profile Name to select: ");
							string ProfileName = Console.ReadLine();

							bool ProfileExist = AuxiliarProfileManager.ChkProfile(ProfileName);

							profile = new Profile(ProfileName);

							AppCfg.SetProfileStatus(ProfileExist);
							AppCfg.SetProfileFocus(ProfileName);
							int LoadReturnValue = profile.LoadProfile();

							if (LoadReturnValue == 0)
							{
								Console.WriteLine("[INFO]::Profile loaded with success!");
							}
							else if (LoadReturnValue == -1)
							{
								Console.WriteLine("[INFO]::Profile loaded, but there are no files to be loaded.");
							}
							else if (LoadReturnValue == -2)
							{
								Console.WriteLine("[INFO]::Profile dosn't exist! Trying create one... Reload and try again.");
								AppCfg.SetProfileFocus(null);
							}

							break;
                        }
						case "listprofiles":	//List all profiles available.
                        {
							string[] ProfileList = AuxiliarProfileManager.GetProfiles();
							Console.WriteLine("List of profiles available\n======================================");
							if (ProfileList != null)
							{
								foreach (var item in ProfileList)
								{
									Console.WriteLine(item);
								}
							}
							else
                            {
								Console.WriteLine("There are no profiles available!");
                            }
							Console.WriteLine("======================================\n");
							break;
                        }
						case "newprofile":		//Create a new profile.
                        {
							Console.Write("Enter with a name for new profile: ");
							string NewProfileName = Console.ReadLine();

							bool NewProfileStatus = AuxiliarProfileManager.ChkProfile(NewProfileName);

							if (AppCfg.GetAndTestPassword())
							{
								if (!NewProfileStatus)
								{
									profile = new Profile(NewProfileName);
									AppCfg.SetProfileFocus(NewProfileName);
									AppCfg.SetProfileStatus(true);
								}
								else
								{
									Console.WriteLine("Error: Already have a profile with this name.");
								}
							}
							break;
                        }
						case "remprofile":	//Remove a profile.
                        {
							Console.Write("Enter with the profile to remove: ");
							string RemoveProfile = Console.ReadLine();

							if (AppCfg.GetAndTestPassword())
							{
								int RemoveReturn = AuxiliarProfileManager.RemoveProfile(RemoveProfile);

								if (RemoveProfile == AppCfg.GetProfileFocus())
								{
									AppCfg.SetProfileFocus(null);
								}

								if (RemoveReturn == 0)
								{
									Console.WriteLine("Profile removed with success.");
								}
								else if (RemoveReturn == -1)
								{
									Console.WriteLine("Fail to remove the profile container.");
								}
								else if (RemoveReturn == -2)
								{
									Console.WriteLine("The path to the profile dosn't exist.");
								}
								else
								{
									Console.WriteLine("Profile Repository dosn't exist.");
								}
							}
							break;
                        }
						case "addapp":	//Adds a new app to profile.
                        {
							if (AppCfg.GetProfileStatus(true))
							{
								if (AppCfg.GetProfileStatus(true))
								{
									int LoadReturnValue = profile.LoadProfile();

									if(LoadReturnValue == -1)
                                    {
										profile.InitializeInitList();
                                    }
									else
                                    {
										Console.WriteLine("Actual Initialization size: {0}", profile.GetInitSize());
                                    }
									
									Console.Write("Enter with the number of additions will do: ");
									string SizeStr = Console.ReadLine();

									if(int.TryParse(SizeStr, out _))
                                    {
										int Entries = int.Parse(SizeStr);

										UserInterations.GetAppEntriesTUI(ref profile, ref Entries);

										profile.SaveProfile();
                                    }
									else
                                    {
										Console.WriteLine("Fail to receave the size.");
                                    }
								}
							}
							break;
                        }
						case "remapp":	//Remove a app from the profile.
                        {
							if (AppCfg.GetProfileStatus(true))
							{
								if (AppCfg.GetProfileStatus(true))
								{
									Console.Write("Enter with the program's name to remove: ");
									string RemoveProgramName = Console.ReadLine();

									string profileStr = AppCfg.GetProfileFocus();
									UserInterations.RemAppInteraction(ref profileStr, ref RemoveProgramName);

									profile.LoadProfile();
								}
							}
							break;
                        }
						case "listapps":	//List all apps in initialization's profile.
                        {
							string[] StartList = profile.ListStart();
							Console.WriteLine("Apps in start list:\n");
                            foreach (var item in StartList)
                            {
								Console.WriteLine(item);
                            }
							break;
                        }
						case "configadmin":	//Config admin pass.
                        {
							if (AppCfg.GetAndTestPassword())
							{
								int CmdLineWeight = 384;
								bool CmdLineOk = true;
								UserFunctions.AdminConfigF(ref AppCfg, ref args, ref CmdLineOk, ref CmdLineWeight);
							}
							break;
                        }
						case "startapps":	//Start the apps in profile.
                        {
							if (AppCfg.GetProfileStatus(true))
							{
								if (AppCfg.GetProfileStatus(true))
								{
									profile.LoadProfile();
									profile.StartAppList();
									profile.SaveProfile();
								}
							}
							break;
                        }
						case "help":		//Show the help.
                        {
							string[] HelpCommands =
							{
								"Help Console Command:",
								"===========================================================\n",
								"GetProfile - Selects a profile to work.",
								"ListProfiles - List all profiles for current user.",
								"NewProfile - Creates a new profile.",
								"RemProfile - Removes a profile.",
								"AddApp - Adds a new application to initialization list.",
								"RemApp - Removes an application from a profile.",
								"ConfigAdmin - Configurate a administrator password.",
								"StartApps - Start apps registered in a profile.",
								"Exit - Close this application."
                            };

                            foreach (var item in HelpCommands)
                            {
								Console.WriteLine(item);
                            }
							break;
                        }
						case "exit":		//Exit the app.
                        {
							KeepLoop = false;
							break;
                        }
                        default:
						{
							Console.WriteLine("Command not recognized! Try again.");
							break;
						}
                    }

					//Wait for user to continue and give time to read all information in buffer.
					Console.Write("Press any key to continue...");
					_ = Console.ReadLine();

					//Clean the buffer to keep organized.
					Console.Clear();
                }
				while (KeepLoop);
			}
		}
	}
}
