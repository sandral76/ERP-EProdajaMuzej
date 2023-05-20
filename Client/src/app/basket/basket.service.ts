import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BasketTotals, Korpa, StavkaPorudzbine } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Ulaznica } from '../shared/models/ulaznice';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = environment.apiUrl;
  private korpaSource = new BehaviorSubject<Korpa | null>(null);
  private stavkaSource = new BehaviorSubject<StavkaPorudzbine | null>(null);
  private basketTotalsSource = new BehaviorSubject<BasketTotals | null>(null);
  korpaSource$ = this.korpaSource.asObservable();
  stavkaSource$ = this.stavkaSource.asObservable();
  basketTotalsSource$ = this.basketTotalsSource.asObservable();
  brojUlaznica: number = 0;
  shipping: number=0;
  total: number | undefined;
  subtotal: number | undefined;
  izlozba: string | undefined
  public korpa: Korpa;
  stavkePorudzbine: StavkaPorudzbine[] = []
  stavkaPorudzbine: StavkaPorudzbine | undefined;

  constructor(private http: HttpClient) {
    this.korpa = this.getKorpaFromStorage() ?? this.createBasket();
  }

  getKorpa(korpaId: string | null) {
    return this.http.get<Korpa>(this.baseUrl + 'korpa/' + korpaId).subscribe({
      next: korpa => {
        this.korpaSource.next(korpa);
        //this.calculateTotals()
      }
    });
  }

  addStavka(stavkaPorudzbine: StavkaPorudzbine) {
    return this.http.post<StavkaPorudzbine>(this.baseUrl + 'stavkaPorudzbine', stavkaPorudzbine).subscribe({
      next: stavkaPorudzbine => this.stavkaSource.next(stavkaPorudzbine)
    })
  }
  addKorpa(korpa: Korpa) {
    return this.http.post<Korpa>(this.baseUrl + 'korpa', korpa).subscribe({
      next: korpa => {
        this.korpaSource.next(korpa);
        //this.calculateTotals();
      }
    })
  }

  getCurrentKorpaValue() {
    return this.korpaSource.value;
  }
  getKorpaFromStorage(): Korpa | null {
    //return this.korpa;
    const korpaString = localStorage.getItem('korpa');
    if (korpaString) {
      return JSON.parse(korpaString);
    }
    return null;
  }

  addStavkaPorudzbineToKorpa(ulaznica: Ulaznica) {
    const stavkaToAdd = this.mapUlaznicaToStavkaPorudzbine(ulaznica);
    //this.korpa = this.getKorpaFromStorage() ?? this.createBasket();
    this.korpa.stavkaPorudzbines = this.addOrUpdateItem(this.korpa.stavkaPorudzbines, stavkaToAdd);
    //this.addKorpa(this.korpa)
    this.addStavka(stavkaToAdd)
    this.getKorpa(localStorage.getItem('korpa_id'));
  }

  removeStavkaFromKorpa(stavkaPorudzbineId: number) {
    const korpa = this.getCurrentKorpaValue();
    if (!korpa) return;
    const stavka = this.stavkePorudzbine.find(x => x.stavkaPorudzbineId === stavkaPorudzbineId);
    if (stavka) {
      const itemIndex = this.stavkePorudzbine.findIndex(x => x.stavkaPorudzbineId === stavkaPorudzbineId);
      this.deleteKorpa(stavka);
      this.brojUlaznica -= 1;
      this.stavkePorudzbine.splice(itemIndex, 1);
      this.subtotal=this.stavkePorudzbine.reduce((subt,s)=>subt+s.cenaStavka,0)
      this.total=this.subtotal+this.shipping;
      //this.calculateTotals();
    }
  }
  deleteKorpa(stavka: StavkaPorudzbine) {
    return this.http.delete(this.baseUrl + 'stavkaPorudzbine/' + stavka.stavkaPorudzbineId + '/' + stavka.ulaznicaId).subscribe({
      next:
        () => {
          this.korpaSource.next(this.getCurrentKorpaValue());
          //this.basketTotalsSource.next;
        }
    });
  }


  private addOrUpdateItem(stavkaPorudzbines: StavkaPorudzbine[], ulaznica: StavkaPorudzbine): StavkaPorudzbine[] {
    const item = stavkaPorudzbines.find(x => x.ulaznicaId === ulaznica.ulaznicaId);
    const itemIndex = stavkaPorudzbines.findIndex(x => x.ulaznicaId === ulaznica.ulaznicaId);
    if (itemIndex > -1) {
      stavkaPorudzbines[itemIndex] = ulaznica;
    } else {
      stavkaPorudzbines.push(ulaznica);
    }
    this.brojUlaznica = stavkaPorudzbines.reduce(sum => sum + 1, 0);
    this.izlozba = ulaznica.izlozba
    this.stavkePorudzbine = stavkaPorudzbines;
    this.subtotal=this.stavkePorudzbine.reduce((subt,s)=>subt+s.cenaStavka,0)
    this.korpa.ukupanIznos=this.subtotal
    this.total=this.subtotal+this.shipping;
    return stavkaPorudzbines;
  }

  createBasket(): Korpa {
    const basket = new Korpa();
    //localStorage.setItem('korpa_id',basket.korpaId);
    localStorage.setItem('korpa_id', basket.korpaId.toString());
    this.addKorpa(basket);
    return basket;
  }

  private mapUlaznicaToStavkaPorudzbine(ulaznica: Ulaznica): StavkaPorudzbine {
    return {
      stavkaPorudzbineId: ulaznica.ulaznicaId,
      cenaStavka: ulaznica.cenaUlaznice,
      popustStavka: 0,
      ulaznicaId: ulaznica.ulaznicaId,
      korpaId: this.korpa.korpaId,
      porudzbinaId: 2,
      izlozba: ulaznica.izlozba
    }
  }
  /*private calculateTotals() {
    const basket = this.getCurrentKorpaValue();
    if (!basket) return;
    this.shipping = 0;
    this.subtotal = basket.ukupanIznos;
    this.total = this.subtotal + this.shipping;
    //this.basketTotalsSource.next({ this.shipping, this.total, this.subtotal });
  }*/

}
