using System;
using System.Collections.Generic;
using AuthApp.Common.Models;
using AuthApp.ViewModels;
using Xamarin.Forms;

namespace AuthApp.Pages
{
    public partial class ApprovalDetailPage : BaseContentPage<ApprovalDetailViewModel>
    {
        public ApprovalDetailPage()
        {
            InitializeComponent();
        }

        public ApprovalDetailPage(Approval approval) : this()
        {
            ViewModel.ApprovalDetail = approval;
        }
    }
}
