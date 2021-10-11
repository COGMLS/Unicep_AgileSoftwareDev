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
		StartApp[] HStartList = null;

		//List controls:
		int MaxSimultaneousInit = 0;

		//Constructors:
		public InitializationList(int itens)
		{
			this.HStartList = new StartApp[itens];
		}
		public InitializationList(int itens, int SimInit)
		{
			this.HStartList = new StartApp[itens];
			this.MaxSimultaneousInit = SimInit;
		}

		//Size mng functions
		public int GetInitSize()
		{
			return this.HStartList.Length;
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

		//Load and Save Init List:
		public int LoadInitList(ref CommonTypes.ImportList[] ImportS)
		{
			if(ImportS != null || ImportS.Length <= this.HStartList.Length)
            {
				for(int i = 0; i < this.HStartList.Length; i++)
                {
					this.HStartList[i].SetProgramName("");
                }

				return 0;
            }
			else
            {
				return -1;
            }
		}

		//
	}
}
