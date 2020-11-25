using _4J;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    public class ViewModelTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null)
            {
                return null;
            }

            return App.Current.Resources.Values.OfType<ViewModelTemplate>().FirstOrDefault(dt => dt.ForType == item.GetType())?.Template;
        }
    }
}
