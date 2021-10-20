using System;
using System.IO;

namespace MetricsLib
{
    public struct MetricsS
    {
        MetricsS(float LastStartT, float StartTimeAvg, float[] MetricsHistory)
        {
            LastStartTime = LastStartT;
            StartTimeAverage = StartTimeAvg;
            MetricsStartTimeHistory = MetricsHistory;
        }

        float LastStartTime;
        float StartTimeAverage;
        float[] MetricsStartTimeHistory;

        //Getters:
        public float GetLastTime()
        {
            return LastStartTime;
        }
        public float GetStartTimeAvg()
        {
            return StartTimeAverage;
        }
        public float[] GetMetricsHistory()
        {
            return MetricsStartTimeHistory;
        }

        //Setters:
        private void SetLastTime(ref float value)
        {
            LastStartTime = value;
        }
        private void SetStartTimeAvg(ref float value)
        {
            StartTimeAverage = value;
        }
        private void SetMetricsHistory(ref float[] value)
        {
            MetricsStartTimeHistory = value;
        }
    }

    public static class Metrics
    {
        readonly static private string AutoStartAppsBase = "Auto Start Apps";
        readonly static private string ProfilesBase = "Profiles";
        readonly static private string ProfileFolderExt = ".profile";
        readonly static private string ProfileInternal_Init = "Init";
        readonly static private string ProfileInternal_Estatistics = "PerfAnalyze";

        //Load the program metrics and returns a float array from the last 30 times.
        public static float[] LoadMetrics(string ProgramMetrics)
        {
            string[] MetricsTemp = LoadMetricFile(ref ProgramMetrics);

            if (MetricsTemp != null)
            {
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
        private static string[] LoadMetricFile(ref string ProgramMetrics)
        {
            string LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string MetricsDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase + "\\" + ProfileInternal_Estatistics;

            if (File.Exists(MetricsDir + "\\" + ProgramMetrics))
            {
                string[] MetricsTemp = File.ReadAllLines(MetricsDir + "\\" + ProgramMetrics);

                return MetricsTemp;
            }
            else
            {
                return null;
            }
        }

        //Save the metrics on storage device.
        public static void SaveMetrics(string ProgramMetrics, ref float[] Metrics)
        {
            if (ProgramMetrics != null)
            {
                string LocalAppDataEnv = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string MetricsDir = LocalAppDataEnv + "\\" + AutoStartAppsBase + "\\" + ProfilesBase + "\\" + ProfileInternal_Estatistics;

                string[] MetricsStringFile = new string[Metrics.Length];

                for (int i = 0; i < Metrics.Length; i++)
                {
                    MetricsStringFile[i] = Metrics[i].ToString();
                }

                File.WriteAllLines(MetricsDir + "\\" + ProgramMetrics, MetricsStringFile);
            }
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
        public static void UpdateMetrics(ref float[] Metrics, float NewMetric)
        {
            //Updates the metrics history keeping the most recent in index 0
            for(int i = 0; i < 30; i++)
            {
                //Verify if the history limit is respected with a maximum of 30 values and if the index limit is respected.
                if (i < 30 && i < (Metrics.Length - 2))
                {
                    //Move the data to the end.
                    Metrics[i + 1] = Metrics[i];

                    //First move the data and after updates the index 0.
                    if (i == 0)
                    {
                        Metrics[0] = NewMetric;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
