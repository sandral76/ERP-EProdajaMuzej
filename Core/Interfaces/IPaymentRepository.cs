using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interfaces
{
    public interface IPaymentRepository
    {
        Task<ActionResult<KorpaDTO2>> CreateOrUpdatePaymentIntent(int korpaId);
        Task<Porudzbina> UpdatePorudzbinaPaymentSucceeded(string paymentIntentId);
        Task<Porudzbina> UpdatePorudzbinaPaymentFailed(string paymentIntentId);


    }
}