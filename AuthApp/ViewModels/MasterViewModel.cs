﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AuthApp.Models;
using AuthApp.Pages;
using AuthApp.Services;

namespace AuthApp.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        IAuthenticationService _authService;

        MasterPageItem _selectedItem;
        public MasterPageItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<MasterPageItem> MenuItems { get; set; }

        public MasterViewModel(IAuthenticationService authService)
        {
            _authService = authService;

            _authService.AuthenticationChanged += OnAuthenticationChanged;

            MenuItems = new ObservableCollection<MasterPageItem>();

            MenuItems.Add(new MasterPageItem
            {
                Title = "Home",
                IconName = "resource://AuthApp.Resources.Images.home.svg",
                Active = true,
                TargetType = typeof(HomePage)
            });

            MenuItems.Add(new MasterPageItem
            {
                Title = "Search / History",
                IconName = "resource://AuthApp.Resources.Images.search.svg",
                Active = false,
                TargetType = typeof(SearchPage)
            });

            MenuItems.Add(new MasterPageItem
            {
                Title = "Tasks",
                IconName = "resource://AuthApp.Resources.Images.tasks.svg",
                Active = false,
                TargetType = typeof(TasksPage)
            });

            MenuItems.Add(new MasterPageItem
            {
                Title = "Task Entry",
                IconName = "resource://AuthApp.Resources.Images.pencil_writing.svg",
                Active = false,
                TargetType = typeof(TaskEntryPage)
            });

            MenuItems.Add(new MasterPageItem
            {
                Title = "Report Issue",
                IconName = "resource://AuthApp.Resources.Images.warning.svg",
                Active = false,
                TargetType = typeof(ReportIssuePage)
            });

            MenuItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconName = "resource://AuthApp.Resources.Images.settings.svg",
                Active = false,
                TargetType = typeof(SettingsPage)
            });
        }

        private void OnAuthenticationChanged(string accessToken)
        {

            Task.Run(async () =>
            {
                var profile = await _authService.GetUserProfileAsync();

                if(profile != null)
                {
                    Name = profile.Name;
                }
            });
        }
    }
}
