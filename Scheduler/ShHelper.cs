using System;
using System.Collections.Generic;
using System.Linq;


public static class ShHelper
{
    public static void Sort(this ref ShProcess_Col Col, EDataType type)
    {
        IEnumerable<ShProcess> temp = null;
        switch (type)
        {
            case EDataType.ArrivalTime:
                temp = Col.shProcesses.OrderBy(proc => proc.info.Arrival);
                break;
            case EDataType.BurstTime:
                temp = Col.shProcesses.OrderBy(proc => proc.info.Burst);
                break;
            case EDataType.Priority:
                temp = Col.shProcesses.OrderBy(proc => proc.info.Priority);
                break;
        }

        Col.shProcesses = temp.ToArray();
    }

    public static IEnumerable<ShProcess> PSort(this IEnumerable<ShProcess> Col, EDataType type)
    {
        IEnumerable<ShProcess> temp = null;

        switch (type)
        {
            case EDataType.ArrivalTime:
                temp = Col.OrderBy(proc => proc.info.Arrival);
                break;
            case EDataType.BurstTime:
                temp = Col.OrderBy(proc => proc.info.Burst);
                break;
            case EDataType.Priority:
                temp = Col.OrderBy(proc => proc.info.Priority);
                break;
        }

        return temp;
    }


    public static int SmallestValueIndex(this IEnumerable<ShProcess> Col, EDataType type)
    {

        var t_list = Col.ToList();
        var t_col = Col;

        switch (type)
        {
            case EDataType.ArrivalTime:
                t_col.PSort(EDataType.ArrivalTime);
                return t_list.FindIndex(x => x.info.Arrival == t_col.First().info.Arrival);
            case EDataType.BurstTime:
                t_col.PSort(EDataType.BurstTime);
                return t_list.FindIndex(x => x.info.Arrival == t_col.First().info.Burst);
            case EDataType.Priority:
                t_col.PSort(EDataType.Priority);
                return t_list.FindIndex(x => x.info.Arrival == t_col.First().info.Priority);
            default:
                return -1;
        }
    }

    public static ShProcess SmallestValueObject(this IEnumerable<ShProcess> Col, EDataType type)
    {

        var t_list = Col.ToList();
        var t_col = Col;

        switch (type)
        {
            case EDataType.ArrivalTime:
                t_col.PSort(EDataType.ArrivalTime);
                return t_list.Find(x => x.info.Arrival == t_col.First().info.Arrival);
            case EDataType.BurstTime:
                t_col.PSort(EDataType.BurstTime);
                return t_list.Find(x => x.info.Arrival == t_col.First().info.Burst);
            case EDataType.Priority:
                t_col.PSort(EDataType.Priority);
                return t_list.Find(x => x.info.Arrival == t_col.First().info.Priority);
            default:
                return new ShProcess();
        }
    }

    public static ShProcess_Col Clone(this ShProcess_Col Col)
    {
        var _cloned = new ShProcess_Col();
        _cloned = Col;
        return _cloned;
    }

    public static void BuildGraphicsData_NP(ref this CompoundResult _result)
    {
        GraphicsData_Col gData_col = new GraphicsData_Col();
        List<GraphicsData> gList = new List<GraphicsData>();
        GraphicsData gData;
        foreach (var p in _result.cmpd_shProcesses.shProcesses)
        {
            gData = new GraphicsData(p.info.Name, p.result.StartTime, p.result.EndTime, new Vector2());
            gList.Add(gData);
        }
        gData_col.graphicsDatas = gList.ToArray().OrderBy(x => x.Start).ToArray();
        _result.cmpd_GetGraphicsData = gData_col;
    }

    public static int GetEndtime(this ShProcess_Col col)
    {
        var t_col = col;
        t_col.Sort(EDataType.ArrivalTime);

        int endTime = 0;

        for(int i = 0;i < t_col.shProcesses.Count(); i++)
        {
            if(i == 0)
            {
                endTime = t_col.shProcesses[i].info.Arrival + t_col.shProcesses[i].info.Burst;
            }
            else
            {
                endTime = t_col.shProcesses[i].info.Arrival <= endTime
                    ? endTime + t_col.shProcesses[i].info.Burst
                    : t_col.shProcesses[i].info.Arrival + t_col.shProcesses[i].info.Burst;
            }
        }

        return endTime;
    }

    public static int GetProcessIndexWith(this IEnumerable<ShProcess> shProcesses, EDataType type, int value)
    {
        var t_list = shProcesses.ToList();
        switch (type)
        {
            case EDataType.ArrivalTime:
                return t_list.FindIndex(x => x.info.Arrival == value && x.ignore == false);
            case EDataType.BurstTime:
                return t_list.FindIndex(x => x.info.Burst == value && x.ignore == false);
            case EDataType.Priority:
                return t_list.FindIndex(x => x.info.Priority == value && x.ignore == false);
            default:
                return -1;
        }
    }

    public static int GetProcessIndexWith(this IEnumerable<ShProcess> shProcesses, ShProcess _proc) => shProcesses.ToList().IndexOf(_proc);

    public static ShProcess GetProcessObjectWith(this IEnumerable<ShProcess> shProcesses, EDataType type, int value)
    {
        var t_list = shProcesses.ToList();
        switch (type)
        {
            case EDataType.ArrivalTime:
                return t_list.Find(x => x.info.Arrival == value && x.ignore == false);
            case EDataType.BurstTime:
                return t_list.Find(x => x.info.Burst == value && x.ignore == false);
            case EDataType.Priority:
                return t_list.Find(x => x.info.Priority == value && x.ignore == false);
        }
        return new ShProcess();
    }


    public static IEnumerable<ShProcess> GetProcessesWith(this IEnumerable<ShProcess> shProcesses, EDataType type, int value)
    {
        switch (type)
        {
            case EDataType.ArrivalTime:
                return shProcesses.Where(z => z.info.Arrival <= value && z.ignore == false);
            case EDataType.BurstTime:
                return shProcesses.Where(z => z.info.Burst <= value && z.ignore == false);
            case EDataType.Priority:
                return shProcesses.Where(z => z.info.Priority <= value && z.ignore == false);
                
        }
        return null;
    }

    public static IEnumerable<int> ToIndex(this IEnumerable<ShProcess> shProcesses,IEnumerable<ShProcess> ReferenceP)
    {
        if (shProcesses.Count() < 1) return null;
        var result = new List<int>();
        var t_arr = ReferenceP.ToList();
        foreach(var p in shProcesses)
        {
            result.Add(t_arr.FindIndex(x => x.info.Name == p.info.Name));
        }
        return result;
    }

    public static IEnumerable<ShProcess> Explicit_GetProcessesWith(this IEnumerable<ShProcess> shProcesses, EDataType type, int value, EDataType type2, int value2)
    {
        switch (type)
        {
            case EDataType.ArrivalTime:
                return shProcesses.Where(z => z.info.Arrival <= value && z.ignore == false).GetProcessesWith(type2,value2);
            case EDataType.BurstTime:
                return shProcesses.Where(z => z.info.Burst <= value && z.ignore == false).GetProcessesWith(type2, value2);
            case EDataType.Priority:
                return shProcesses.Where(z => z.info.Priority <= value && z.ignore == false).GetProcessesWith(type2, value2);

        }
        return null;
    }

    public static ShProcess Explicit_GetProcessWith(this IEnumerable<ShProcess> shProcesses, EDataType type, int value, EDataType type2)
    {
        switch (type)
        {
            case EDataType.ArrivalTime:
                return shProcesses.Where(z => z.info.Arrival <= value && z.ignore == false).SmallestValueObject(type2);
            case EDataType.BurstTime:
                return shProcesses.Where(z => z.info.Burst <= value && z.ignore == false).SmallestValueObject(type2);
            case EDataType.Priority:
                return shProcesses.Where(z => z.info.Priority <= value && z.ignore == false).SmallestValueObject(type2);

        }
        return new ShProcess();
    }

    public static void CleanUp(this ref ShProcess_Col shProcesses)
    {
        var indexes = shProcesses.shProcesses.Where(x => x.info.Burst == 0 && x.ignore == false).ToIndex(shProcesses.shProcesses);
        if (indexes == null) return;
        foreach (var item in indexes)
        {
            shProcesses.shProcesses[item].ignore = true;
        }
    }

    public static GraphicsData_Col EResultToGraphicsData(this ExtendedResult[] _result)
    {
        GraphicsData_Col gData_col = new GraphicsData_Col();
        List<GraphicsData> gList = new List<GraphicsData>();
        GraphicsData gData;
        foreach(var p in _result)
        {
            gData = new GraphicsData(p.name, p.result.StartTime, p.result.EndTime, new Vector2());
            gList.Add(gData);
        }
        gData_col.graphicsDatas = gList.ToArray();

        return gData_col;
    }
}
