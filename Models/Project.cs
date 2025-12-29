using System.Text.Json.Serialization;

public record class Project(
    string Id,
    [property: JsonPropertyName("inbox_project")] bool IsInbox
);