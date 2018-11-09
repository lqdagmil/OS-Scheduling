public struct ShProcess
{
    public Info info;
    public Result result;
    public bool ignore;
    public ShProcess(Info info, Result result, bool ignore)
    {
        this.info = info;
        this.result = result;
        this.ignore = ignore;
    }
}

public struct Info
{
    public string Name;
    public int Arrival;
    public int Burst;
    public int Priority;
    
    public Info(string Name, int Arrival, int Burst, int Priority)
    {
        this.Name = Name;
        this.Arrival = Arrival;
        this.Burst = Burst;
        this.Priority = Priority;
    }
}

public struct Result
{
    public int StartTime;
    public int EndTime;
    public int Waitingtime;
    public int TurnaroundTime;

    public Result(int WaitingTime, int TurnArroundTime, int StartTime, int EndTime)
    {
        this.StartTime = StartTime;
        this.EndTime = EndTime;
        this.Waitingtime = WaitingTime;
        this.TurnaroundTime = TurnArroundTime;
    }
}

public struct ShProcess_Col
{
    public ShProcess[] shProcesses;
    public int Count;
    public double Average_WaitingTime;
    public double Average_TurnArounTime;
}

public struct GraphicsData
{
    public string Name;
    public int Start;
    public int End;
    public Vector2 _Vector2;

    public GraphicsData(string name,int start,int end,Vector2 _vector2)
    {
        Name = name;
        Start = start;
        End = end;
        _Vector2 = _vector2;
    }
}

public struct GraphicsData_Col
{
    public GraphicsData[] graphicsDatas;
    public bool UniformColor;
}

public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x,float y)
    {
        this.x = x;
        this.y = y;
    }

}

public struct CompoundResult
{
    public ShProcess_Col cmpd_shProcesses;
    public GraphicsData_Col cmpd_GetGraphicsData;
}

public struct ExtendedResult
{
    public string name;
    public Result result;
    public ExtendedResult(string name,Result result)
    {
        this.name = name;
        this.result = result;
    }
}

public struct Segment
{
    public ExtendedResult[] results;
    public Segment(ExtendedResult[] results)
    {
        this.results = results;
    }
}