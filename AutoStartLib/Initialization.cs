using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStartLib
{
	public class InitializationList
	{
		//Handle to a Initialization list:
		//private StartApp[] HStartList = null;
		private List<StartApp> HStartList;

		//Index in use:
		private int HStartIndex;

		//List controls:
		private int MaxSimultaneousInit = 0;	//Reserved for a future update to support multithreading.

		//Constructors:
		public InitializationList()
		{
			this.HStartIndex = -1;	//No elements in use.
		}
		public InitializationList(int SimInit)
		{
			this.MaxSimultaneousInit = SimInit;
			this.HStartIndex = -1;	//No elements in use.
		}

		//Size mng functions
		public int GetInitSize()
		{
			if(this.HStartList == null)
            {
				return 0;
            }
			return this.HStartList.Count;
		}
		public int GetInitIndexPos()
        {
			return this.HStartIndex;
        }

		//Organize List by Priority
		private void SortList()
		{
			SortClass.Sort(ref this.HStartList);
		}

		//Prepare Init List
		public void PrepareInitList2Start()
        {
			this.SortList();
        }

		//Add a object to initialyze:
		public int Add2Init(string programName, string cmdLine, string args, int waitTime, string workingDir, CommonTypes.StartPriority priority, StartWindowStyle windowStyle, string ProfileContainer = null)
		{
			if (GetInitSize() > 0)
			{
				//Atribute the configurations for the object
				StartApp temp = new StartApp(programName, cmdLine, args, waitTime, workingDir, priority, windowStyle);
				//Set the ProfileContainer path for metrics loading files
				temp.SetProfileContainer(ProfileContainer);

				this.HStartList.Add(temp);

				this.HStartIndex++;

				return 0;
			}
			else
			{
				return -1;
			}
		}
		public int Add2Init(string programName, string cmdLine, string args, int waitTime, CommonTypes.StartPriority priority, StartWindowStyle windowStyle, string ProfileContainer = null)
		{
			if (GetInitSize() > 0)
			{
				//Atribute the configurations for the object
				StartApp temp = new StartApp(programName, cmdLine, args, waitTime, priority, windowStyle);
				//Set the ProfileContainer path for metrics loading files
				temp.SetProfileContainer(ProfileContainer);

				this.HStartList.Add(temp);

				this.HStartIndex++;

				return 0;
			}
			else
			{
				return -1;
			}
		}

		//Export object list
		public string[] ExpObjList()
		{
			if (this.GetInitSize() > 0)
			{
				string[] ObjList = new string[this.GetInitSize()];

				for (int i = 0; i <= this.GetInitIndexPos(); i++)
				{
					ObjList[i] = this.HStartList[i].GetProgramName();
				}

				return ObjList;
			}
			else
			{
				return null;
			}
		}

		//Export the object information
		public string[] ExpObjInfo(int Index)
        {
			string[] ExportList = null;

			if(Index >= 0 && Index <= this.GetInitIndexPos())
            {
				ExportList = new string[7];

				ExportList[0] = this.HStartList[Index].GetProgramName();
				ExportList[1] = this.HStartList[Index].GetCmdLine();
				ExportList[2] = this.HStartList[Index].GetArgs();
				ExportList[3] = this.HStartList[Index].GetWorkingDir();
				ExportList[4] = this.HStartList[Index].GetWindowStyleS();
				ExportList[5] = this.HStartList[Index].GetStartPriorityS();
				ExportList[6] = this.HStartList[Index].GetWaitTime().ToString();
            }
			
			return ExportList;
        }

		//Export the metrics history
		public float[] ExpObjMetrics(int Index)
        {
			float[] ExportList = null;

			if (Index >= 0 && Index <= this.GetInitIndexPos())
			{
				ExportList = this.HStartList[Index].GetMetrics();
			}

			return ExportList;
		}

		//Remove application from the start list
		public int RemAppStartList(string ProgramName)
        {
			for(int i = 0; i < GetInitSize(); i++)
            {
				if(this.HStartList[i].GetProgramName().ToLower() == ProgramName.ToLower())
                {
					this.HStartList.RemoveAt(i);
					return 0;
                }
            }

			return -1;
        }

		//Start the object
		public int StartObj(int Index)
        {
			if(Index < this.HStartIndex && Index >= 0)
            {
				int StartStatus = this.HStartList[Index].StartProcess();

				return StartStatus;		//0 for success and -1 if fail.
            }
			else
            {
				return 1;	//Index Error.
            }
        }
	}
}
