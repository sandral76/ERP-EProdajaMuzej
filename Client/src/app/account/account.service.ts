import { Injectable } from '@angular/core';
import { ReplaySubject, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Korisnik } from '../shared/models/korisnik';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { DetaljiPorudzbine } from '../shared/models/detaljiPorudzbine';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private currentKorisnikSource = new ReplaySubject<Korisnik | null>(1);
  currentKorisnik$ = this.currentKorisnikSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentKorisnik(token: string | null) {
    if (token === null) {
      this.currentKorisnikSource.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get<Korisnik>(this.baseUrl + 'auth', { headers }).pipe(
      map(korisnik => {
        if (korisnik) {
          localStorage.setItem('token', korisnik.token);
          localStorage.setItem('korisnik_id', korisnik.korisnikId.toString());
          this.currentKorisnikSource.next(korisnik);
          console.log(korisnik);
          return korisnik;
        } else {
          return null
        }
      })
    )
  }

  
  login(values: any) {
    return this.http.post<Korisnik>(this.baseUrl + 'auth/login', values).pipe(
      map(korisnik => {
        localStorage.setItem('token', korisnik.token);
        localStorage.setItem('korisnik_id', korisnik.korisnikId.toString());
        this.currentKorisnikSource.next(korisnik);
        console.log(korisnik);
        return korisnik;
        
      })  //subscribe(korisnik=>console.log(korisnik))
    )
  }

  register(values: any) {
    return this.http.post<Korisnik>(this.baseUrl + 'auth/register', values).pipe(
      map(korisnik => {
        localStorage.setItem('token', korisnik.token);
        localStorage.setItem('korisnik_id', korisnik.korisnikId.toString());
        this.currentKorisnikSource.next(korisnik);
      })
    )
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('korisnik_id');
    this.currentKorisnikSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkKorisnickoImeExists(korisnickoIme: string) {
    return this.http.get<boolean>(this.baseUrl + 'auth/korisnickoImeexists?korisnickoIme=' + korisnickoIme);
  }

  getKorisnikDetaljiPorudzbine() {
    return this.http.get<DetaljiPorudzbine>(this.baseUrl + 'auth/dostava',)
  }
  updateKorisnikDetaljiPorudzbine(detaljiPorudzbine: DetaljiPorudzbine) {
    return this.http.put(this.baseUrl + 'auth/dostava', detaljiPorudzbine);
  }
}
