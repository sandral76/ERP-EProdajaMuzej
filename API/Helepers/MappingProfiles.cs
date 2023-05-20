using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Specification;

namespace API.Helepers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Ulaznica, UlaznicaDTO>().ForMember(d => d.Izlozba, o => o.MapFrom(s => s.Izlozba.NazivIzlozbe));
            CreateMap<TipKorisnika, TipKorisnikaDTO>();
            CreateMap<Korisnik, KorisnikDTO>();
            CreateMap<Korisnik, KorisnikDTO>().ForMember(d => d.TipKorisnika, o => o.MapFrom(s => s.TipKorisnika.Uloga));
            CreateMap<Korisnik, LoginWithTokenDTO>();
            CreateMap<Muzej, MuzejDTO>();
            CreateMap<Muzej, MuzejGradDTO>();
            CreateMap<Izlozba, IzlozbaDTO>();
            CreateMap<Korpa, KorpaDTO>();
            CreateMap<Korpa, KorpaBrojUlaznicaDTO>();
            CreateMap<DetaljiPorudzbine, DetaljiPorudzbineDTO>();
            CreateMap<IzlozbaUMuzeju, IzlozbaUMuzejuDTO>()
            .ForMember(d => d.Muzej, o => o.MapFrom(s => s.Muzej.Naziv))
            .ForMember(d => d.Izlozba, o => o.MapFrom(s => s.Izlozba.NazivIzlozbe));
            CreateMap<Porudzbina,PorudzbinaDTO>()
            .ForMember(d => d.Dostava, o => o.MapFrom(s => s.Dostava.EmailDostave))
            .ForMember(d => d.Korisnik, o => o.MapFrom(s => s.Korisnik.KorisnickoIme));
            CreateMap<Placanje, PlacanjeDTO>().ForMember(d => d.IznosPorudzbine, o => o.MapFrom(s => s.Porudzbina.IznosPorudzbine));
            CreateMap<StavkaPorudzbine, StavkaPorudzbineDTO>().ForMember(d => d.DatumKreiranja, o => o.MapFrom(s => s.Porudzbina.DatumKreiranja));
            CreateMap<Korisnik, KorisnikDostavaDTO>();
            CreateMap<Korisnik, DetaljiPorudzbineDTO>().ForMember(d => d.EmailDostave, o => o.MapFrom(s => s.Email)).
                                                        ForMember(d => d.KontaktTelefon, o => o.MapFrom(s => s.BrojTel));
   
            

        }
    }
}