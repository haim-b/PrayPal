using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.PrayPal.Content.LegacyModels
{
    public class ParagraphModel //: INotifyPropertyChanged
    {
        private string _content;
        private string _title;
        //internal static bool _useLargeFont;

        //private static FontFamily _font = new FontFamily(@"\Assets\Fonts\segoeuil.ttf#Segoe UI light");

        static ParagraphModel()
        {
            //if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue("UseLargeFont", out _useLargeFont))
            //{
            //    _useLargeFont = false;
            //}

            //ParagraphModel._fontSize = _useLargeFont ? (double)System.Windows.Application.Current.Resources["PhoneFontSizeLarge"] : (double)System.Windows.Application.Current.Resources["PhoneFontSizeMediumLarge"];
            //ParagraphModel._title2FontSize = _useLargeFont ? (double)App.Current.Resources["PhoneFontSizeMediumLarge"] : (double)App.Current.Resources["PhoneFontSizeNormal"];
        }

        public ParagraphModel(string content)
            : this(content, null)
        { }

        public ParagraphModel(string content, string title)
        {
#if DEBUG
            if (string.IsNullOrEmpty(content) && System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif

            _content = content;
            _title = title;
            IsExpanded = true;
        }

        public virtual string Content
        {
            get
            {
                //if (_defaultStringFormatArgs == null)
                //{
                return _content;
                //}

                //Moadim moed = HebDateHelper.GetCalendarToday();

                //if (moed == Moadim.None)
                //{
                //    return string.Format(_content, _defaultStringFormatArgs);
                //}

                //foreach (KeyValuePair<Moadim, string[]> item in _stringFormatArgsForMoadim)
                //{
                //    if (item.Key == moed)
                //    {
                //        IsBold = true;
                //        return string.Format(_content, item.Value);
                //    }
                //}

                //return string.Format(_content, _defaultStringFormatArgs);
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    //RaisePropertyChanged("Title");
                }
            }
        }

        private string _title2;

        public string Title2
        {
            get { return _title2; }
            set
            {
                if (_title2 != value)
                {
                    _title2 = value;
                    //RaisePropertyChanged();
                }
            }
        }

        public static double _fontSize;
        public static double _title2FontSize;

        public double FontSize
        {
            get { return _fontSize; }
        }

        public double Title2FontSize
        {
            get { return _title2FontSize; }
        }

        public static double Title2FontSizeStatic
        {
            get { return _title2FontSize; }
        }

        //public FontFamily Font
        //{
        //    get { return _font; }
        //}

        public bool IsActive
        {
            get
            {
                //if (IsExpanded != null)
                //{
                //    return !IsExpanded.Value;
                //}

                //if (_activeDates == null)
                {
                    return true;
                }

                //Moadim moed = HebDateHelper.GetCalendarToday();

                //foreach (Moadim date in _activeDates)
                //{
                //    if ((moed & date) == date)
                //    {
                //        return true;
                //    }
                //}

                //return false;
            }
        }

        public bool IsExpanded { get; set; }

        private bool _isCollapsible;

        public bool IsCollapsible
        {
            get { return _isCollapsible; }
            set
            {
                _isCollapsible = value;
                IsExpanded = !_isCollapsible;
            }
        }

        private bool _isBold;

        public bool IsBold
        {
            get { return _isBold; }
            set
            {
                if (_isBold != value)
                {
                    _isBold = value;
                    //RaisePropertyChanged("IsBold");
                }
            }
        }

        //public void SetDefaultStringFormatArgs(params string[] args)
        //{
        //    _defaultStringFormatArgs = args;
        //}

        //public void AddStringFormatArgsForMoadim(Moadim moadim, params string[] args)
        //{
        //    if (_defaultStringFormatArgs == null)
        //    {
        //        throw new InvalidOperationException("SetDefaultStringFormatArgs must be called first.");
        //    }

        //    if (_stringFormatArgsForMoadim == null)
        //    {
        //        _stringFormatArgsForMoadim = new List<KeyValuePair<Moadim, string[]>>();
        //    }

        //    _stringFormatArgsForMoadim.Add(new KeyValuePair<Moadim, string[]>(moadim, args));
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //public virtual void OnLocationCalculated(Geopoint position)
        //{
        //    RaisePropertyChanged("IsActive");

        //    if (_defaultStringFormatArgs != null)
        //    {
        //        RaisePropertyChanged("Content");
        //    }
        //}

        //private FontFamily _fontFamily = new FontFamily("Segoe UI Semilight");

        //public FontFamily FontFamily
        //{
        //    get { return _fontFamily; }
        //    set { _fontFamily = value; }
        //}

    }
}
