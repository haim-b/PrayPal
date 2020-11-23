using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.TextPresenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ViewFor(typeof(TextPresenterViewModel))]
    public partial class TextPresenterView : ContentPage
    {
        public TextPresenterView()
        {
            InitializeComponent();

            lst.ItemsSourceChanged += OnItemsSourceChanged;

            if (Device.RuntimePlatform != Device.Android)
            {
                GroupHeaderView.RemoveBinding(ContentView.IsVisibleProperty);
                GroupHeaderView.IsVisible = false;
            }
        }

        public async void OnGroupClicked(object sender, EventArgs e)
        {
            //ListView jumpList = new ListView(ListViewCachingStrategy.RecycleElement);
            //jumpList.BindingContext = this.BindingContext;
            //jumpList.Style = (Style)Resources["JumpListStyle"];

            PopupPage page = new PopupPage();// { Content = jumpList };
            page.Style = (Style)Resources["PopupPageStype"];

            await PopupNavigation.Instance.PushAsync(page);
        }

        public async void OnJumpListItemClicked(object sender, EventArgs e)
        {
            IEnumerable<object> group = ((Element)sender).BindingContext as IEnumerable<object>;

            if (group == null)
            {
                return;
            }

            try
            {
                await PopupNavigation.Instance.PopAllAsync();

                lst.ScrollTo(group.FirstOrDefault(), group, ScrollToPosition.Start, false);
            }
            catch { }
        }

        private static async void OnItemsSourceChanged(object sender, EventArgs e)
        {
            ExtendedListView lst = (ExtendedListView)sender;
            TextPresenterViewModel vm = lst.BindingContext as TextPresenterViewModel;

            if (vm?.StartFromParagraphIndex > 0)
            {
                // Wait a bit for the content to load:
                await Task.Delay(1500);

                object item = null; ;

                if (vm.TextDocument.HasGroups)
                {
                    item = lst.ItemsSource.Cast<IEnumerable<object>>().SelectMany(a => a).ElementAtOrDefault(vm.StartFromParagraphIndex);
                }
                else
                {
                    item = lst.ItemsSource.Cast<object>().FirstOrDefault();
                }

                if (item != null)
                {
                    lst.ScrollTo(item, /*group,*/ ScrollToPosition.Start, false); // We don't use animation because when it's on we can't scroll after the 70th item.
                }
            }
        }
    }
}
