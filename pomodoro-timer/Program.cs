using var cts = new CancellationTokenSource();

var timer = new PomodoroTimer(cts.Token, TimeSpan.FromMinutes(0.05));

Console.CancelKeyPress += (sender, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};


try
{
    Console.CursorVisible = false;
    await timer.RunTimerAsync();
    timer.EndTimer();
}
catch (OperationCanceledException)
{
    // cancellation is expected, session recorded in finally.
}
finally
{
    Console.CursorVisible = true;
    PomodoroSession session = timer.EndTimer();
    SessionLogger logger = new();
    logger.Log(session);
}
