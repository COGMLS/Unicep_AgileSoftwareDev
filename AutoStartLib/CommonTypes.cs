
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
			public string ProgramName;
			public string CmdLine;
			public string Args;
			public string WorkingDir;

			//Window Style
			public StartWindowStyle windowStyle;

            //Start method:
            public int WaitTime;
			public CommonTypes.StartPriority Priority;
		}
	}
}
