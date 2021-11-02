﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStartLib
{
	public class InitializationList
	{
		//Handle to a Initialization list:
		private StartApp[] HStartList = null;

		//Index in use:
		private int HStartIndex;

		//List controls:
		private int MaxSimultaneousInit = 0;	//Reserved for a future update to support multithreading.

		//Constructors:
		public InitializationList(int itens)
		{
			this.HStartList = new StartApp[itens];
			this.HStartIndex = -1;	//No elements in use.
		}
		public InitializationList(int itens, int SimInit)
		{
			this.HStartList = new StartApp[itens];
			this.MaxSimultaneousInit = SimInit;
			this.HStartIndex = -1;	//No elements in use.
		}

		//Size mng functions
		public int GetInitSize()
		{
			return this.HStartList.Length;
		}
		public int GetInitIndexPos()
        {
			return this.HStartIndex;
        }
		public int SetInitSize(int newSize)
		{
			StartApp[] temp = null;

			if (newSize > 0)
			{
				if (newSize > this.HStartList.Length)		//Add more space in the list.
				{
					temp = new StartApp[newSize];

					for (int i = 0; i < this.HStartList.Length; i++)
					{
						temp[i] = this.HStartList[i];
					}

					this.HStartList = temp;

					return 0;
				}
				else if (newSize < this.HStartList.Length)	//Remove space in the list. Any content will be deleted.
				{
					temp = new StartApp[newSize];

					for(int i = 0; i < temp.Length; i++)
					{
						temp[i] = this.HStartList[i];
					}

					this.HStartList = temp;

					return 0;
				}
				else
				{
					return 1;	//Keep size unchanged.
				}
			}
			else
			{
				return -1;		//An invalid number was sended.
			}
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

		//Save Init List:
		public int SaveInitList()
        {
			return 0;
        }

		//Add a object to initialyze:
		public int Add2Init(string programName, string cmdLine, string args, int waitTime = 0)
		{
			if (this.HStartIndex + 1 < this.HStartList.Length)
			{
				this.HStartList[this.HStartIndex + 1].SetProgramName(programName);
				this.HStartList[this.HStartIndex + 1].SetCmdLine(cmdLine);
				this.HStartList[this.HStartIndex + 1].SetArgs(args);
				this.HStartList[this.HStartIndex + 1].SetWaitTime(waitTime);
				this.HStartList[this.HStartIndex + 1].SetWindowStyle(StartWindowStyle.NORMAL);
				this.HStartList[this.HStartIndex + 1].SetStartPriority(CommonTypes.StartPriority.NORMAL);

				this.HStartIndex++;

				return 0;
			}
			else
            {
				return -1;
            }
		}
		public int Add2Init(string programName, string cmdLine, string args, int waitTime, string workingDir)
		{
			if (this.HStartIndex + 1 < this.HStartList.Length)
			{
				this.HStartList[this.HStartIndex + 1].SetProgramName(programName);
				this.HStartList[this.HStartIndex + 1].SetCmdLine(cmdLine);
				this.HStartList[this.HStartIndex + 1].SetArgs(args);
				this.HStartList[this.HStartIndex + 1].SetWaitTime(waitTime);
				this.HStartList[this.HStartIndex + 1].SetWindowStyle(StartWindowStyle.NORMAL);
				this.HStartList[this.HStartIndex + 1].SetStartPriority(CommonTypes.StartPriority.NORMAL);
				this.HStartList[this.HStartIndex + 1].SetWorkingDir(workingDir);

				this.HStartIndex++;

				return 0;
			}
			else
            {
				return -1;
            }
		}
		public int Add2Init(string programName, string cmdLine, string args, int waitTime, string workingDir, CommonTypes.StartPriority priority)
		{
			if (this.HStartIndex + 1 < this.HStartList.Length)
			{
				this.HStartList[this.HStartIndex + 1].SetProgramName(programName);
				this.HStartList[this.HStartIndex + 1].SetCmdLine(cmdLine);
				this.HStartList[this.HStartIndex + 1].SetArgs(args);
				this.HStartList[this.HStartIndex + 1].SetWaitTime(waitTime);
				this.HStartList[this.HStartIndex + 1].SetWindowStyle(StartWindowStyle.NORMAL);
				this.HStartList[this.HStartIndex + 1].SetStartPriority(priority);
				this.HStartList[this.HStartIndex + 1].SetWorkingDir(workingDir);

				this.HStartIndex++;

				return 0;
			}
			else
            {
				return -1;
            }
		}
		public int Add2Init(string programName, string cmdLine, string args, int waitTime, string workingDir, CommonTypes.StartPriority priority, StartWindowStyle windowStyle)
		{
			if (this.HStartIndex + 1 < this.HStartList.Length)
			{
				this.HStartList[this.HStartIndex + 1].SetProgramName(programName);
				this.HStartList[this.HStartIndex + 1].SetCmdLine(cmdLine);
				this.HStartList[this.HStartIndex + 1].SetArgs(args);
				this.HStartList[this.HStartIndex + 1].SetWaitTime(waitTime);
				this.HStartList[this.HStartIndex + 1].SetWindowStyle(windowStyle);
				this.HStartList[this.HStartIndex + 1].SetStartPriority(priority);
				this.HStartList[this.HStartIndex + 1].SetWorkingDir(workingDir);

				this.HStartIndex++;

				return 0;
			}
			else
            {
				return -1;
            }
		}
		public int Add2Init(string programName, string cmdLine, string args, int waitTime, CommonTypes.StartPriority priority, StartWindowStyle windowStyle)
		{
			if (this.HStartIndex + 1 < this.HStartList.Length)
			{
				this.HStartList[this.HStartIndex + 1].SetProgramName(programName);
				this.HStartList[this.HStartIndex + 1].SetCmdLine(cmdLine);
				this.HStartList[this.HStartIndex + 1].SetArgs(args);
				this.HStartList[this.HStartIndex + 1].SetWaitTime(waitTime);
				this.HStartList[this.HStartIndex + 1].SetWindowStyle(windowStyle);
				this.HStartList[this.HStartIndex + 1].SetStartPriority(priority);

				this.HStartIndex++;

				return 0;
			}
			else
			{
				return -1;
			}
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
