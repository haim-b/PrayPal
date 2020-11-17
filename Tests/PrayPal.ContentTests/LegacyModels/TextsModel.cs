using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.PrayPal.Content.LegacyModels
{
    public class TextsModel : List<ParagraphModel>
    {
        //public TextsModel(string title, string content)
        //{
        //    Add(new ParagraphModel(content));
        //    Title = title;
        //}

        public TextsModel(string title, params string[] paragraphsText)
        {
            Title = title;
            AddRange(paragraphsText);
        }

        public TextsModel(ParagraphModel p)
        {
            AddParagraph(p);
            Title = p.Title;
        }

        public TextsModel(params ParagraphModel[] p)
        {
            AddRange(p);

            if (p.Length > 0)
            {
                Title = p[0].Title;
            }
        }

        public void Add(string paragraphText)
        {
            AddParagraph(new ParagraphModel(paragraphText));
        }

        public void AddRange(params string[] paragraphsText)
        {
            foreach (string t in paragraphsText)
            {
                AddParagraph(new ParagraphModel(t));
            }
        }

        public void AddRange(params ParagraphModel[] paragraphsText)
        {
            foreach (ParagraphModel p in paragraphsText)
            {
                AddParagraph(p);
            }
        }

        public new void Add(ParagraphModel p)
        {
            AddParagraph(p);
        }

        private const int NormalFontSplittingThreshold = 3000;
        private const int LargeFontSplittingThreshold = 2000;

        private void AddParagraph(ParagraphModel p)
        {
            //if (p.Content != null
            //    && ((ParagraphModel._useLargeFont && p.Content.Length > LargeFontSplittingThreshold)
            //    || (!ParagraphModel._useLargeFont && p.Content.Length > NormalFontSplittingThreshold)))
            //{
            //    string text = p.Content;

            //    while (text.Length > 0)
            //    {
            //        int periodIndex = text.Length >= LargeFontSplittingThreshold ? text.IndexOf('.', LargeFontSplittingThreshold) + 1 : text.Length;

            //        if (periodIndex == 0)
            //        {
            //            periodIndex = text.Length;
            //        }

            //        base.Add(new ParagraphModel(text.Substring(0, periodIndex), p.Title));

            //        if (periodIndex <= text.Length)
            //        {
            //            text = text.Substring(periodIndex).TrimStart();
            //        }
            //    }
            //}
            //else
            {
                base.Add(p);
            }
        }

        public TextsModel()
        {

        }

        public string Title { get; set; }

        internal TextsModel Clone()
        {
            TextsModel clone = new TextsModel();
            clone.Title = this.Title;
            clone.AddRange(this);
            return clone;
        }
    }
}
