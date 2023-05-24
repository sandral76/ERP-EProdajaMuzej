using Core.Entities;
using AutoMapper;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PaymentRepository : IPaymentRepository

    {
        private readonly IGenericRepository<Korpa> korpaRepo;
        private readonly IGenericRepository<Porudzbina> porudzbinaRepo;
        //private readonly KorpaController korpaCtrl;
        private readonly EProdajaMuzejContext dbContext;

        private readonly IGenericRepository<Ulaznica> ulaznicaRepo;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public PaymentRepository(IGenericRepository<Korpa> korpaRepo, IConfiguration config,
        IGenericRepository<Ulaznica> ulaznicaRepo, IMapper mapper, EProdajaMuzejContext dbContext, IGenericRepository<Porudzbina> porudzbinaRepo)
        {
            this.korpaRepo = korpaRepo;
            this.config = config;
            this.ulaznicaRepo = ulaznicaRepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.porudzbinaRepo = porudzbinaRepo;

        }

        public async Task<ActionResult<KorpaDTO2>> CreateOrUpdatePaymentIntent(int korpaId)
        {

            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
            //var korpa = await korpaRepo.GetByIdAsync(korpaId);
            var korpa = dbContext.Korpas
            .Include(s => s.StavkaPorudzbines)
            .Where(s => s.KorpaId == korpaId)
            .Select(s => new Korpa
            {
                KorpaId = s.KorpaId,
                BrojUlaznica = s.BrojUlaznica,
                UkupanIznos = s.UkupanIznos,
                ClientSecret = s.ClientSecret,
                PaymentIntendId = s.PaymentIntendId,
                StavkaPorudzbines = s.StavkaPorudzbines
            .Select(sp => new StavkaPorudzbine
            {
                StavkaPorudzbineId = sp.StavkaPorudzbineId,
                CenaStavka = sp.CenaStavka,
                PopustStavka = sp.PopustStavka,
                UlaznicaId = sp.UlaznicaId,
                KorpaId = sp.KorpaId,
            })
            .ToList()
            })
            .FirstOrDefault();

            if (korpa == null) return null;

            foreach (var stavka in korpa.StavkaPorudzbines)
            {
                var ulaznicaStavka = await ulaznicaRepo.GetByIdAsync(stavka.UlaznicaId);
                if (stavka.CenaStavka != ulaznicaStavka.CenaUlaznice)
                {
                    stavka.CenaStavka = ulaznicaStavka.CenaUlaznice;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;
            if (string.IsNullOrEmpty(korpa.PaymentIntendId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)korpa.UkupanIznos,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                korpa.PaymentIntendId = intent.Id;
                korpa.ClientSecret = intent.ClientSecret;

            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)korpa.UkupanIznos,
                };
                await service.UpdateAsync(korpa.PaymentIntendId, options);
            }
            await korpaRepo.Update(korpa);
            return mapper.Map<Korpa, KorpaDTO2>(korpa);

        }

        public async Task<Porudzbina> UpdatePorudzbinaPaymentFailed(string paymentIntentId)
        {
            var porudzbina = dbContext.Porudzbinas
            .Include(s => s.Dostava)
            .Include(s => s.Korisnik)
            .Include(s => s.StavkaPorudzbines)
            .Where(s => s.PaymentIntendId == paymentIntentId)
            .Select(s => new Porudzbina
            {
                PorudzbinaId = s.PorudzbinaId,
                DatumKreiranja = s.DatumKreiranja,
                StatusPorudzbine = s.StatusPorudzbine,
                IznosPorudzbine = s.IznosPorudzbine,
                PopustNaPorudzbinu = s.PopustNaPorudzbinu,
                DatumAzuriranja = s.DatumAzuriranja,
                Dostava = s.Dostava,
                Korisnik = s.Korisnik,
                PaymentIntendId = s.PaymentIntendId,
                StavkaPorudzbines = s.StavkaPorudzbines
            .Select(sp => new StavkaPorudzbine
            {
                StavkaPorudzbineId = sp.StavkaPorudzbineId,
                CenaStavka = sp.CenaStavka,
                PopustStavka = sp.PopustStavka,
                UlaznicaId = sp.UlaznicaId,
                KorpaId = sp.KorpaId,
                //DatumKreiranja = s.DatumKreiranja
            })
            .ToList()
            })
            .FirstOrDefault();
            if (porudzbina != null)
            {
                porudzbina.PorudzbinaId=porudzbina.PorudzbinaId;
                porudzbina.DatumKreiranja = porudzbina.DatumKreiranja;
                porudzbina.StatusPorudzbine = "odbijeno";
                porudzbina.IznosPorudzbine = porudzbina.IznosPorudzbine;
                porudzbina.PopustNaPorudzbinu = porudzbina.PopustNaPorudzbinu;
                porudzbina.DatumAzuriranja = porudzbina.DatumAzuriranja;
                porudzbina.DostavaId = porudzbina.DostavaId;
                porudzbina.KorisnikId = porudzbina.KorisnikId;
                
            }
            await porudzbinaRepo.Update(porudzbina);
            return porudzbina;

        }

        public async Task<Porudzbina> UpdatePorudzbinaPaymentSucceeded(string paymentIntentId)
        {
            var porudzbina = dbContext.Porudzbinas
            .Include(s => s.Dostava)
            .Include(s => s.Korisnik)
            .Include(s => s.StavkaPorudzbines)
            .Where(s => s.PaymentIntendId == paymentIntentId)
            .Select(s => new Porudzbina
            {
                PorudzbinaId = s.PorudzbinaId,
                DatumKreiranja = s.DatumKreiranja,
                StatusPorudzbine = s.StatusPorudzbine,
                IznosPorudzbine = s.IznosPorudzbine,
                PopustNaPorudzbinu = s.PopustNaPorudzbinu,
                DatumAzuriranja = s.DatumAzuriranja,
                Dostava = s.Dostava,
                Korisnik = s.Korisnik,
                PaymentIntendId = s.PaymentIntendId,
                StavkaPorudzbines = s.StavkaPorudzbines
            .Select(sp => new StavkaPorudzbine
            {
                StavkaPorudzbineId = sp.StavkaPorudzbineId,
                CenaStavka = sp.CenaStavka,
                PopustStavka = sp.PopustStavka,
                UlaznicaId = sp.UlaznicaId,
                KorpaId = sp.KorpaId,
                //DatumKreiranja = s.DatumKreiranja
            })
            .ToList()
            })
            .FirstOrDefault();
            if (porudzbina != null){
                 porudzbina.PorudzbinaId=porudzbina.PorudzbinaId;
                porudzbina.DatumKreiranja = porudzbina.DatumKreiranja;
                porudzbina.StatusPorudzbine = "placeno";
                porudzbina.IznosPorudzbine = porudzbina.IznosPorudzbine;
                porudzbina.PopustNaPorudzbinu = porudzbina.PopustNaPorudzbinu;
                porudzbina.DatumAzuriranja = porudzbina.DatumAzuriranja;
                porudzbina.DostavaId = porudzbina.DostavaId;
                porudzbina.KorisnikId = porudzbina.KorisnikId;}
            await porudzbinaRepo.Update(porudzbina);
            return porudzbina;
        }
    }
}