using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class PaymentIntentResult
    {
        public PaymentIntentResult(string paymentIntendId, string clientSecert)
        {
            PaymentIntendId = paymentIntendId;
            ClientSecert = clientSecert;
        }

        public string PaymentIntendId { get;  } = default!;
        public string ClientSecert { get;  } = default!;
    }
}
