using System.Collections.Generic;

namespace AutoStartLib
{
	static class SortClass
	{
		//Sort function to order by Priority and Wait Time.
		public static void Sort(ref List<StartApp> startArr)
		{
			int start = 0;
			int end = startArr.Count - 1;

			QuickSort(ref startArr, start, end);
		}

		//Quick sort algorithm
		private static void Swap(ref List<StartApp> List,  ref int a, ref int b)
		{
			StartApp temp = null;

			temp = List[a];
			List[a] = List[b];
			List[b] = temp;
		}
		private static int Partition(ref List<StartApp> startArrParted, int start, int end)
		{
			int pivot = ((int)startArrParted[end].GetStartPriority());
			int pivot2 = startArrParted[end].GetWaitTime();
			int i = (start - 1);

			for(int j = start; j <= end - 1; j++)
			{
				if(((int)startArrParted[j].GetStartPriority()) > pivot && startArrParted[j].GetWaitTime() < pivot2)
				{
					i++;
					Swap(ref startArrParted, ref i, ref j);
				}
				else if(((int)startArrParted[j].GetStartPriority()) == pivot && startArrParted[j].GetWaitTime() < pivot2)
				{
					i++;
					Swap(ref startArrParted, ref i, ref j);
				}
			}

			int t = i + 1;

			Swap(ref startArrParted, ref t, ref end);

			return (i + 1);
		}
		private static void QuickSort(ref List<StartApp> startArrParted, int start, int end)
		{
			if(start < end)
			{
				int PartI = Partition(ref startArrParted, start, end);

				QuickSort(ref startArrParted, start, PartI - 1);
				QuickSort(ref startArrParted, PartI + 1, end);
			}
		}
	}
}
