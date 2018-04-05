using System;
using AuthApp.Common.Models;

namespace AuthApp.ViewModels
{
    public class ApprovalDetailViewModel : BaseViewModel
    {
        Approval _approvalDetail;
        public Approval ApprovalDetail
        {
            get => _approvalDetail;
            set => SetProperty(ref _approvalDetail, value);
        }


        public ApprovalDetailViewModel()
        {
        }
    }
}
