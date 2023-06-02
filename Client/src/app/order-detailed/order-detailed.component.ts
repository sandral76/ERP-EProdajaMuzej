import { Component, OnInit } from '@angular/core';
import { Porudzbina } from '../shared/models/porudzbina';
import { OrdersService } from '../orders/orders.service';
import { ActivatedRoute,Router } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { StavkaPorudzbine } from '../shared/models/basket';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order?: Porudzbina;
  stavka?:StavkaPorudzbine;

  constructor(private orderService: OrdersService, private route: ActivatedRoute,
    private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    id && this.orderService.getOrderDetailed(+id).subscribe({
      next: order => {
        this.order = order;
        this.bcService.set('@OrderDetailed', `Porudzbina# ${order.porudzbinaId} - ${order.statusPorudzbine}`);
      }
    })
  }
}
