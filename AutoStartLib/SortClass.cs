namespace AutoStartLib
{
	static class SortClass
	{
		//Sort function to order by Priority and Wait Time.
		public static void Sort(ref StartApp[] startArr)
		{
			int start = 0;
			int end = startArr.Length - 1;

			QuickSort(ref startArr, start, end);
		}

		//Quick sort algorithm
		private static void Swap(ref StartApp a, ref StartApp b)
		{
			StartApp temp = null;

			temp = a;
			a = b;
			b = temp;
		}
		private static int Partition(ref StartApp[] startArrParted, int start, int end)
		{
			int pivot = ((int)startArrParted[end].GetStartPriority());
			int pivot2 = startArrParted[end].GetWaitTime();
			int i = (start - 1);

			for(int j = start; j <= end - 1; j++)
			{
				if(((int)startArrParted[j].GetStartPriority()) > pivot && startArrParted[j].GetWaitTime() < pivot2)
				{
					i++;
					Swap(ref startArrParted[i], ref startArrParted[j]);
				}
				else if(((int)startArrParted[j].GetStartPriority()) == pivot && startArrParted[j].GetWaitTime() < pivot2)
				{
					i++;
					Swap(ref startArrParted[i], ref startArrParted[j]);
				}
			}

			Swap(ref startArrParted[i + 1], ref startArrParted[end]);

			return (i + 1);
		}
		private static void QuickSort(ref StartApp[] startArrParted, int start, int end)
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
