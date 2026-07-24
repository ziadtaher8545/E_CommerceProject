using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class PaymentGatwaySettings
    {
        public string SecertKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = default!;

    }
}
