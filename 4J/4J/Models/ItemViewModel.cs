using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahadut.Models
{
    public class ItemViewModel : BindableBase
    {
        private string _title;
        private string _subtitle;
        private readonly string _pageName;
        private readonly object _pageParameter;

        public ItemViewModel(string title, string pageName, object pageParameter = null)
            : this(pageName, pageParameter)
        {
            _title = title;
        }

        public ItemViewModel(string pageName, object pageParameter = null)
        {
            _pageName = pageName;
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
