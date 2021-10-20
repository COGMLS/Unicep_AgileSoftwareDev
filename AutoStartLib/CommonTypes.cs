
namespace AutoStartLib
{
	public class CommonTypes
	{
		//Enumerator for define the priority values
		public enum StartPriority : int
		{
			LOW,
			NORMAL,
			HIGH
		}

		//Struct to import the initialization list
		public struct ImportList
		{
			//Application Info:
			string ProgramName;
			string CmdLine;
			string Args;
			string WorkingDir;

			bool DefinedAdvancedOptions;
			bool useShellEx;
			bool createNoWindow;

			StartWindowStyle windowStyle;

            //Start method:
            int WaitTime;
			CommonTypes.StartPriority Priority;

			//Aplication Statistics:
			float[] StartTimeHistory;
			float StartTimeAverage;
		}
	}
}
