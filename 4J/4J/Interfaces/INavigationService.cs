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
    }
}
