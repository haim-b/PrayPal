using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrayPal.Common.Services
{
    public interface ILocationService
    {
        Task<Geoposition> GetCurrentPositionAsync(CancellationToken cancellationToken = default);
    }
}
