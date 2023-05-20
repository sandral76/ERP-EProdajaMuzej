import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Route, Router } from '@angular/router';
import { debounceTime, finalize, map, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  errors: string[] | null = null;

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }


  registerForm = this.fb.group({
    korisnickoIme: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W).+$')], [this.validateKorisnickoImeNotTaken()]],
    lozinka: ['', [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W).+$')]],
    brojTel: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    ime: ['', [Validators.required]],
    prezime: ['', [Validators.required]]
  })


  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl('/shop'),
      error: error => this.errors = error.errors
    })
  }


  validateKorisnickoImeNotTaken(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return control.valueChanges.pipe(
        debounceTime(1000),
        take(1),
        switchMap(() => {
          return this.accountService.checkKorisnickoImeExists(control.value).pipe(
            map(result => result ? { korisnickoImeExists: true } : null),
            finalize(() => control.markAsTouched())
          )
        })
      )
    }
  }
}
