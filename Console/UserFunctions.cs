//System libs:
using System;
using System.IO;

//App libs:
using ConfigLib;
using ProfilesLib;
using AutoStartLib;

namespace AutoStartupConsole
{
    public static class UserFunctions
    {
        public static void LoadProfileF(ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight)
        {
            string ProfileName = CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.LOADPROFILE, ref CmdLineOk, ref CmdLineWeight);
            Profile profile = new Profile(ProfileName);
            _ = profile.LoadProfile();
            profile.StartAppList();
            profile.SaveProfile();
        }

        public static void NewProfileF(ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight)
        {
            string ProfileName = CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.NEWPROFILE, ref CmdLineOk, ref CmdLineWeight);
            Profile profile = new Profile(ProfileName);
            Console.Write("Enter with how many initialization entries you will add: ");
            string EntriesS = Console.ReadLine();
            int Entries = 0;

            if (int.TryParse(EntriesS, out Entries))
            {
                if (Entries > 0)
                {
                    UserInterations.GetAppEntriesTUI(ref profile, ref Entries);
                }
                else
                {
                    Console.WriteLine("Only numbers greater than zero!");
                }
            }
        }

        public static void RemoveProfileF(ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight)
        {
            string ProfileName = CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.REMOVEPROFILE, ref CmdLineOk, ref CmdLineWeight);
            UserInterations.RemoveProfileInteration(ref ProfileName);
        }

        public static void ListProfilesF()
        {
            string[] ProfilesList = AuxiliarProfileManager.GetProfiles();

            if (ProfilesList != null)
            {
                Console.WriteLine("List Profile Available:\n--------------------------------------------");
                for (int i = 0; i < ProfilesList.Length; i++)
                {
                    Console.WriteLine(ProfilesList[i]);
                }
                Console.WriteLine("--------------------------------------------");
            }
            else
            {
                Console.WriteLine("No profiles available.");
            }
        }

        public static void Add2ProfileF(ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight)
        {
            string ProfileName = null;
            CommonTypes.ImportList List = UserInterations.Add2ProfileCmd(ref ProfileName, ref args, ref CmdLineOk, ref CmdLineWeight);

            if (ProfileName != null)
            {
                Profile profile = new Profile(ProfileName);

                profile.Add2StartList(List.ProgramName, List.CmdLine, List.Args, List.WorkingDir, List.windowStyle, List.Priority, List.WaitTime);
            }
            else
            {
                Console.WriteLine("Wasn't possible read all the data.");
            }
        }

        public static void RemAppF(ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight)
        {
            string RemAppValue = CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.REMAPP, ref CmdLineOk, ref CmdLineWeight);

            string ProfileName = null;
            string ProgramName = null;

            bool isProgramName = false;

            //Extract the values of ProfileName and ProgramName
            for(int i = 0; i < RemAppValue.Length; i++)
            {
                if(RemAppValue[i] == ',')
                {
                    i++;
                    isProgramName = true;
                }
                if(isProgramName)
                {
                    ProgramName += RemAppValue[i];
                }
                else
                {
                    ProfileName += RemAppValue[i];
                }
            }

            if (ProfileName != null)
            {
                UserInterations.RemAppInteraction(ref ProfileName, ref ProgramName);
            }
            else
            {
                Console.WriteLine("Wasn't possible read the data.");
            }
        }

        public static void AddCsv2Profile(ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight)
        {
            string CsvPath = CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADDCSV2PROFILE, ref CmdLineOk, ref CmdLineWeight);

            if (File.Exists(CsvPath))
            {
                string[] Csv = File.ReadAllLines(CsvPath);

                for (int i = 0; i < Csv.Length; i++)
                {
                    string ProfileName = null;
                    CommonTypes.ImportList List = UserInterations.Add2ProfileCmd(ref ProfileName, ref args, ref CmdLineOk, ref CmdLineWeight);

                    if (ProfileName != null)
                    {
                        Profile profile = new Profile(ProfileName);

                        profile.SetInitSize(profile.GetInitSize() + 1);

                        profile.Add2StartList(List.ProgramName, List.CmdLine, List.Args, List.WorkingDir, List.windowStyle, List.Priority, List.WaitTime);
                    }
                    else
                    {
                        Console.WriteLine("Wasn't possible read all the data.");
                    }
                }
            }
        }

        public static void AdminConfigF(ref Config AppCfg, ref string[] args, ref bool CmdLineOk, ref int CmdLineWeight, bool IsCmdLine = true)
        {
            string Password = null;
            if (IsCmdLine)
            {
                Password = CliClass.GetClValue(ref args, CliClass.CommandLineAvailable.ADMINPASS, ref CmdLineOk, ref CmdLineWeight);
            }
            Console.Write("Enter with the new password: ");
            string NewPassword = Console.ReadLine();
            AppCfg.SetAdminPass(Password, NewPassword);
        }
    }
}
