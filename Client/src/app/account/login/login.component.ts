import { Component } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent {

  loginForm = new FormGroup({
    korisnickoIme: new FormControl('', [Validators.required,Validators.minLength(3), Validators.maxLength(100),Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W).+$')]),
    lozinka: new FormControl('', [Validators.required,Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W).+$')])
  })
  returnUrl:string;
 
  constructor(private accountService:AccountService,private router:Router, private activatedRoute:ActivatedRoute){
    this.returnUrl=this.activatedRoute.snapshot.queryParams['returnUrl']||'/shop'
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe({
      next:()=>this.router.navigateByUrl('/shop')}
    )
  }
}
