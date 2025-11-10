using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using TodoListApp.Models;

using System.Linq;
using System;
using System.IO;
using System.Text.Json;

namespace TodoListApp;

public partial class MainWindow : Window
{
    private ObservableCollection<TaskItem> _tasks = new();
    private const string _directory = "data";

    public MainWindow()
    {
        InitializeComponent();
        loadTaskFromJson();
        TaskList.ItemsSource = _tasks;

        AddButton.Click += OnAddClick;
        DeleteButton.Click += OnDeleteClick;
        SaveToJson.Click += OnSaveToJsonClick;
        TagFilter.SelectionChanged += OnTagFilterChanged;

        LoadTagsIntoFilter();
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TaskInput.Text))
            return;

        var item = new TaskItem { Title = TaskInput.Text };
        item.SetTagsFromString(TagInput.Text ?? string.Empty);
        _tasks.Add(item);

        TaskInput.Text = string.Empty;
        TagInput.Text = string.Empty;

        LoadTagsIntoFilter();
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        for (int i = _tasks.Count -1; i >= 0; i--)
        {
            if (_tasks[i].IsCompleted == true)
            {
                _tasks.RemoveAt(i);
            }
        }
    }
    
    private void OnSaveToJsonClick(object? sender, RoutedEventArgs e)
    {
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }

        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_tasks, jsonOptions);

        string filePath = Path.Combine(_directory, "tasks.json");
        File.WriteAllText(filePath, json);
    }

    private void loadTaskFromJson()
    {
        string filePath = Path.Combine(_directory, "tasks.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<ObservableCollection<TaskItem>>(json);
            if (tasks != null)
            {
                _tasks = tasks;
                TaskList.ItemsSource = _tasks;
            }
        }
    }

    private void OnTagFilterChanged(object? sender, SelectionChangedEventArgs e)
    {
        string? selectedTag = TagFilter.SelectedItem as string;

        if (string.IsNullOrWhiteSpace(selectedTag) || selectedTag == "All")
        {
            TaskList.ItemsSource = _tasks;
        }
        else
        {
            var filtered = _tasks.Where(t => t.Tags.Contains(selectedTag)).ToList();
            TaskList.ItemsSource = filtered;
        }
    }

    private void LoadTagsIntoFilter()
    {
        var allTags = _tasks.SelectMany(t => t.Tags).Distinct().OrderBy(t => t).ToList();
        allTags.Insert(0, "All");
        TagFilter.ItemsSource = allTags;
        TagFilter.SelectedIndex = 0;
    }
}
