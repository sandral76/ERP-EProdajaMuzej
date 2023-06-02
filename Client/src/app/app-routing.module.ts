import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './core/admin/admin.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminUlaznicaComponent } from './admin/components/admin-ulaznica/admin-ulaznica.component';

const routes: Routes = [
  { path: '', component: HomeComponent, data: { breadcrumb: 'Pocetna' } },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    component: AdminComponent,
    children:[
      {
        path:"ulaznica",
        component:AdminUlaznicaComponent
      }
    ]
    
  },
  //{ path: 'not-found', component: NotFoundComponent},
  //{ path: 'server-error', component: ServerErrorComponent},
  { path: 'shop', loadChildren: () => import('./shop/shop.module').then(m => m.ShopModule) },
  { path: 'basket', loadChildren: () => import('./basket/basket.module').then(m => m.BasketModule) },
  {
    path: 'checkout',
    canActivate: [AuthGuard],
    loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule)
  },
  { path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule), data: { breadcrumb: { skip: true } } },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
  {
    path: 'orders',
    canActivate: [AuthGuard],
    loadChildren: () => import('./orders/orders.module').then(mod => mod.OrdersModule),
    data: { breadcrumb: 'Porudzbine' }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
