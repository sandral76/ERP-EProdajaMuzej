import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-checkouts-address',
  templateUrl: './checkouts-address.component.html',
  styleUrls: ['./checkouts-address.component.scss']
})
export class CheckoutsAddressComponent {
  @Input() checkoutForm?: FormGroup;

  constructor(private asccountService: AccountService, private toastr: ToastrService) { }

  saveKorisnikDetalji() {
    this.asccountService.updateKorisnikDetaljiPorudzbine(this.checkoutForm?.get('addressForm')?.value).subscribe({
      next: () =>{ 
        this.toastr.success('Detalji porudzbine su sacuvani!');
        this.checkoutForm?.get('addressForm')?.reset(this.checkoutForm?.get('addressForm')?.value);
      }
    })
  }
}
