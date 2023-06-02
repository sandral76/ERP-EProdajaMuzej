import * as cuid from 'cuid';
export interface Korpa {
    korpaId: number
    brojUlaznica: number
    ukupanIznos: number
    stavkaPorudzbines: StavkaPorudzbine[],
    clientSecret?:string,
    paymentIntendId?:string
  }
  
  export interface StavkaPorudzbine {
    stavkaPorudzbineId: number
    cenaStavka: number
    popustStavka: number
    ulaznicaId: number
    korpaId: number | undefined
    porudzbinaId: number | undefined
    izlozba: string
    dostupna:boolean
  }

  export interface Porudzbina {
    porudzbinaId: number
    datumKreiranja: Date
    statusPorudzbine: string,
    iznosPorudzbine:number,
    popustNaPorudzbinu:number,
    datumAzuriranja:Date,
    //dostavaId:number,
    korisnikId:number,
    stavkaPorudzbines: StavkaPorudzbine[],
    paymentIntendId?:string | null | undefined
  }
  export class Korpa implements Korpa{
    
    korpaId = Math.floor(Math.random() *100)+4;
    brojUlaznica= 0;
    ukupanIznos= 0
    stavkaPorudzbines: StavkaPorudzbine[]=[];
  }
  export class Porudzbina implements Porudzbina{
    porudzbinaId= Math.floor(Math.random() *100)+4;
    datumKreiranja=new Date();
    statusPorudzbine="u obradi";
    iznosPorudzbine=0;
    popustNaPorudzbinu=0
    datumAzuriranja=new Date()
    //dostavaId=1; //DetaljiPorudzbine
    korisnikId=Number(localStorage.getItem('korisnik_id'));
    paymentIntendId?:string |null
    //paymentIntendId?: string | undefined
    //stavkaPorudzbines:StavkaPorudzbine[]=[];
  }

  export interface BasketTotals{
    shipping:number;
    subtotal:number;
    total:number;
  }