import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { AccountService } from 'src/app/account/account.service';
import { Porudzbina } from 'src/app/shared/models/porudzbina'
import { Korpa } from 'src/app/shared/models/basket';


@Component({
  selector: 'app-checkouts-payment',
  templateUrl: './checkouts-payment.component.html',
  styleUrls: ['./checkouts-payment.component.scss']
})
export class CheckoutsPaymentComponent {
  @Input() checkoutForm?: FormGroup;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, private toastr: ToastrService, private accountService: AccountService) { }

  submitOrder() {
    const basket = this.basketService.korpa;
    if (!basket) return;
    console.log(basket);
    const porudzbinaToCreate = this.getPorudzbinaToCreate(basket);
    if (!porudzbinaToCreate) return;
    this.checkoutService.createOrder(porudzbinaToCreate).subscribe({
      next: porudzbina => {
        this.toastr.success('Porudzbina kreirana uspesno!');
        //this.basketService.deleteLocalKorpa();
        console.log(porudzbina);
      }
    })
  }
  private getPorudzbinaToCreate(korpa:Korpa) : Porudzbina {
    return {
      datumKreiranja: new Date().toISOString().substring(0, 10),
      statusPorudzbine: 'u obradi',
      iznosPorudzbine:korpa.ukupanIznos,
      popustNaPorudzbinu: 0.0,
      datumAzuriranja: new Date().toISOString().substring(0, 10),
      dostavaId:1, // Specify the necessary details for the dostava property
      korisnikId:6
      //stavkaPorudzbines:korpa.stavkaPorudzbines // Provide an array of StavkaPorudzbine objects
    };    
    /*const addressForm = this.checkoutForm?.get('addressForm')?.value as DetaljiPorudzbine;
    console.log(addressForm);
    if (!addressForm) return;
    return {
      korpaId: basket.korpaId,
      dostava: addressForm
    }*/
  }
}

