using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal.View
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
            result.BindingContext = _viewModelFactory();
            //(result.BindingContext as IContentPage)?.GenerateContentAsync();
            return result;
        }
    }
}
