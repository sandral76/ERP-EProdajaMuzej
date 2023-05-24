import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkouts-review',
  templateUrl: './checkouts-review.component.html',
  styleUrls: ['./checkouts-review.component.scss']
})
export class CheckoutsReviewComponent {
  @Input() appStepper?:CdkStepper;

  constructor(private basketService:BasketService,private toastr:ToastrService){}

  createPaymentIntent(){
    this.basketService.createPaymentIntent().subscribe({
      next:()=> {
        this.appStepper?.next();
    },
      error:error=>this.toastr.error(error.message)
    })
  }
}
