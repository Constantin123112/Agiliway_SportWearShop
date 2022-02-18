using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface IAnalyticsService
    {
        List<Chart_registration_users> Registrations_for_last_month();
    }
}
