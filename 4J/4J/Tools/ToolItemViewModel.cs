using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Tools
{
    public class ToolItemViewModel : ItemViewModel
    {
        public ToolItemViewModel(string name, string title, string subtitle, Type viewModelType)
            : base(title, subtitle)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Name = name;
            ViewModelType = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));
        }

        public ToolItemViewModel(string name, string title, Type viewModelType)
            : this(name, title, null, viewModelType)
        { }

        public string Name { get; }

        public Type ViewModelType { get; }
    }
}
