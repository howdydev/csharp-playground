var duration = TimeSpan.FromMinutes(10);
var started = DateTime.UtcNow;

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (sender, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};

try
{
    Console.CursorVisible = false;
    await RunTimerAsync(duration, cts.Token);
}
catch (OperationCanceledException)
{
    Console.Clear();
    var now = DateTime.UtcNow;
    var diff = now - started;
    Console.WriteLine($"Pomodoro timer cancelled after {diff:mm\\:ss}");
}
finally
{
    Console.CursorVisible = true;
}

async Task RunTimerAsync(TimeSpan duration, CancellationToken token)
{
    var endTime = DateTime.UtcNow + duration;

    while (DateTime.UtcNow < endTime) {
        Draw(endTime - DateTime.UtcNow, duration);
        await Task.Delay(1000, token);
    }

    Draw(TimeSpan.Zero, duration);
    Console.Write("\a");
}

static void Draw(TimeSpan remaining, TimeSpan total)
{
    Console.Clear();

    var barWidth = 30;
    var progress = (total - remaining) / total;
    var filled = (int)(barWidth * progress);
    string[] bar = new string[barWidth];

    for (int i = 0; i < barWidth; i++) {
        var cell = i < filled ? "x" : "-";
        bar[i] = cell;
    }

    var timeRemaining = remaining.ToString(@"mm\:ss");
    Console.WriteLine($"Pomodoro timer running ({total:mm}m)");
    Console.WriteLine($"{timeRemaining} remaining");
    Console.Write($"[{string.Join("", bar)}]");
}
