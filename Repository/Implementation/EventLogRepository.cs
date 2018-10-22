using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class EventLogRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public bool SetLog(string text, string desc)
        {
            db.EventsLogs.Add(new EntityFramework.EventsLogs { DateCreated = DateTime.Now, Text = text, Description = desc });
            db.SaveChanges();

            return true;
        }
    }
}
