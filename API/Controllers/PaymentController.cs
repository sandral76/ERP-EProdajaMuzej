using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private const string WhSecret = "whsec_b55aaa393ddf653aacd2ec3e3be7ea66031c6159de19d421a9a350cb2ba6d9d7";
        private readonly IPaymentRepository paymentRepository;
        private readonly ILogger<PaymentController> logger;

        public PaymentController(IPaymentRepository paymentRepository, ILogger<PaymentController> logger)
        {
            this.paymentRepository = paymentRepository;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("{korpaId}")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<KorpaDTO2>> CreateOrUpdatePaymentIntent(int korpaId)
        {
            try
            {
                var korpa = await paymentRepository.CreateOrUpdatePaymentIntent(korpaId);
                return korpa;
            }
            catch (Exception ex)
            {
                // Log the exception or return a specific error response with the exception message
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
            Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;
            Porudzbina porudzbina;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment succeeded:", intent.Id);
                    porudzbina=await paymentRepository.UpdatePorudzbinaPaymentSucceeded(intent.Id);
                    logger.LogInformation("Porudzbina modifikovana kao uspesno placena:", porudzbina.PorudzbinaId);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment failed:", intent.Id);
                    porudzbina=await paymentRepository.UpdatePorudzbinaPaymentSucceeded(intent.Id);
                    logger.LogInformation("Porudzbina modifikovana kao neuspesno placena:", porudzbina.PorudzbinaId);
                    break;
            }

            return new EmptyResult();
        }
    }
}