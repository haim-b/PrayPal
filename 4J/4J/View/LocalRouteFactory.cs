using System;
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

            if (viewModel != null && viewModel.GetType().GetCustomAttribute<QueryPropertyAttribute>() == null)
            {
                // If the page has no query parameters, we can generate its content immediately:
                (viewModel as IContentPage)?.GenerateContentAsync();
            }

            result.BindingContext = viewModel;

            return result;
        }
    }
}
