using System.ComponentModel;
using Xamarin.Forms;
using _4J.ViewModels;

namespace _4J.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}