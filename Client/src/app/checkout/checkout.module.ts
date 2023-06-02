import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CheckoutsAddressComponent } from './checkouts-address/checkouts-address.component';
import { CheckoutsReviewComponent } from './checkouts-review/checkouts-review.component';
import { CheckoutsPaymentComponent } from './checkouts-payment/checkouts-payment.component';
import { CheckoutsSuccessComponent } from './checkouts-success/checkouts-success.component';

@NgModule({
  declarations: [
    CheckoutComponent,
    CheckoutsAddressComponent,
    CheckoutsReviewComponent,
    CheckoutsPaymentComponent,
    CheckoutsSuccessComponent
  ],
  imports: [
    CommonModule,
    CheckoutRoutingModule,
    SharedModule
  ]
})
export class CheckoutModule { }
