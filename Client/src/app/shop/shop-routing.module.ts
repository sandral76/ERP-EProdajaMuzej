import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from './shop.component';
import { UlaznicaDetailsComponent } from './ulaznica-details/ulaznica-details.component';

const routes:Routes=[
  {path:'',component:ShopComponent},
  {path:':id',component:UlaznicaDetailsComponent,data:{breadcrumb:{alias:'ulaznicaDetails'}}},
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule
  ]
})
export class ShopRoutingModule { }
