using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Common.Services
{
    public interface IPermissionsService
    {
        Task<bool> IsAllowedAsync(Permissions permission);

        Task<bool> RequestAsync(Permissions permission);

        Task<bool> HasBeenRequestedAsync(Permissions permission);
    }

    public enum Permissions
    {
        Location, Camera
    }
}
