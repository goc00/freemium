using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ConfigurationsRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public string GetConfig(string TagName)
        {
            var config = db.Configuration.FirstOrDefault(e => e.TagName == TagName);

            if (config == null)
                return "- NO CONFIG -";

            return config.Value;
        }
    }
}
