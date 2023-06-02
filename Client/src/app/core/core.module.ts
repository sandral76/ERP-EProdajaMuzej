import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { BreadcrumbModule} from 'xng-breadcrumb';
import { NgxSpinnerComponent, NgxSpinnerModule } from 'ngx-spinner';
import { SharedModule } from '../shared/shared.module';
import { SideBarComponent } from './side-bar/side-bar.component';


@NgModule({
  declarations: [
    NavBarComponent,
    AdminComponent,
    NotFoundComponent,
    ServerErrorComponent,
    SectionHeaderComponent,
    SideBarComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right',
      preventDuplicates:true
    }),
    BreadcrumbModule,
    NgxSpinnerModule,
    SharedModule,
  ],
  exports:[
    NavBarComponent,
    SectionHeaderComponent,
    NgxSpinnerComponent
  ]
})
export class CoreModule { }
