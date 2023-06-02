import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  title = 'Muzejska kultura Srbije';

  constructor(private basketService:BasketService,private accountService:AccountService) {}

  ngOnInit(): void {
    this.loadKorpa();
    this.loadCurrentKorisnik();
    
  }

  loadKorpa(){
    const korpaId=localStorage.getItem('korpa_id');  //ili korpa samo
    if(korpaId) this.basketService.getKorpa(korpaId);
  }
  loadCurrentKorisnik(){
    const token=localStorage.getItem('token');
    this.accountService.loadCurrentKorisnik(token).subscribe();
  }
}
 

