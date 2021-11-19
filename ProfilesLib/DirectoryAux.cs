/********************************************************************
 * Project: User Path Converter for Unix and Windows systems
 * ******************************************************************
 * Author: Matheus L. Silvati
 * ***********************************
 * Version: 1.5
 * ***********************************
 * First Release Date: 2021/06/15
 * Last Release Date: 2021/08/20
 * ***********************************
 * All rights reserved
 * ******************************************************************
 * OBS: This lib. has some functions to make more simple for console
 * .NET applications when is used path in user interactions, including
 * relative paths.
 */

/**********************************************
 * NOTE: this lib is imported to this project.
 * This library dosn't belong to any specific
 * project.
 * ********************************************
 */

using System;
using System.IO;

/* DirectoryAux namespace for treatment classes */
namespace DirectoryAux
{
	/* Directory Treatment class to treat correctly the
	 * paths to directory and auxiliate some basic and
	 * common operations with directories.
	 * -------------------------------------------------
	 * This class is developed to work if Windows and
	 * Unix like systems.
	 */
	static public class DirectoryTreatment
	{
		//Verify is a relative path.
		static public bool IsRelativePath(string Path)
		{
			if (Path.StartsWith(".\\"))
			{
				return true;
			}
			else if (Path.StartsWith("./"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//Check and transform the relative path.
		static public string TransformRelativePath(string RelativePath)
		{
			if (RelativePath.StartsWith(".\\"))
			{
				string CurrentDir = Directory.GetCurrentDirectory();
				string Path = CurrentDir + "\\" + RelativePath.Substring(2);

				//Return the path for Windows systems.
				return Path;
			}
			else if (RelativePath.StartsWith("./"))
			{
				string CurrentDir = Directory.GetCurrentDirectory();
				string Path;

				if (OperatingSystem.IsWindows())
				{
					//Converts '/' to Windows compatible system with '\'.
					Path = CurrentDir + "\\" + RelativePath.Substring(2);
				}
				else
				{
					Path = CurrentDir + "/" + RelativePath.Substring(2);
				}

				//Return a treated path with a convertion if is appropriated or for Unix systems.
				return Path;
			}
			else
			{
				//return a NULL value if isn't a relative path.
				return null;
			}
		}

		//Get a files list string array in the path.
		static public string[] GetFilesName(string Path)
		{
			//In case a relative path is passed.
			if (IsRelativePath(Path))
			{
				Path = TransformRelativePath(Path);
			}

			//Get the files list with a full paths.
			string[] FilesList = Directory.GetFiles(Path);
			int PathCut = Path.Length + 1;

			//Verify if the FilesList has some file(s) to treat.
			if (FilesList.Length != 0)
			{
				string[] FinalFilesList = new string[FilesList.Length];

				for (int i = 0; i < FilesList.Length; i++)
				{
					FinalFilesList[i] = FilesList[i].Substring(PathCut);
				}

				//Return the treated list array.
				return FinalFilesList;
			}
			else
			{
				//Return a NULL value in case a zeroed list.
				return null;
			}
		}
	}
}