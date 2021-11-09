using System;
using System.IO;

namespace MetricsLib
{
	//Metrics class to load, save, update and calculates the metrics for the applications.
	public static class Metrics
	{
		readonly static private string AutoStartAppsBase = "Auto Start Apps";
		readonly static private string ProfilesBase = "Profiles";
		readonly static private string ProfileFolderExt = ".profile";
		readonly static private string ProfileInternal_Init = "Init";
		readonly static private string ProfileInternal_Estatistics = "PerfAnalyze";

		//Contant value to limit the maximum length to keep.
		const int MetricsLengthLimit = 30;

		//Load the program metrics and returns a float array from the last 30 times.
		public static float[] LoadMetrics(ref string ProfileContainer ,ref string ProgramName)
		{
			//Receaves the loaded string array.
			string[] MetricsTemp = LoadMetricFile(ref ProfileContainer, ref ProgramName);

			if (MetricsTemp != null)
			{
				//Creates a float array and converts the strings to floating points.
				float[] MetricsF = new float[MetricsTemp.Length];

				for (int i = 0; i < MetricsTemp.Length; i++)
				{
					MetricsF[i] = float.Parse(MetricsTemp[i]);
				}

				return MetricsF;
			}
			else
			{
				return null;
			}
		}

		//Load the saved program metrics file
		private static string[] LoadMetricFile(ref string ProfileContainer, ref string ProgramName)
		{
			string MetricsDir = ProfileContainer + "\\" + ProfileInternal_Estatistics;

			//Verify if exist the metrics file and load it to a string array.
			if (File.Exists(MetricsDir + "\\" + ProgramName))
			{
				string[] MetricsTemp = File.ReadAllLines(MetricsDir + "\\" + ProgramName);

				return MetricsTemp;
			}
			else
			{
				return null;
			}
		}

		//Save the metrics on storage device.
		public static void SaveMetrics(ref string ProfileContainer, ref string ProgramName, ref float[] Metrics)
		{
			string MetricsDir = ProfileContainer + "\\" + ProfileInternal_Estatistics;

			string[] MetricsStringFile = new string[Metrics.Length];

			//Convert the float array to a string array
			for (int i = 0; i < Metrics.Length; i++)
			{
				MetricsStringFile[i] = Metrics[i].ToString();
			}

			File.WriteAllLines(MetricsDir + "\\" + ProgramName, MetricsStringFile);
		}

		//Calculates the Average time
		public static float MetricsAvgTime(ref float[] Metrics)
		{
			float Avg = 0.0f;

			int j = 0;

			for(int i = 0; i < Metrics.Length; i++)
			{
				Avg += Metrics[i];
				j++;
			}

			Avg /= j;

			return Avg;
		}

		//Updates the metrics
		public static void UpdateMetrics(ref float[] Metrics, ref float NewMetric)
		{
			//Index variable, used to start from the end.
			int i = Metrics.Length - 1;

			//Verify if the Metrics array exceded the maximum length.
			if(Metrics.Length > MetricsLengthLimit)
            {
				i = MetricsLengthLimit - 1;
            }
			
			//Updates the metrics history keeping the most recent in index 0
			for( ; i >= 0; i--)
            {
				//Verify if the history limit is respected with a maximum defined by MetricsLengthLimit constant value and if the index limit is respected.
				if((i + 1 < MetricsLengthLimit) && (i + 1 < Metrics.Length) && i >= 0)
                {
					//Move the data forward.
					Metrics[i + 1] = Metrics[i];
					
					//First move the data and after updates the index 0.
					if(i == 0)
                    {
						Metrics[0] = NewMetric;
                    }
                }
				else	//In case something goes wrong.
                {
					break;
                }

            }
		}
	}
}
