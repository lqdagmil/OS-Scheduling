public static class FCFS
{
    public static CompoundResult S_FCFS(this ShProcess_Col Collection)
    {
        CompoundResult result = new CompoundResult();
        var t_col = Collection;
        var duration = t_col.GetEndtime();
        t_col.Sort(EDataType.ArrivalTime);

        for (int i = 0;i< duration;)
        {
            var A_List = t_col.shProcesses.GetProcessesWith(EDataType.ArrivalTime, i).ToIndex(t_col.shProcesses);
            if (A_List != null)
            {
                foreach (var indx in A_List)
                {
                    t_col.shProcesses[indx].result.StartTime = i;
                    i = t_col.shProcesses[indx].result.EndTime = i + t_col.shProcesses[indx].info.Burst;
                    t_col.shProcesses[indx].ignore = true;
                }
            }
            else
            {
                i += 1;
            }
        }

        t_col.Calculate();
        result.cmpd_shProcesses = t_col;
        result.BuildGraphicsData_NP();
        return result;
    }
}
