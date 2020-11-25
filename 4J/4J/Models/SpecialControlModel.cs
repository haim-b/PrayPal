using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Models
{
    /// <summary>
    /// A model for displaying special controls among the text.
    /// </summary>
    public class SpecialControlModel : ParagraphModel
    {
        public SpecialControlModel()
        { }

        public SpecialControlModel(SpecialContentViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }

        public SpecialContentViewModelBase ViewModel { get; set; }

        public override void Add(string text)
        {
            throw new InvalidOperationException("Cannot add text to a control model.");
        }
    }

    public class SpecialContentViewModelBase : BindableBase
    {
        public double Width { get; set; } = -1d;

        public double Height { get; set; } = -1d;
    }
}
