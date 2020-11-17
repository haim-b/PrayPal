using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Content
{
    public abstract class SpansPrayerBase : PrayerBase<SpanModel>
    {
        public SpansPrayerBase()
        {
            HasGroups = true;
        }

        protected override void AddStringFormat(string text, string argument, bool isHighlighted = false, bool isBold = false, string title = null, bool isCollapsible = false)
        {
            ParagraphModel p = PrayersHelper.CreateParagraphForStringFormat(text, argument, isHighlighted, isBold, null, false);

            if (p != null)
            {
                SpanModel span = new SpanModel(title);// { IsCollapsible = isCollapsible });

                span.Add(p);

                _items.Add(span);
            }
        }

        protected override void Add(string text, string title = null, bool isCollapsible = false)
        {
            _items.Add(new SpanModel(title, text));// { IsCollapsible = isCollapsible });
        }

        protected void Add(string title, params string[] texts)
        {
            if (texts == null || texts.Length == 0)
            {
                throw texts == null ? new ArgumentNullException("texts") : new ArgumentException("At least one text is required.", "texts");
            }

            _items.Add(new SpanModel(title, texts));
        }

        protected void Add(string title, params ParagraphModel[] texts)
        {
            if (texts == null || texts.Length == 0)
            {
                throw texts == null ? new ArgumentNullException("texts") : new ArgumentException("At least one text is required.", "texts");
            }

            Add(title, (IEnumerable<ParagraphModel>)texts);
        }

        protected void Add(string title, IEnumerable<ParagraphModel> texts)
        {
            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            SpanModel span = new SpanModel(title);
            span.AddRange(texts);

            _items.Add(span);
        }

        protected override ParagraphModel GetItemAtIndexImpl(int index)
        {
            return _items.SelectMany(s => s).ElementAt(index);
        }

        public override int GetItemsCount()
        {
            return _items.SelectMany(s => s).Count();
        }
    }
}
