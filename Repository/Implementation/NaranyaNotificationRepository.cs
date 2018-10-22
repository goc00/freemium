using Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class NaranyaNotificationRepository
    {
        public EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public bool SaveNewNotification(NaranyaNotification data)
        {
            var Notification = new EntityFramework.NaranyaNotification
            {
                id_event = data.id_event,
                ipn_url = data.ipn_url,
                ipn_type = data.ipn_type,
                verify_sign = data.verify_sign,
                id_app = data.id_app,
                id_customer = data.id_customer,
                id_transaction = data.id_transaction,
                amount = data.amount,
                currency = data.currency,
                id_subscription = data.id_subscription,
                status = data.status,
                id_service = data.id_service,
                created = data.created,
                updated = data.updated,
                fecha = DateTime.Now
            };

            db.NaranyaNotification.Add(Notification);

            db.SaveChanges();

            return true;
        }
    }
}
