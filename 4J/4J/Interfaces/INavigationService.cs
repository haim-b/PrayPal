using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route);
        Task NavigateToAsync(string route, string paramName, string ParamValue);
        Task NavigateToAsync(string route, string param1Name, string Param1Value, string param2Name, string Param2Value);
        Task NavigateToAsync(string route, string param1Name, string Param1Value, string param2Name, string Param2Value, string param3Name, string Param3Value);
    }
}
