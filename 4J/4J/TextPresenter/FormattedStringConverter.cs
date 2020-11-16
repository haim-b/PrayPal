using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PrayPal.TextPresenter
{
    public class FormattedStringConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FormattedStringConverter));
        public static readonly BindableProperty HighlightColorProperty = BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(FormattedStringConverter));

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color HighlightColor
        {
            get { return (Color)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<RunModel> runs = value as IEnumerable<RunModel>;

            if (runs == null || !runs.Any())
            {
                return null;
            }

            FormattedString result = new FormattedString();

            foreach (RunModel run in runs)
            {
                result.Spans.Add(CreateUISpan(run));
            }

            return result;
        }

        private Span CreateUISpan(RunModel run)
        {
            Span span = new Span { Text = run.Text };

            if (run.IsHighlighted)
            {
                span.SetBinding(Span.TextColorProperty, new Binding(nameof(HighlightColor), source: this));
            }
            else
            {
                span.SetBinding(Span.TextColorProperty, new Binding(nameof(TextColor), source: this));
            }

            if (run.Font != null)
            {
                span.FontFamily = run.Font;
            }

            if (run.FontSize > 0)
            {
                span.FontSize = run.FontSize;
            }

            if (run.IsBold)
            {
                span.FontAttributes |= FontAttributes.Bold;
            }

            return span;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
