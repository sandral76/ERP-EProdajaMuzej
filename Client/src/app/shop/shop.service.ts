import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Ulaznica } from '../shared/models/ulaznice';
import { Muzej } from '../shared/models/muzej';
import { Observable } from 'rxjs';
import { Pagination } from '../shared/models/pagination';
import { ShopParams } from '../shared/models/shopParams';


@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getUlaznice(shopParams: ShopParams) {
    let params = new HttpParams();
    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber);
    params = params.append('pageSize', shopParams.pageSize);
    if(shopParams.search) params= params.append('search', shopParams.search);
    if (shopParams.grad != null) {
      const url = 'https://localhost:5001/api/ulaznica/grad/' + shopParams.grad;
      return this.http.get<Pagination<Ulaznica[]>>(url,{params});
    }
    else
      //params=params.append('grad',grad);
      return this.http.get<Pagination<Ulaznica[]>>(this.baseUrl + 'ulaznica', { params });
  }

  getUlaznicaById(ulaznicaId:number){
    return this.http.get<Ulaznica>(this.baseUrl+'ulaznica/'+ulaznicaId);
  }
  
  getProductsByGrad(): Observable<Muzej[]> {
    const url = 'https://localhost:5001/api/muzej/grad';
    return this.http.get<Muzej[]>(url);
  }
  getProductsByBrandName(shopParams: ShopParams): Observable<Pagination<Ulaznica[]>> {
    let params = new HttpParams();
    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber);
    params = params.append('pageSize', shopParams.pageSize);
    if(shopParams.search) params= params.append('search', shopParams.search);
    if (shopParams.grad == 'Svi gradovi' || shopParams.grad==null) {
      const url = 'https://localhost:5001/api/ulaznica/grad';
      return this.http.get<Pagination<Ulaznica[]>>(url,{params});
    } else {
      const url = 'https://localhost:5001/api/ulaznica/grad/' + shopParams.grad;
      return this.http.get<Pagination<Ulaznica[]>>(url,{params});
    }
  }

}
