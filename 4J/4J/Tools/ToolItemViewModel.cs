using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Tools
{
    public class ToolItemViewModel : ItemViewModel
    {
        public ToolItemViewModel(string title, string subtitle, Type viewModelType)
            : base(title, subtitle)
        {
            ViewModelType = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));
        }

        public ToolItemViewModel(string title, Type viewModelType)
            : this(title, null, viewModelType)
        { }

        public Type ViewModelType { get; }
    }
}
