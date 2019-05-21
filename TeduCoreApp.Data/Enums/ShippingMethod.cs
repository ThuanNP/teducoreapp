using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TeduCoreApp.Data.Enums
{
    public enum ShippingMethod
    {
        [Description("Standard service : on 3 - 5 work days with free cost.")]
        Standard,
        [Description("Express service : on 1 - 2 work days with 10.000")]
        Express
    }
}
