using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using AuthApp.ViewModels;


namespace AuthApp.Pages
{
    public partial class MasterPage : BaseContentPage<MasterViewModel>
    {
        public ListView ListView { get { return listView; } }

        public MasterPage()
        {
            InitializeComponent();
        }

        public void UnsetActive()
        {
            foreach (var active in ViewModel.MenuItems.Where(p => p.Active))
            {
                active.Active = false;
            }
        }
    }
}
