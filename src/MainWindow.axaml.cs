using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using TodoListApp.Models;

namespace TodoListApp;

public partial class MainWindow : Window
{
    private ObservableCollection<TaskItem> _tasks = new();

    public MainWindow()
    {
        InitializeComponent();
        TaskList.ItemsSource = _tasks;

        AddButton.Click += OnAddClick;
        DeleteButton.Click += OnDeleteClick;
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
}
