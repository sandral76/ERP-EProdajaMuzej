import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { CheckoutsSuccessComponent } from './checkouts-success/checkouts-success.component';

const routes: Routes=[
  {path:'',component: CheckoutComponent},
  {path:'success',component: CheckoutsSuccessComponent}
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[RouterModule]
})
export class CheckoutRoutingModule { }
