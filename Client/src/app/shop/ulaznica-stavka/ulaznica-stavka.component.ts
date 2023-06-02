import { Component, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Ulaznica } from 'src/app/shared/models/ulaznice';

@Component({
  selector: 'app-ulaznica-stavka',
  templateUrl: './ulaznica-stavka.component.html',
  styleUrls: ['./ulaznica-stavka.component.scss']
})
export class UlaznicaStavkaComponent {
  @Input() ulaznica?: Ulaznica;

  constructor(private basketService: BasketService) { }

  addStavkaPorudzbineToKorpa() {
    this.ulaznica && this.basketService.addStavkaPorudzbineToKorpa(this.ulaznica);
  }
  //hideUlaznicaFlag: boolean = false;

  /*hideUlaznica() {
    this.hideUlaznicaFlag = true;
  }*/
  

}
