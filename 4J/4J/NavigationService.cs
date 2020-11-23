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

        public async Task NavigateToAsync(string route, string param1Name, string Param1Value, string param2Name, string Param2Value)
        {
            await Shell.Current.GoToAsync($"{route}?{param1Name}={Param1Value}&{param2Name}={Param2Value}");
        }

        public async Task NavigateToAsync(string route, string param1Name, string Param1Value, string param2Name, string Param2Value, string param3Name, string Param3Value)
        {
            await Shell.Current.GoToAsync($"{route}?{param1Name}={Param1Value}&{param2Name}={Param2Value}&{param3Name}={Param3Value}");
        }
    }
}
