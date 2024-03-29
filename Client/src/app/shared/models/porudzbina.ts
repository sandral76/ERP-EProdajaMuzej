import { DetaljiPorudzbine } from "./detaljiPorudzbine"

export interface Root {
    korisnikId: number
    korisnickoIme: string
    ime: string
    prezime: string
    tipKorisnika: string
    porudzbinas: Porudzbina[]
  }

  export interface PorudzbinaToCreate{
    korpaId:number;
    dostava:DetaljiPorudzbine
  }
  
  export interface Porudzbina {
    porudzbinaId?:number,
    datumKreiranja: string
    statusPorudzbine: string
    iznosPorudzbine: number
    popustNaPorudzbinu: number
    datumAzuriranja: string
    dostavaId?: number | undefined//DetaljiPorudzbine
    korisnikId: number
    paymentIntendId?:string
    stavkaPorudzbines?: StavkaPorudzbine[]
  }
  
  export interface StavkaPorudzbine {
    stavkaPorudzbineId: number
    cenaStavka: number
    popustStavka: number
    ulaznicaId: number
    korpaId: number | undefined
    izlozba: string
  }
  export class Porudzbina implements Porudzbina{
    datumKreiranja="2022-05-10";
    statusPorudzbine="u obradi";
    iznosPorudzbine=0;
    popustNaPorudzbinu=0
    datumAzuriranja="2022-05-10";
    dostavaId?=1; //DetaljiPorudzbine
    korisnikId=6;
    paymentIntendId?:string
    //paymentIntendId?: string | undefined
    //stavkaPorudzbines?:StavkaPorudzbine[]=[];
  }