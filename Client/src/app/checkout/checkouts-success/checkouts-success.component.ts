import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Porudzbina } from 'src/app/shared/models/porudzbina';

@Component({
  selector: 'app-checkouts-success',
  templateUrl: './checkouts-success.component.html',
  styleUrls: ['./checkouts-success.component.scss']
})
export class CheckoutsSuccessComponent {
  order?: Porudzbina
  
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.order = navigation?.extras?.state as Porudzbina
  }
}

