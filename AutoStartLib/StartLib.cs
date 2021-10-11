using System;
using System.Diagnostics;

namespace AutoStartLib
{
	public enum StartWindowStyle
	{
		NOWINDOW = -1,
		NORMAL,
		HIDDEN,
		MINIMIZED,
		MAXIMIZED
	}

	class StartApp
	{
		//Application basic info:
		string ProgramName;
		string CmdLine;
		string Args;
		string WorkingDir;

		bool DefinedAdvancedOptions = false;
		bool useShellEx;
		bool createNoWindow;

		StartWindowStyle windowStyle;

		//Start method:
		int WaitTime = 0;
		CommonTypes.StartPriority Priority;

		//Aplication Statistics:
		float LastStartTime;
		float StartTimeAverage;

		//Constructors:
		StartApp()	//Empty constructor
        {
			this.ProgramName = null;
			this.CmdLine = null;
			this.Args = null;
			this.WorkingDir = null;

			this.windowStyle = StartWindowStyle.NORMAL;

			this.useShellEx = true;
			this.createNoWindow = false;

			this.Priority = CommonTypes.StartPriority.NORMAL;
        }
		StartApp(string programName, string cmdLine, string args, int waitTime)
		{
			this.ProgramName = programName;
			this.CmdLine = cmdLine;
			this.Args = args;
			this.WaitTime = waitTime;

			this.windowStyle = StartWindowStyle.NORMAL;

			this.Priority = CommonTypes.StartPriority.NORMAL;
		}
		StartApp(string programName, string cmdLine, string args, int waitTime, string workingDir)
		{
			this.ProgramName = programName;
			this.CmdLine = cmdLine;
			this.Args = args;
			this.WorkingDir = workingDir;

			this.windowStyle = StartWindowStyle.NORMAL;

			this.Priority = CommonTypes.StartPriority.NORMAL;
		}
		StartApp(string programName, string cmdLine, string args, int waitTime, string workingDir, CommonTypes.StartPriority priority)
		{
			this.ProgramName = programName;
			this.CmdLine = cmdLine;
			this.Args = args;
			this.WorkingDir = workingDir;

			this.windowStyle = StartWindowStyle.NORMAL;

			this.Priority = priority;
		}
		StartApp(string programName, string cmdLine, string args, int waitTime, string workingDir, CommonTypes.StartPriority priority, StartWindowStyle windowStyle)
		{
			this.ProgramName = programName;
			this.CmdLine = cmdLine;
			this.Args = args;
			this.WorkingDir = workingDir;

			this.windowStyle = windowStyle;

			this.Priority = priority;
		}

		//Getters:
		public string GetProgramName() { return this.ProgramName; }
		public string GetCmdLine() { return this.CmdLine; }
		public string GetArgs() { return this.Args; }
		public string GetWorkingDir() { return this.WorkingDir; }
		public StartWindowStyle GetWindowStyle() { return this.windowStyle; }
		public int GetWaitTime() { return this.WaitTime; }
		public CommonTypes.StartPriority GetStartPriority() { return this.Priority; }

		//Setters:
		public void SetProgramName(string ProgramName) { this.ProgramName = ProgramName; }
		public void SetCmdLine(string CmdLine) { this.CmdLine = CmdLine; }
		public void SetArgs(string Args) { this.Args = Args; }
		public void SetWorkingDir(string WorkingDir) { this.WorkingDir = WorkingDir; }
		public void SetWindowStyle(StartWindowStyle winStyle) { this.windowStyle = winStyle; }
		public void SetWaitTime(int waitTime) { this.WaitTime = waitTime; }
		public void SetStartPriority(CommonTypes.StartPriority priority) { this.Priority = priority; }


		//Prepare Start
		private void PrepareStart()
		{
			ProcessStartInfo process = new ProcessStartInfo();
			process.Arguments = this.Args;
			process.FileName = this.CmdLine;
			process.UseShellExecute = this.useShellEx;
			process.WorkingDirectory = this.WorkingDir;
			process.CreateNoWindow = this.createNoWindow;

			if(this.windowStyle == StartWindowStyle.NOWINDOW)
			{
				process.WindowStyle = ProcessWindowStyle.Hidden;
				process.UseShellExecute = false;
			}
			else if(this.windowStyle == StartWindowStyle.HIDDEN)
			{
				process.WindowStyle = ProcessWindowStyle.Hidden;
				process.UseShellExecute = true;
			}
			else if(this.windowStyle == StartWindowStyle.MAXIMIZED)
			{
				process.WindowStyle = ProcessWindowStyle.Maximized;
			}
			else if(this.windowStyle == StartWindowStyle.MINIMIZED)
			{
				process.WindowStyle = ProcessWindowStyle.Minimized;
			}
			else if(this.windowStyle == StartWindowStyle.NORMAL)
			{
				process.WindowStyle = ProcessWindowStyle.Normal;
			}
			else
			{
				process.WindowStyle = ProcessWindowStyle.Normal;
			}
		}


		int StartProcess()
		{
			int returnValue = 0;



			return returnValue;
		}
	}
}