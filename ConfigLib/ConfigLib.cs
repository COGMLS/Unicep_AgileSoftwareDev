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
		bool IsAdminPass;
		bool IsAdd2ProfileCl;
		bool IsAddCsv2ProfileCl;
		bool IsLoadProfile;
		bool IsNewProfile;
		bool IsRemoveProfile;
		bool IsListProfile;

		//
		//Public vaiables to define the program configurations:
		//
		string ProfileFocus = null;

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

		//Define is Add2Profile var
		public void SetAdd2ProfileControl(bool value)
        {
			this.IsAdd2ProfileCl = value;
        }

		//Define is AddCsv2Profile var
		public void SetAddCsv2ProfileControl(bool value)
        {
			this.IsAddCsv2ProfileCl = value;
        }

		//Define is LoadProfile var
		public void SetLoadProfile(bool value)
		{
			this.IsLoadProfile = value;
		}

		//Define is NewProfile var
		public void SetNewProfile(bool value)
		{
			this.IsNewProfile = value;
		}

		//Define is RemoveProfile var
		public void SetRemoveProfile(bool value)
		{
			this.IsRemoveProfile = value;
		}

		//Define is ListProfile var
		public void SetListProfile(bool value)
		{
			this.IsListProfile = value;
		}

		//Define the profile focus to work
		public void SetProfileFocus(string ProfileName)
        {
			this.ProfileFocus = ProfileName;
        }

		//
		// Getters for configs:
		//

		//Shows the focused profile
		public string GetProfileFocus()
        {
			return this.ProfileFocus;
        }
	}
}
