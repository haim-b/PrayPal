using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Models
{
    public class RunModel
    {
        private static readonly string _defaultFont;
        //private static FontFamily _defaultFont = new FontFamily(@"ms-appx:/Assets/Fonts/segoeuisl.ttf#Segoe UI Semilight");
        // static FontFamily _defaultFont;// new FontFamily(@"Segoe WP SemiLight");
        //private static FontFamily _defaultFont = new FontFamily(@"\Assets\Fonts\segoeuil.ttf#Segoe UI light");
        //private static FontFamily _defaultFont = new FontFamily(@"ms-appx:/Assets/Fonts/david.ttf#David");
        //private static FontFamily _defaultFont = new FontFamily(@"Segoe UI Semilight");

        string _font = _defaultFont;
        private double _fontSize;

        static RunModel()
        {
            try
            {
                _defaultFont = "SegoeUISL";// new FontFamily(@"Segoe WP SemiLight");
            }
            catch
            { }
        }

        public RunModel(string text, bool ishighlighted = false, bool isBold = false)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("text");
            }

            Text = text;
            IsHighlighted = ishighlighted;
            IsBold = isBold;

            try
            {
                _fontSize = Utils.GetFontSize(Settings.UseLargeFont);
            }
            catch
            {
                _fontSize = 18;
            }
        }

        public string Text { get; private set; }

        public bool IsHighlighted { get; private set; }

        public bool IsBold { get; private set; }

        public string Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public double FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
    }
}
