using System;
using System.Diagnostics;

using MetricsLib;

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

	public class StartApp
	{
		//Application basic info:
		private string ProgramName;
		private string CmdLine;
		private string Args;
		private string WorkingDir;

		private bool DefinedAdvancedOptions = false;
		private bool useShellEx;
		private bool createNoWindow;

		StartWindowStyle windowStyle;

		//Start method:
		private int WaitTime = 0;
		private CommonTypes.StartPriority Priority;

		//Aplication Statistics:
		private float[] StartTimeHistory;
		private float StartTimeAverage = -1;

		//Application process obj:
		ProcessStartInfo ObjProcess = null;

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
		StartApp(string programName, string cmdLine, string args, int waitTime, CommonTypes.StartPriority priority, StartWindowStyle windowStyle)
		{
			this.ProgramName = programName;
			this.CmdLine = cmdLine;
			this.Args = args;

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
			this.ObjProcess = new ProcessStartInfo();
			this.ObjProcess.Arguments = this.Args;
			this.ObjProcess.FileName = this.CmdLine;
			this.ObjProcess.UseShellExecute = this.useShellEx;
			this.ObjProcess.WorkingDirectory = this.WorkingDir;
			this.ObjProcess.CreateNoWindow = this.createNoWindow;

			if(this.windowStyle == StartWindowStyle.NOWINDOW)
			{
				this.ObjProcess.WindowStyle = ProcessWindowStyle.Hidden;
				this.ObjProcess.UseShellExecute = false;
			}
			else if(this.windowStyle == StartWindowStyle.HIDDEN)
			{
				this.ObjProcess.WindowStyle = ProcessWindowStyle.Hidden;
				this.ObjProcess.UseShellExecute = true;
			}
			else if(this.windowStyle == StartWindowStyle.MAXIMIZED)
			{
				this.ObjProcess.WindowStyle = ProcessWindowStyle.Maximized;
			}
			else if(this.windowStyle == StartWindowStyle.MINIMIZED)
			{
				this.ObjProcess.WindowStyle = ProcessWindowStyle.Minimized;
			}
			else if(this.windowStyle == StartWindowStyle.NORMAL)
			{
				this.ObjProcess.WindowStyle = ProcessWindowStyle.Normal;
			}
			else
			{
				this.ObjProcess.WindowStyle = ProcessWindowStyle.Normal;
			}

			this.StartTimeHistory = Metrics.LoadMetrics(this.ProgramName);

			if (this.StartTimeHistory != null)
			{
				this.StartTimeAverage = Metrics.MetricsAvgTime(ref this.StartTimeHistory);

			}
		}


		int StartProcess()
		{
			int returnValue = 0;



			return returnValue;
		}
	}
}