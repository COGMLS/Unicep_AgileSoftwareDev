using System;
using System.Security.Cryptography;
using System.IO;

namespace ConfigLib
{
	public class AdminCfg
	{
		//
		//Private variables for configurate the program
		//
		protected byte[] AdminPass = null;

		//
		// Admin public functions:
		//

		//Define the administrator password
		public void SetAdminPass(string Password, string NewPassword)
		{
			string adm_bin = ConfigCommonTypes.CommonConfigPath + "\\adm.bin";
			if (File.Exists(adm_bin))
			{
				if (ApprovedAdminPass(Password))
				{
					byte[] buffer = AdminPassBuffer(NewPassword);
					File.WriteAllBytes(adm_bin, buffer);
				}
				else
				{
					Console.WriteLine("Admin Password NOT APPROVED!");
				}
			}
			else
			{
				byte[] buffer = AdminPassBuffer(NewPassword);
				File.WriteAllBytes(adm_bin, buffer);
			}
		}

		//Get from the user the password and already test if is correct. In case the AdminPass wasn't setted, th return value will be always true.
		public bool GetAndTestPassword()
		{
			bool PasswordApporved = false;

			if (IsAdminPassSetted())
			{
				Console.WriteLine("Enter with the Administrator Password: ");
				string Password = Console.ReadLine();

				PasswordApporved = ApprovedAdminPass(Password);

				if (!PasswordApporved)
				{
					Console.WriteLine("\n\n[WARNING]::PASSWORD NOT APPROVED!\n\n");
				}

				return PasswordApporved;
			}
			return true;
		}

		//Check if the password is correct
		public bool ApprovedAdminPass(string Password)
		{
			return this.AdminPass.Equals(Password2HashCode(Password));
		}

		//Shows if the AdminPass is setted or not
		public bool IsAdminPassSetted()
		{
			if (AdminPass != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//
		//Private functions to configurate the application:
		//

		//Creates a buffer byte array from a string to be used to calculate the hash code
		private byte[] AdminPassBuffer(string Password)
		{
			byte[] buffer = new byte[Password.Length];

			foreach (var item in Password)
			{
				buffer[item] = (byte)Password[item];
			}

			return buffer;
		}

		//Converts the password to byte array that represents a Hash code (SHA-256)
		private byte[] Password2HashCode(string Password)
		{
			HashAlgorithm AdminPass = HashAlgorithm.Create("SHA256");
			return AdminPass.ComputeHash(AdminPassBuffer(Password));
		}
	}
}
