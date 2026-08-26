public record PomodoroSession(DateTime Started, DateTime Ended, TimeSpan Duration, bool Completed);

public class PomodoroTimer(CancellationToken token, TimeSpan duration)
{
    private readonly static int barWidth = 30;
    private readonly DateTime startTime = DateTime.UtcNow;
    private DateTime endTime;

    public async Task RunTimerAsync()
    {
        endTime = DateTime.UtcNow + duration;

        while (DateTime.UtcNow < endTime)
        {
            Draw(endTime - DateTime.UtcNow, duration);
            await Task.Delay(1000, token);
        }

        // one final draw to show the bar as complete
        Draw(TimeSpan.Zero, duration);
        Console.Write("\a");
        EndTimer();
    }

    static void Draw(TimeSpan remaining, TimeSpan total)
    {
        Console.Clear();

        var progress = (total - remaining) / total;
        var filled = (int)(barWidth * progress);
        string[] bar = new string[barWidth];

        for (int i = 0; i < barWidth; i++)
        {
            var cell = i < filled ? "x" : "-";
            bar[i] = cell;
        }

        Console.WriteLine($"Pomodoro timer running ({total.TotalMinutes}m)");
        Console.WriteLine($"{FormatTime(remaining)} remaining");
        Console.Write($"[{string.Join("", bar)}]");
    }

    public PomodoroSession EndTimer()
    {
        Console.Clear();
        var now = DateTime.UtcNow;
        var totalDuration = now - startTime;
        var session = new PomodoroSession(startTime, now, totalDuration, now >= endTime);
        Console.WriteLine($"Pomodoro timer finished after {FormatTime(totalDuration)}");
        return session;
    }

    public static string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalMinutes}m {time.Seconds:D2}s";
    }
}
