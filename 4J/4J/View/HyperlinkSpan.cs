﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PrayPal
{
    public class HyperlinkSpan : Span
    {
        public static readonly BindableProperty UrlProperty =
            BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public HyperlinkSpan()
        {
            TextDecorations = TextDecorations.Underline; ;
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                // Launcher.OpenAsync is provided by Xamarin.Essentials.
                Command = new Command(async () => await Launcher.OpenAsync(Url))
            });
        }
    }
}
