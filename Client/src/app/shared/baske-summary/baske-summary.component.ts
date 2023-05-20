import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StavkaPorudzbine } from '../models/basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-baske-summary',
  templateUrl: './baske-summary.component.html',
  styleUrls: ['./baske-summary.component.scss']
})
export class BaskeSummaryComponent {
  @Output() addStavka = new EventEmitter<StavkaPorudzbine>();
  @Output() removeStavka = new EventEmitter<number>();
  @Input() isBasket=true;

  constructor(public basketService: BasketService) { }
  
  addStavkaPorudzbine(stavka: StavkaPorudzbine) {
    this.addStavka.emit(stavka);
  }

  removeStavkaPorudzbine(stavkaId: number) {
    this.removeStavka.emit(stavkaId);
  }
}
