using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Perm = Xamarin.Essentials.Permissions;

namespace PrayPal.Common.Services
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<bool> HasBeenRequestedAsync(Permissions permission)
        {
            Func<Task<PermissionStatus>> check = GetCheckingMethod(permission);

            var status = await check();
            return status != PermissionStatus.Unknown;
        }

        public async Task<bool> IsAllowedAsync(Permissions permission)
        {
            Func<Task<PermissionStatus>> check = GetCheckingMethod(permission);

            return await check() == PermissionStatus.Granted;
        }

        private static Func<Task<PermissionStatus>> GetCheckingMethod(Permissions permission)
        {
            Func<Task<PermissionStatus>> check;

            switch (permission)
            {
                case Permissions.Location:
                    check = Perm.CheckStatusAsync<Perm.LocationWhenInUse>;
                    break;
                case Permissions.Camera:
                    check = Perm.CheckStatusAsync<Perm.Camera>;
                    break;
                default:
                    throw new NotSupportedException("Permission is not supported.");
            }

            return check;
        }

        public async Task<bool> RequestAsync(Permissions permission)
        {
            Func<Task<PermissionStatus>> request;

            switch (permission)
            {
                case Permissions.Location:
                    request = Perm.RequestAsync<Perm.LocationWhenInUse>;
                    break;
                case Permissions.Camera:
                    request = Perm.RequestAsync<Perm.Camera>;
                    break;
                default:
                    throw new NotSupportedException("Permission is not supported.");
            }

            return await MainThread.InvokeOnMainThreadAsync(async () => await request()) == PermissionStatus.Granted;
        }
    }
}
