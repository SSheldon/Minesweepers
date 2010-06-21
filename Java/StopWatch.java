public class StopWatch
{
    private long elapsedTime;
    private long startTime;
    private boolean isRunning;
    
    public StopWatch()
    {
        Reset();
    }
    
    public void Start()
    {
        if (isRunning) return;
        isRunning = true;
        startTime = System.currentTimeMillis();
    }
    
    public void Stop()
    {
        if (!isRunning) return;
        isRunning = false;
        long endTime = System.currentTimeMillis();
        elapsedTime = elapsedTime + endTime - startTime;
    }
    
    public long GetElapsedTime()
    {
        if (isRunning)
        {
            long endTime = System.currentTimeMillis();
            return elapsedTime + endTime - startTime;
        }
        else
            return elapsedTime;
    }
    
    public void Reset()
    {
        elapsedTime = 0;
        isRunning = false;
    }
}
