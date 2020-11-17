using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Models
{
    public class ParagraphModel : List<RunModel>, ITextContainer
    {
        private static double _fontSize;
        private static double _titleFontSize;
        private bool _isCollapsible;

        static ParagraphModel()
        {
            SetFontSize();
        }

        public ParagraphModel(string content)
            : this(null, content)
        { }

        public ParagraphModel(string title, string content)
            : this(title, new RunModel[] { new RunModel(content) })
        { }

        public ParagraphModel(string title, params RunModel[] runs)
        {
            //_runs = new List<RunModel>(runs);
            this.AddRange(runs);
            Title = title;
            IsExpanded = true;
            //_titleFontSize = GetTitleSize();

        }

        internal static void SetFontSize()
        {
            try
            {
                _fontSize = Utils.GetFontSize(Settings.UseLargeFont);
                _titleFontSize = Utils.GetTitleFontSize(Settings.UseLargeFont);
            }
            catch
            {
                _fontSize = 18;
                _titleFontSize = 18;
            }
        }


        //public static double GetTitleSize()
        //{
        //    return Settings.Instance.UseLargeFont ? (double)Windows.UI.Xaml.Application.Current.Resources["TextStyleExtraLargeFontSize"] : (double)Windows.UI.Xaml.Application.Current.Resources["TextStyleMediumFontSize"];
        //}

        public virtual IEnumerable<RunModel> Content
        {
            get { return this; }//_runs; }
        }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public double FontSize
        {
            get { return _fontSize; }
        }

        public double TitleFontSize
        {
            get { return _titleFontSize; }
        }

        public bool IsExpanded { get; set; }

        public bool IsCollapsible
        {
            get { return _isCollapsible; }
            set
            {
                _isCollapsible = value;
                IsExpanded = !_isCollapsible;
            }
        }

        public bool IsLtr { get; set; }

        //public void Add(RunModel run)
        //{
        //    if (run == null)
        //    {
        //        throw new ArgumentNullException("run");
        //    }

        //    this.Add(run);
        //}

        public void Add(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.Add(new RunModel(text));
        }
    }
}
