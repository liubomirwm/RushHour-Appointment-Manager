using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RushHour.Enums
{
    [Flags]
    public enum CustomAuthorizeEnum
    {
        Default = 1,
        AnonymousUser = 2,
        NonAdmin = 4,
        Admin = 8,
        Everyone = 16
    }
}