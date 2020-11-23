using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Zmanim.HebrewCalendar;

namespace PrayPal.Books.Psalms
{
    public class PsalmEntry : Entry
    {
        string _lastValidText = "";
        int _lastValidCaretIndex;
        bool _changingText;

        private static readonly HashSet<string> _validVerses;

        static PsalmEntry()
        {
            HebrewDateFormatter formatter = new HebrewDateFormatter();
            formatter.UseGershGershayim = false;
            formatter.UseEndLetters = false;

            _validVerses = new HashSet<string>();

            for (int i = 1; i <= 150; i++)
            {
                _validVerses.Add(formatter.formatHebrewNumber(i));
            }
        }

        public PsalmEntry()
        {
            Placeholder = "א";
            TextChanged += OnTextChanged;
        }



        //public bool IsFocused
        //{
        //    get { return (bool)GetValue(IsFocusedProperty); }
        //    private set { SetValue(IsFocusedProperty, value); }
        //}

        //public static readonly BindableProperty IsFocusedProperty =
        //    BindableProperty.Create("IsFocused", typeof(bool), typeof(PsalmEntry), false);



        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_changingText)
            {
                return;
            }

            _changingText = true;

            try
            {
                if (!IsTextValid(Text))
                {
                    Text = _lastValidText;
                    CursorPosition = _lastValidCaretIndex;
                }
                else
                {
                    _lastValidText = Text;
                    _lastValidCaretIndex = CursorPosition;
                }
                //_lastValidText = new string(verseText.Text.Where(IsCharValid).ToArray());

            }
            catch (Exception) { }
            finally
            {
                _changingText = false;
            }
        }

        private static bool IsTextValid(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            if (_validVerses.Contains(text))
            {
                return true;
            }

            //if (string.IsNullOrWhiteSpace(text))
            //{
            //    return false;
            //}

            int num;

            if (int.TryParse(text, out num))
            {
                return num > 0 && num <= 150;
            }

            return false;
        }

        //protected override void OnGotFocus(RoutedEventArgs e)
        //{
        //    base.OnGotFocus(e);
        //    IsFocused = true;
        //}

        //protected override void OnLostFocus(RoutedEventArgs e)
        //{
        //    base.OnLostFocus(e);
        //    IsFocused = false;
        //}
    }
}
