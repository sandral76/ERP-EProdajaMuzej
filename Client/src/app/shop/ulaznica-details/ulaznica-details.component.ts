import { Component, OnInit } from '@angular/core';
import { Ulaznica } from 'src/app/shared/models/ulaznice';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-ulaznica-details',
  templateUrl: './ulaznica-details.component.html',
  styleUrls: ['./ulaznica-details.component.scss']
})
export class UlaznicaDetailsComponent implements OnInit {
  ulaznica?: Ulaznica;
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,private bcService:BreadcrumbService) {
    this.bcService.set('@ulaznicaDetails',' ')
   }

  ngOnInit(): void {
    this.loadUlaznica();
  }
  loadUlaznica() {
    const ulaznicaId = this.activatedRoute.snapshot.paramMap.get('id');
    if (ulaznicaId) this.shopService.getUlaznicaById(+ulaznicaId).subscribe({
      next: ulaznica =>{
        this.ulaznica = ulaznica,
        this.bcService.set('@ulaznicaDetails',ulaznica.izlozba)},
      error: error => console.log(error)
    });
  }


}
