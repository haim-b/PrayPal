using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrayPal
{
    class NavigationService : INavigationService
    {
        public async Task NavigateToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public async Task NavigateToAsync(string route, string paramName, string ParamValue)
        {
            await Shell.Current.GoToAsync($"{route}?{paramName}={ParamValue}");
        }
    }
}
