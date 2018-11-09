using System.Linq;
using System.Collections.Generic;
using System;

public static class ShMath
{
    public static void Calculate(this ref ShProcess_Col Col)
    {
        Col.Count = Col.shProcesses.Count();
        for(int i = 0; i < Col.Count; i++)
        {
            Col.shProcesses[i].result.Waitingtime = Col.shProcesses[i].result.StartTime - Col.shProcesses[i].info.Arrival;
            Col.shProcesses[i].result.TurnaroundTime = Col.shProcesses[i].result.EndTime - Col.shProcesses[i].info.Arrival;
        }

        
        Col.Average_TurnArounTime = Math.Round(Col.shProcesses.Average(ar => ar.result.TurnaroundTime),2);
        Col.Average_WaitingTime = Math.Round (Col.shProcesses.Average(ar => ar.result.Waitingtime),2);
    }



    public static void Calculate(this ref ShProcess Col, ECalculate eCalculate)
    {
        
        switch (eCalculate)
        {
            case ECalculate.WaitingTime:
                Col.result.Waitingtime = Col.result.StartTime - Col.info.Arrival;
                break;
            case ECalculate.TurnAroundTime:
                Col.result.TurnaroundTime = Col.result.EndTime - Col.info.Arrival;
                break;
        }
    }

    public static void ExtendedCalculate(this ref ShProcess_Col Col, ExtendedResult[] extendedResult)
    {
        Segment _s;
        for (int i = 0; i < Col.shProcesses.Count(); i++)
        {
            _s = new Segment(extendedResult.GetDataByName(Col.shProcesses[i].info.Name));
            if(_s.results.Count() > 1)
            {
                int calc = _s.results[0].result.StartTime - Col.shProcesses[i].info.Arrival;
                for (int p = 0; p < _s.results.Count()-1; p++)
                {
                    calc += (_s.results[p + 1].result.StartTime - _s.results[p].result.EndTime);
                }
                Col.shProcesses[i].result.Waitingtime = calc;
                Col.shProcesses[i].result.TurnaroundTime = _s.results.LastOrDefault().result.EndTime - Col.shProcesses[i].info.Arrival;
            }
            else if(_s.results.Count() == 1)
            {
                Col.shProcesses[i].result.Waitingtime = _s.results[0].result.StartTime - Col.shProcesses[i].info.Arrival;
                Col.shProcesses[i].result.TurnaroundTime = _s.results[0].result.EndTime - Col.shProcesses[i].info.Arrival;
            }
        }
        Col.Average_TurnArounTime = Math.Round(Col.shProcesses.Average(ar => ar.result.TurnaroundTime), 2);
        Col.Average_WaitingTime = Math.Round(Col.shProcesses.Average(ar => ar.result.Waitingtime), 2);
    }


    #region Helper Functions Exclusive for ShMath

    private static ExtendedResult[] GetDataByName(this IEnumerable<ExtendedResult> extendedResult, string name) => extendedResult.Where(x => x.name == name).ToArray();



    #endregion
}

