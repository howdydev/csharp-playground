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

    public void Print()
    {
        List<PomodoroSession> sessions = GetSessions();
        if (sessions.Count == 0)
        {
            Console.WriteLine("You have no sessions recorded.");
            return;
        }

        var table = new Table()
            .AddColumn("Started")
            .AddColumn("Ended")
            .AddColumn("Duration")
            .AddColumn("Completed");

        foreach (var session in sessions)
            table.AddRow(session.Started.ToString(), session.Ended.ToString(), PomodoroTimer.FormatTime(session.Duration), session.Completed ? "[green]Yes[/]" : "[red]No[/]");

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
