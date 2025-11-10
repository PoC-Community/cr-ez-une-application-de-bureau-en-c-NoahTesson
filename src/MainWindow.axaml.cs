using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using TodoListApp.Models;

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
        TaskList.ItemsSource = _tasks;

        AddButton.Click += OnAddClick;
        DeleteButton.Click += OnDeleteClick;
        SaveToJson.Click += OnSaveToJsonClick;
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskInput.Text))
        {
            var item = new TaskItem { Title = TaskInput.Text, IsCompleted = false };

            _tasks.Add(item);
            TaskInput.Text = string.Empty;
        }
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
}
