using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahadut.Models
{
    public class SpanModel : List<ParagraphModel>
    {
        public SpanModel(string title)
        {
            Title = title;
        }

        public SpanModel(string title, params string[] paragraphTexts)
        {
            Title = title;
            Add(paragraphTexts);
        }

        public string Title { get; private set; }

        public string ShortTitle { get; set; }

        public void Add(params string[] paragraphsText)
        {
            foreach (string t in paragraphsText)
            {
                Add(new ParagraphModel(t));
            }
        }
    }
}
