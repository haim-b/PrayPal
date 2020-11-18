using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Models;

namespace PrayPal.Content
{
    public abstract class ParagraphsPrayerBase : PrayerBase<ParagraphModel>
    {
        public ParagraphsPrayerBase()
        {
            HasGroups = false;
        }

        protected override void AddStringFormat(string text, string argument, bool isHighlighted = false, bool isBold = false, string title = null, bool isCollapsible = false)
        {
            ParagraphModel p = PrayersHelper.CreateParagraphForStringFormat(text, argument, isHighlighted, isBold, title, isCollapsible);

            if (p != null)
            {
                _items.Add(p);
            }
        }

        protected override void Add(string text, string title = null, bool isCollapsible = false)
        {
            _items.Add(new ParagraphModel(title, text) { IsCollapsible = isCollapsible });
        }

        protected override void Add(ParagraphModel paragraph)
        {
            if (paragraph is null)
            {
                throw new ArgumentNullException(nameof(paragraph));
            }

            _items.Add(paragraph);
        }

        protected override ParagraphModel GetItemAtIndexImpl(int index)
        {
            return _items.ElementAt(index);
        }

        public override int GetItemsCount()
        {
            return _items.Count;
        }
    }
}
