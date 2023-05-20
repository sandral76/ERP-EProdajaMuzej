import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { UlaznicaStavkaComponent } from './ulaznica-stavka/ulaznica-stavka.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { UlaznicaDetailsComponent } from './ulaznica-details/ulaznica-details.component';
import { ShopRoutingModule } from './shop-routing.module';

@NgModule({
  declarations: [
    ShopComponent,
    UlaznicaStavkaComponent,
    UlaznicaDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ShopRoutingModule
  ]
})
export class ShopModule { }
