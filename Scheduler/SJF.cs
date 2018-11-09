public static class SJF
{
    public static CompoundResult S_SJF(this ShProcess_Col Collection)
    {
        CompoundResult c_result = default;
        var t_col = Collection;
        var duration = t_col.GetEndtime();
        t_col.Sort(EDataType.ArrivalTime);

        for (int i = 0; i <= duration;)
        {
            var A_list = t_col.shProcesses.GetProcessesWith(EDataType.ArrivalTime, i).PSort(EDataType.BurstTime).ToIndex(t_col.shProcesses);
            if (A_list != null)
            {
                foreach (var indx in A_list)
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
        c_result.cmpd_shProcesses = t_col;
        c_result.BuildGraphicsData_NP();
        return c_result;
    }
}
