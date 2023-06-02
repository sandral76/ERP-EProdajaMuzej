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

  createOrder(porudzbina:Porudzbina,porudzbinaId:number){
    console.log(porudzbina);
    return this.http.put<Porudzbina>(this.baseUrl+'porudzbina/'+porudzbinaId,porudzbina)
  }


}
