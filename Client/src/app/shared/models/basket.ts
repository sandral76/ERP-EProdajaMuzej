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
    porudzbinaId: number
    izlozba: string
  }
  export class Korpa implements Korpa{
    
    korpaId = Math.floor(Math.random() *100)+4;
    brojUlaznica= 0;
    ukupanIznos= 0
    stavkaPorudzbines: StavkaPorudzbine[]=[];
  }

  export interface BasketTotals{
    shipping:number;
    subtotal:number;
    total:number;
  }