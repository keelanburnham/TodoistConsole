using System.Net.Http.Json;

namespace TodoistConsole;

public static class TodoistController
{
    private const string API_ENDPOINT = "https://api.todoist.com/api/v1";

    public static async Task<List<TodoTask>> GetInboxTasks(HttpClient client)
    {
        string inboxProjectId = await GetInboxProjectId(client);
        if (string.IsNullOrWhiteSpace(inboxProjectId))
            return Enumerable.Empty<TodoTask>().ToList();
        else
        {
            var repsonse = await client.GetFromJsonAsync<TaskResult>($"{API_ENDPOINT}/tasks?project_id={inboxProjectId}");
            return repsonse?.Results ?? Enumerable.Empty<TodoTask>().ToList(); // if Results is null, the pass empty list
        }
    }

    // method to get the id of the inbox project
    // TODO: try to store this to reduce API calls
    private static async Task<string> GetInboxProjectId(HttpClient client)
    {
        try
        {
            var response = await client.GetFromJsonAsync<ProjectResult>($"{API_ENDPOINT}/projects");

            foreach (var project in response?.Results ?? Enumerable.Empty<Project>())
                if (project.IsInbox)
                    return project.Id;

            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return "";
        }
    }
}
