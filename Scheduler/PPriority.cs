using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Ppriority
{
    public static CompoundResult S_Ppriority(this ShProcess_Col collection)
    {

        List<GraphicsData> graphicsDatas = new List<GraphicsData>();
        CompoundResult result = default;
        var t_col = collection;
        var duration = t_col.GetEndtime();
        t_col.Sort(EDataType.ArrivalTime);

        ExtendedResult[] extendedResults = new ExtendedResult[256];
        IEnumerable<int> A_list;

        int elements = -1;// -1 empty
        string prevProc = "";
        for (int i = 0; i < duration;)
        {
             A_list = t_col.shProcesses.GetProcessesWith(EDataType.ArrivalTime, i).PSort(EDataType.Priority).ToIndex(t_col.shProcesses);
            if(A_list != null)
            {
                if(prevProc == t_col.shProcesses[A_list.FirstOrDefault()].info.Name)
                {
                    extendedResults[elements] = new ExtendedResult(t_col.shProcesses[A_list.FirstOrDefault()].info.Name,
                    new Result(0, 0, extendedResults[elements].result.StartTime, i = i + 1));
                    t_col.shProcesses[A_list.FirstOrDefault()].info.Burst -= 1;
                }
                else
                {
                    ++elements;
                    extendedResults[elements] = new ExtendedResult(t_col.shProcesses[A_list.FirstOrDefault()].info.Name,
                    new Result(0, 0, i, i = i + 1));
                    t_col.shProcesses[A_list.FirstOrDefault()].info.Burst -= 1;
                }
                prevProc = t_col.shProcesses[A_list.FirstOrDefault()].info.Name;
            }
            else
            {
                i += 1;
            }
            t_col.CleanUp();
        }
        extendedResults = extendedResults.Where(x => x.name != null).ToArray();
        t_col.ExtendedCalculate(extendedResults);
        result.cmpd_shProcesses = t_col;
        result.cmpd_GetGraphicsData = extendedResults.EResultToGraphicsData();
        return result;
    }
}

