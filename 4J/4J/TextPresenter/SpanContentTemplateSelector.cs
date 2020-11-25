using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal.TextPresenter
{
    public class SpanContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ParagraphTemplate { get; set; }

        public DataTemplate SpecialContentTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is SpecialControlModel scm)
            {
                return SpecialContentTemplate;
            }
            else if (item is ParagraphModel)
            {
                return ParagraphTemplate;
            }

            return null;
        }
    }
}
