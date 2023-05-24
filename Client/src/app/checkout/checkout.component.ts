import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  constructor(private fb: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    this.getAdressFormValues();
  }

  checkoutForm = this.fb.group({
    addressForm: this.fb.group({
      emailDostave: ['', Validators.required],
      ime: ['', Validators.required],
      kontaktTelefon: ['', Validators.required],
      prezime: ['', Validators.required],
    }),
    paymentForm: this.fb.group({
      //tipPlacanja: ['', Validators.required],
      vlasnikKartice: ['', Validators.required],
    })
  })

  getAdressFormValues() {
    this.accountService.getKorisnikDetaljiPorudzbine().subscribe({
      next: address => {
        address && this.checkoutForm.get('addressForm')?.patchValue(address)
        const addressForm = this.checkoutForm.get('addressForm')?.value;
        return addressForm;
      }
    })
  }
}
