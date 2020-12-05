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

        public ItemViewModel(string title)
            : this(title, null)
        { }

        public ItemViewModel(string title, string subtitle)
        {
            _title = title ?? throw new ArgumentNullException(nameof(title));
            _subtitle = subtitle;
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
    }
}
