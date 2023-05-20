import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Porudzbina, PorudzbinaToCreate } from '../shared/models/porudzbina';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl=environment.apiUrl;
  
  constructor(private http:HttpClient){ }

  createOrder(porudzbina:Porudzbina){
    return this.http.post<Porudzbina>(this.baseUrl+'porudzbina',porudzbina)
  }


}
