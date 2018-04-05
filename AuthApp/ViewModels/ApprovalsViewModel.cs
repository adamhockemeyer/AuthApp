using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using AuthApp.Common.Models;
using AuthApp.Services.Data;
using AuthApp.Models;
using System.Linq;
using AuthApp.Services;

namespace AuthApp.ViewModels
{
    public class ApprovalsViewModel : BaseViewModel
    {
        INavigationService _navService;
        ApprovalsDataService _dataService;

        event Action<Approval> OnSelectedItem;

        ObservableCollection<GroupedList<Approval>> _approvals;
        public ObservableCollection<GroupedList<Approval>> Approvals
        {
            get => _approvals;
            set => SetProperty(ref _approvals, value);
        }

        Approval _selectedItem;
        public Approval SelectedItem
        {
            get => _selectedItem;
            set  
            { 
                SetProperty(ref _selectedItem, value);
                if(value != null)
                {
                    OnSelectedItem?.Invoke(value);
                }

            }
        }

        Command _getApprovalsCommand;
        public Command GetApprovalsCommand
        {
            get => _getApprovalsCommand;
            set => SetProperty(ref _getApprovalsCommand, value);
        }


        public ApprovalsViewModel(ApprovalsDataService dataService, INavigationService navService)
        {
            _dataService = dataService;
            _navService = navService;

            Approvals = new ObservableCollection<GroupedList<Approval>>();

            InitCommands();

            // Load approvals 
            GetApprovalsCommand.Execute(null);


            OnSelectedItem += async (Approval selected) => {
                if(selected != null)
                {
                    SelectedItem = null;
                    await navService.NavigateAsync("ApprovalDetailPage", selected);
                }
            };
        }

        void InitCommands()
        {
            GetApprovalsCommand = new Command(async () =>
            {
                var data = await _dataService.GetApprovalsAsync();

                if(data != null && data.Any())
                {
                   var groupedApprovals = data.GroupBy(p => p.Category)
                                              .OrderBy(p => p.Key)
                                              .Select(p => new GroupedList<Approval>(p.ToList()) { Heading = p.Key });

                    Approvals = new ObservableCollection<GroupedList<Approval>>(groupedApprovals);
                }
                else
                {
                    Approvals?.Clear();
                }
            });
        }
    }
}
