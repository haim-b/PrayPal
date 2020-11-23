using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    public class LocalRouteFactory : RouteFactory
    {
        private readonly Func<Element> _viewFactory;
        private readonly Func<object> _viewModelFactory;

        public LocalRouteFactory(Func<Element> viewFactory, Func<object> viewModelFactory)
        {
            _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
        }

        public override Element GetOrCreate()
        {
            Element result = _viewFactory();
            object viewModel = _viewModelFactory();

            result.BindingContext = viewModel;

            if (result is Page p && viewModel is IContentPage cp)
            {
                p.Appearing += (s, e) => cp.GenerateContentAsync();
            }

            return result;
        }
    }
}
