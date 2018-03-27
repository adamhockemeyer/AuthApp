using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using AuthApp.Common.Models;
using AuthApp.Services.Data;


namespace AuthApp.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        TasksDataService _dataService;

        ObservableCollection<TaskItem> _tasks;
        public ObservableCollection<TaskItem> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }

        Command _getTasksCommand;
        public Command GetTasksCommand
        {
            get => _getTasksCommand;
            set => SetProperty(ref _getTasksCommand, value);
        }


        public TasksViewModel(TasksDataService dataService)
        {
            _dataService = dataService;

            Tasks = new ObservableCollection<TaskItem>();

            InitCommands();

            // Load tasks 
            GetTasksCommand.Execute(null);
        }

        void InitCommands()
        {
            GetTasksCommand = new Command(async () =>
            {
                var data = await _dataService.GetTasksAsync();

                Tasks = new ObservableCollection<TaskItem>(data ?? new System.Collections.Generic.List<TaskItem>());
            });
        }
    }
}
