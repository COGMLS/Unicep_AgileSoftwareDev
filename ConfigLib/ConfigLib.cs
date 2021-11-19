using System;
using System.IO;

namespace ConfigLib
{
	public class Config : AdminCfg
	{
		//
		//Constants:
		//
		private readonly string CommonConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + "Auto Start Apps";


		//
		//Private variables for configurate the program
		//
		private bool IsProfileSelected = false;
		private string ProfileFocus = null;

		//
		//Public vaiables to define the program configurations:
		//
		

		//Contructor for ConfigLib
		public Config()
        {
			string adm_bin = this.CommonConfigPath + "\\adm.bin";
			if (File.Exists(adm_bin))
            {
				this.AdminPass = File.ReadAllBytes(adm_bin);
            }
		}

		//
		//Public functions to configurate properly the application:
		//

		//
		// Setters to define the current configurantion is used in actual instance.
		//

		//Define the profile focus to work
		public void SetProfileFocus(string ProfileName)
        {
			if(ProfileName == null)
            {
				this.IsProfileSelected = false;
            }

			this.ProfileFocus = ProfileName;
        }

		public void SetProfileStatus(bool ProfileStatus)
        {
			this.IsProfileSelected = ProfileStatus;
        }

		//
		// Getters for configs:
		//

		//Shows the focused profile
		public string GetProfileFocus()
        {
			if(this.ProfileFocus == null)
            {
				return "NONE PROFILE SELECTED";
            }
			return this.ProfileFocus;
        }

		//Show if has a profile selected.
		public bool GetProfileStatus(bool ShowWarningMessage = false)
        {
			if(ShowWarningMessage)
            {
				if(!this.IsProfileSelected)
                {
					Console.WriteLine("\nNO PROFILE SELECTED!\n");
                }
            }
			return this.IsProfileSelected;
        }
	}
}
