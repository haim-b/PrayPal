using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahadut.Models
{
    public class RunModel
    {
        //private static FontFamily _defaultFont = new FontFamily(@"ms-appx:/Assets/Fonts/segoeuisl.ttf#Segoe UI Semilight");
        // static FontFamily _defaultFont;// new FontFamily(@"Segoe WP SemiLight");
        //private static FontFamily _defaultFont = new FontFamily(@"\Assets\Fonts\segoeuil.ttf#Segoe UI light");
        //private static FontFamily _defaultFont = new FontFamily(@"ms-appx:/Assets/Fonts/david.ttf#David");
        //private static FontFamily _defaultFont = new FontFamily(@"Segoe UI Semilight");

        // FontFamily _font = _defaultFont;
        private double _fontSize;

        static RunModel()
        {
            try
            {
                //_defaultFont = new FontFamily(@"Segoe WP SemiLight");
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
            //_fontSize = Settings.Instance.UseLargeFont ? (double)Windows.UI.Xaml.Application.Current.Resources["TextStyleExtraLargeFontSize"] : (double)Windows.UI.Xaml.Application.Current.Resources["ContentControlFontSize"];
            IsHighlighted = ishighlighted;
            IsBold = isBold;
        }

        public string Text { get; private set; }

        public bool IsHighlighted { get; private set; }

        public bool IsBold { get; private set; }

        //public FontFamily Font
        //{
        //    get { return _font; }
        //    set { _font = value; }
        //}

        public double FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        public bool IsLtr { get; set; }
    }
}
