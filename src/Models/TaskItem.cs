namespace TodoListApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public List<string> Tags { get; set; } = new();

    public void SetTagsFromString(string tagString)
    {
        if (string.IsNullOrWhiteSpace(tagString))
        {
            Tags.Clear();
            return;
        }

        Tags = tagString
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToLowerInvariant())
            .Distinct()
            .ToList();
    }

    public string TagsDisplay => string.Join(", ", Tags);
}
