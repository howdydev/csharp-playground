using System.Text.Json;

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
        List<PomodoroSession> sessions;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            sessions = JsonSerializer.Deserialize<List<PomodoroSession>>(json)
                ?? [];
        }
        else sessions = [];

        sessions.Add(session);

        string updatedSessions = JsonSerializer.Serialize(sessions);
        File.WriteAllText(path, updatedSessions);
    }

    public void Print()
    {
        // TODO
    }
}
