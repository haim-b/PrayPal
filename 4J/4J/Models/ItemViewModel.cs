using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Models
{
    public class ItemViewModel : BindableBase
    {
        private string _title;
        private string _subtitle;
        private readonly string _pageName;
        private readonly object _pageParameter;

        public ItemViewModel(string pageName, string title, object pageParameter = null)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                throw new ArgumentException($"'{nameof(pageName)}' cannot be null or whitespace", nameof(pageName));
            }

            _pageName = pageName;
            _title = title ?? throw new ArgumentNullException(nameof(title));
            _pageParameter = pageParameter;
        }

        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set
            {
                SetProperty(ref _subtitle, value);
            }
        }

        public string PageName
        {
            get { return _pageName; }
        }

        public object PageParameter
        {
            get { return _pageParameter; }
        }
    }
}
