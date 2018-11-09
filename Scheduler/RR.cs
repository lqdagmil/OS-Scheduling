using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class RR
{
    public static CompoundResult S_RR(this ShProcess_Col collection,int Quantum)
    {
        List<GraphicsData> graphicsDatas = new List<GraphicsData>();
        CompoundResult result = default;
        var t_col = collection;
        var duration = t_col.GetEndtime();
        t_col.Sort(EDataType.ArrivalTime);

        ExtendedResult[] extendedResults = new ExtendedResult[256];
        IEnumerable<int> A_list;

        int elements = -1;// -1 empty

        Queue<int> indexes = new Queue<int>();

        for (int i = 0; i < duration;)
        {
             A_list = t_col.shProcesses.GetProcessesWith(EDataType.ArrivalTime, i).ToIndex(t_col.shProcesses);
            if (A_list != null)
            {
                foreach (var v in A_list)
                {
                    if (!indexes.Contains(v))
                    {
                        indexes.Enqueue(v);
                    }
                }
                   
            
            if (indexes != null)
            {
                    ++elements;
                    if (t_col.shProcesses[indexes.FirstOrDefault()].info.Burst >= Quantum)
                    {
                        extendedResults[elements] = new ExtendedResult(t_col.shProcesses[indexes.FirstOrDefault()].info.Name,
                            new Result(0, 0, i, i = i + 1));
                        t_col.shProcesses[indexes.FirstOrDefault()].info.Burst -= 1;

                        for (int y = 1; y < Quantum; y++)
                        {
                            extendedResults[elements] = new ExtendedResult(t_col.shProcesses[indexes.FirstOrDefault()].info.Name,
                            new Result(0, 0, extendedResults[elements].result.StartTime, i = i + y));
                            t_col.shProcesses[indexes.FirstOrDefault()].info.Burst -= 1;
                        }

                        if(indexes.Count() == 1)
                        {
                            t_col.CleanUp();
                            A_list = t_col.shProcesses.GetProcessesWith(EDataType.ArrivalTime, i).ToIndex(t_col.shProcesses);
                            if(A_list != null)
                            {
                                foreach (var v in A_list)
                                {
                                    if (!indexes.Contains(v))
                                    {
                                        indexes.Enqueue(v);
                                    }
                                }
                                indexes.Enqueue(indexes.Dequeue());
                            }
                            else
                            {
                                indexes.Dequeue();
                            }
                            
                        }
                        else if(t_col.shProcesses[indexes.FirstOrDefault()].info.Burst != 0)
                        {
                            A_list = t_col.shProcesses.GetProcessesWith(EDataType.ArrivalTime, i).ToIndex(t_col.shProcesses);
                            if (A_list != null)
                            {
                                foreach (var v in A_list)
                                {
                                    if (!indexes.Contains(v))
                                    {
                                        indexes.Enqueue(v);
                                    }
                                }
                                indexes.Enqueue(indexes.Dequeue());
                            }
                        }
                        else if (t_col.shProcesses[indexes.FirstOrDefault()].info.Burst == 0)
                        {
                            indexes.Dequeue();
                        }
                        t_col.CleanUp();
                    }
                    else
                    {
                        extendedResults[elements] = new ExtendedResult(t_col.shProcesses[indexes.FirstOrDefault()].info.Name,
                        new Result(0, 0, i, i = i + 1));
                        t_col.shProcesses[indexes.FirstOrDefault()].info.Burst -= 1;
                        indexes.Dequeue();
                    }
                }
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

