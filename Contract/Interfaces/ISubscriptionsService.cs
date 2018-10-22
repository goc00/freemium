using Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Interfaces
{
    public interface ISubscriptionsService : IDisposable
    {
        Subscriptions Get(string ProductToken, string UserCode);

        //bool AddSubscription(string NombreUsuario, string Email, string Birthday, string Genre, string Country, string Password, int SubscriptionProfileId);
        bool SetSubscription(string UserCode, int IdPlan, string AppToken);

        bool SetSubscription(string UserCode, int IdPlan, string AppToken, int addDays);

        bool SetSubscriptionWithPromo(string UserCode, int IdPlan, string AppToken, string TagPromo);
        List<ActiveSubscriptionsData> GetActiveSubscriptions(string ProductToken);
        bool ResetSubscription(string AppToken, string UserCode);

        bool ResetSubscriptionByCategory(string AppToken, string UserCode, string Category);

        List<SubscriptionsResponse> GetSubscriptionsByRenewal(DateTime CurrentDate, int top);
    }
}
