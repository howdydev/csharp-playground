using System.Text.Json;
using Spectre.Console;

public class SessionLogger
{
    private readonly string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "pomodoro");
    private readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "pomodoro", "sessions.json");

    public SessionLogger()
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

    public void Log(PomodoroSession session)
    {
        List<PomodoroSession> sessions = GetSessions();
        sessions.Add(session);

        string updatedSessions = JsonSerializer.Serialize(sessions);
        File.WriteAllText(path, updatedSessions);
    }

    public void Print(bool today)
    {
        List<PomodoroSession> loggedSessions = GetSessions();

        var table = new Table()
            .AddColumn("Started")
            .AddColumn("Ended")
            .AddColumn("Duration")
            .AddColumn("Completed");

        var todaysSessions = loggedSessions.Where(s => s.Started.ToLocalTime().Date == DateTime.Today);
        var sessions = today ? todaysSessions : loggedSessions;

        if (!sessions.Any())
        {
            Console.WriteLine("You have no sessions recorded.");
            return;
        }

        foreach (var session in sessions)
            table.AddRow(session.Started.ToString(), session.Ended.ToString(), PomodoroTimer.FormatTime(session.Duration), session.Completed ? "[green]Yes[/]" : "[red]No[/]");

        var durationInSeconds = sessions.Sum(s  => s.Duration.TotalSeconds);
        TimeSpan t = TimeSpan.FromSeconds(durationInSeconds);
        AnsiConsole.WriteLine($"Total duration over sessions: {t.Hours}h {t.Minutes}m {t.Seconds:D2}s");

        AnsiConsole.Write(table);
    }

    private List<PomodoroSession> GetSessions()
    {
        List<PomodoroSession> sessions;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            sessions = JsonSerializer.Deserialize<List<PomodoroSession>>(json)
                ?? [];
        }
        else sessions = [];

        return sessions;
    }
}
