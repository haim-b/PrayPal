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
        }

        public async void OnGroupClicked(object sender, EventArgs e)
        {
            ListView jumpList = new ListView(ListViewCachingStrategy.RecycleElement);
            jumpList.BindingContext = this.BindingContext;
            jumpList.Style = (Style)Resources["JumpListStyle"];

            PopupPage page = new PopupPage { Content = jumpList };
            page.CloseWhenBackgroundIsClicked = true;

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

                lst.ScrollTo(group.FirstOrDefault(), group, ScrollToPosition.Start, true);
            }
            catch { }
        }
    }
}
