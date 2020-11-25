using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    public class ContentControl : ContentView
    {
        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentControl), null, propertyChanged: OnContentTemplateChanged);

        private DataTemplate _previousTemplate;

        private static void OnContentTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var cp = (ContentControl)bindable;

            cp.Content = cp.CreateContent() ?? cp.Content;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            Content = CreateContent() ?? Content;
        }

        private View CreateContent()
        {
            if (BindingContext == null)
            {
                return null;
            }

            DataTemplate template = ContentTemplate;
            while (true)
            {
                DataTemplateSelector selector = template as DataTemplateSelector;
                if (selector == null)
                {
                    break;
                }
                template = selector.SelectTemplate(base.BindingContext, this);
            }
            if (template == _previousTemplate && Content != null)
            {
                return null;
            }
            _previousTemplate = template;
            return (View)(template?.CreateContent());
        }

        /// <summary>
        /// A <see cref="DataTemplate"/> used to render the view. This property is bindable.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

    }
}
