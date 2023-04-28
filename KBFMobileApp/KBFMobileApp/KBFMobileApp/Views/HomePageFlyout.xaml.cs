using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBFMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageFlyout : ContentPage
    {
        public ListView ListView;

        public HomePageFlyout()
        {
            InitializeComponent();

            BindingContext = new HomePageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class HomePageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomePageFlyoutMenuItem> MenuItems { get; set; }

            public HomePageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<HomePageFlyoutMenuItem>(new[]
                {
                    new HomePageFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new HomePageFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new HomePageFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new HomePageFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new HomePageFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}