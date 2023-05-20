import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  token?:string;

  constructor(private accoutnService:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accoutnService.currentKorisnik$.pipe(take(1)).subscribe({
      next:korisnik=>this.token=korisnik?.token
    })

    if(this.token){
      request=request.clone({
        setHeaders:{
         Authorization: `Bearer ${this.token}`
        }
      })
    }
    return next.handle(request);
  }
}
