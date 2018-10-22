using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class IntegrationConfigController : ApiController
    {
        // POST: api/IntegrationConfig
        public void Post(decimal NumericValue, bool SwitchValue, bool UserIsActive)
        {
            Contract.Models.IntegrationValues.NumericValue = NumericValue;
            Contract.Models.IntegrationValues.SwitchValue = SwitchValue;
            Contract.Models.IntegrationValues.UserIsActive = UserIsActive;
        }
    }
}
