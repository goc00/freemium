using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.Interfaces;

namespace Business.Implementation
{
    public class StatusService : IStatusService
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "API: OK (Implementation)", "BD: ok (Implementation)" };
        }
    }
}
