using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal.TextPresenter
{
    public class CollapsibleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularTemplate { get; set; }

        public DataTemplate CollapsibleTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ParagraphModel paragraph = item as ParagraphModel;

            if (paragraph == null || !paragraph.IsCollapsible)
            {
                return RegularTemplate;
            }

            return CollapsibleTemplate;
        }
    }
}
