using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Payments
{
    internal class StripePaymentGatway : IPaymentGatway
    {

        private readonly PaymentIntentService _payment = new();
        public StripePaymentGatway(IOptions<PaymentGatwaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecertKey;
           
        }


        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var intentOptions = new PaymentIntentCreateOptions()
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                PaymentMethodTypes = ["card"]
            };
            var paymentIntend = await _payment.CreateAsync(intentOptions, cancellationToken: ct);
            return new PaymentIntentResult(paymentIntend.Id, paymentIntend.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntendId, decimal amount, CancellationToken ct = default)
        {
            var intentOptions = new PaymentIntentUpdateOptions()
            {
                Amount = (long)amount

            };
            var paymentIntend = await _payment.UpdateAsync(paymentIntendId, intentOptions, cancellationToken: ct);
            return new PaymentIntentResult(paymentIntend.Id, paymentIntend.ClientSecret);

        }
    }
   
}
