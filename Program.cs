using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using TodoistConsole;

// Setup config to get API key
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .Build();

var apiKey = config["ApiKey:TodoistApiKey"];

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Todoist API key is not set.");
    return;
}

// Setup HTTP client
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
client.DefaultRequestHeaders.Accept.Add(mediaType);
client.DefaultRequestHeaders.Add("User-Agent", "My test app");

// Read arguments
if (args.Length == 0)
{
    Console.WriteLine("No arguments passed. Use 'list'.");
    return;
}

var command = args[0];

switch (command)
{
    case "list":
        await ListTasks(client);
        break;
    default:
        Console.WriteLine("Invalid arguments.");
        break;
}

static async Task ListTasks(HttpClient client)
{
    var tasks = await TodoistController.GetInboxTasks(client);

    if (tasks.Count > 0)
        foreach (var task in tasks)
            Console.WriteLine(task.Content);
    else
        Console.WriteLine("No tasks available.");
}
