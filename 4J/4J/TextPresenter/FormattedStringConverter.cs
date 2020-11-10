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
    public class FormattedStringConverter : IValueConverter
    {
        public Color TextColor { get; set; }

        public Color HighlightColor { get; set; }

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
            Span span = new Span { Text = run.Text, TextColor = run.IsHighlighted ? HighlightColor : TextColor };

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
