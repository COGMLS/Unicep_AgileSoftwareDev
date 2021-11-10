using System;
using System.Security.Cryptography;

namespace ConfigLib
{
	public class ConfigLib
	{
		//Private variables for configurate the program
		private byte[] AdminPass = null;
		bool IsAdminPass;
		bool IsAdd2ProfileCl;
		bool IsAddCsv2ProfileCl;

		//Public vaiables to define the program configurations:
		string ProfileFocus = null;

		//Public functions to configurate properly the application:


		//Private functions to configurate the application:

		//Creates a buffer byte array from a string to be used to calculate the hash code
		byte[] AdminPassBuffer(string Password)
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
