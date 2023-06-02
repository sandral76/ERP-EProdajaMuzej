import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { AccountService } from 'src/app/account/account.service';
import { Porudzbina } from 'src/app/shared/models/porudzbina'
import { Korpa } from 'src/app/shared/models/basket';
import { Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement, loadStripe } from '@stripe/stripe-js';
import { NavigationExtras, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';


@Component({
  selector: 'app-checkouts-payment',
  templateUrl: './checkouts-payment.component.html',
  styleUrls: ['./checkouts-payment.component.scss']
})
export class CheckoutsPaymentComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;
  @ViewChild('cardNumber') cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc') cardCvcElement?: ElementRef;
  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardNumberComplete=false;
  cardExpiryComplete=false;
  cardCvcComplete=false;
  cardErrors: any;
  loading = false;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService,
    private toastr: ToastrService, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    loadStripe('pk_test_51NABwOJL93lh1XvrwWzJ9oYxIXLQcdjy2xIPn0qAC1kwVmt5SO2ZZF9m6Ex5HUWHNGk6Hm93qJVZf1ymwMxHCB5b00HctMO95B').then(stripe => {
      this.stripe = stripe;
      const elements = stripe?.elements();
      if (elements) {
        this.cardNumber = elements.create('cardNumber');
        this.cardNumber.mount(this.cardNumberElement?.nativeElement);
        this.cardNumber.on('change', event => {
          this.cardNumberComplete=event.complete;
          if (event.error) this.cardErrors ="Popunite broj kartice do kraja!";
          else this.cardErrors = null;
        })

        this.cardExpiry = elements.create('cardExpiry');
        this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
        this.cardExpiry.on('change', event => {
          this.cardExpiryComplete=event.complete;
          if (event.error) this.cardErrors = "Godina isteka kartice je nevazeca!";
          else this.cardErrors = null;
        })

        this.cardCvc = elements.create('cardCvc');
        this.cardCvc.mount(this.cardCvcElement?.nativeElement);
        this.cardCvc.on('change', event => {
          this.cardCvcComplete=event.complete;
          if (event.error) this.cardErrors = "CVC broj nije potpun!";
          else this.cardErrors = null;
        })
      }
    })
  }
  get paymentFormComplete(){
    return this.checkoutForm?.get('paymentForm')?.valid && this.cardNumberComplete && this.cardExpiryComplete && this.cardCvcComplete
  }

  async submitOrder() {
    this.loading = true;
    const basket = this.basketService.getCurrentKorpaValue();
    try {
      const createdPorudzbina = await this.createPorudzbina(basket);
      console.log(createdPorudzbina);
      const paymentResult = await this.confirmPaymentWithStripe(basket);
      if (paymentResult.paymentIntent) {
        const navigationExtras: NavigationExtras = { state: createdPorudzbina };
        this.router.navigate(['checkout/success'], navigationExtras);
      } else {
        this.toastr.error(paymentResult.error.message);
      }
    } catch (error: any) {
      console.log(error);
      this.toastr.error(error.message);
    } finally {
      this.loading = false;
    }

  }
  private async confirmPaymentWithStripe(basket: Korpa | null) {
    if (!basket) throw new Error('Korpa je prazna.');
    const result = this.stripe?.confirmCardPayment(basket.clientSecret!, {
      payment_method: {
        card: this.cardNumber!,
        billing_details: {
          name: this.checkoutForm?.get('paymentForm')?.get('vlasnikKartice')?.value
        }
      }
    });
    if (!result) throw new Error('Problem sa placanjem.')
    return result;
  }
  private async createPorudzbina(basket: Korpa | null) {
    if (!basket) throw new Error('Korpa je prazna.');
    const porudzbinaToCreate = this.getPorudzbinaToCreate(basket);
    const porudzbinaId=Number(localStorage.getItem('porudzbina_id'))
    return firstValueFrom(this.checkoutService.createOrder(porudzbinaToCreate,porudzbinaId));
  }
  private getPorudzbinaToCreate(korpa: Korpa): Porudzbina {
    return {
      datumKreiranja: new Date().toISOString().substring(0, 10),
      statusPorudzbine: 'u obradi',
      iznosPorudzbine: korpa.ukupanIznos,
      popustNaPorudzbinu: 0.0,
      datumAzuriranja: new Date().toISOString().substring(0, 10),
      dostavaId: 1,
      korisnikId:Number(localStorage.getItem('korisnik_id')) ,
      paymentIntendId:korpa.paymentIntendId
      //stavkaPorudzbines:[]
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

