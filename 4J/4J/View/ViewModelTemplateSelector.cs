using _4J;
using System;
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

            if (App.Current.Resources.TryGetValue(item.GetType().Name, out var template))
            {
                return (DataTemplate)template;
            }

            return null;
        }
    }
}
