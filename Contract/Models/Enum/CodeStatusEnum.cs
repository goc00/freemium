using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contract.Models.Enum {

    public enum CodeStatusEnum
    {
        OK                              = 200,
        NO_CONTENT                      = 204,
        UNAUTHORIZED                    = 401,
        CONFLICT                        = 409,
        UNSOPPORTED_MEDIA_TYPE          = 415,
        BAD_REQUEST                     = 400,
        INTERNAL_ERROR                  = 500
    }

}