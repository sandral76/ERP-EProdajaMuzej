import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { Ulaznica } from 'src/app/shared/models/ulaznice';
import { ShopService } from 'src/app/shop/shop.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AddUpdateUlaznicaDialogComponent } from '../add-update-ulaznica-dialog/add-update-ulaznica-dialog.component';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-admin-ulaznica',
  templateUrl: './admin-ulaznica.component.html',
  styleUrls: ['./admin-ulaznica.component.scss']
})
export class AdminUlaznicaComponent implements OnInit {
  @ViewChild('deleteConfirmationModal') deleteConfirmationModal!: TemplateRef<any>;
  dataSource: Ulaznica[]=[];
  subscription!: Subscription;
  bsModalRef: BsModalRef | undefined;
  selectedUlaznica: Ulaznica | undefined;
  constructor(private shopService: ShopService,private modalService: BsModalService,private toastr: ToastrService){}

  openModal() {
    this.bsModalRef = this.modalService.show(AddUpdateUlaznicaDialogComponent);
  }
  openEditModal(item: Ulaznica) {
    const dostupnaValue = item.dostupna ? "true" : "false";
    this.bsModalRef = this.modalService.show(AddUpdateUlaznicaDialogComponent, {
      initialState: {
        ulaznicaId: item.ulaznicaId,
        cenaUlaznice: item.cenaUlaznice,
        izlozbaId: item.izlozba,
        dostupna:dostupnaValue
      }
    });
  }
  confirmDelete(element: Ulaznica) {
    this.selectedUlaznica = element;
    this.bsModalRef = this.modalService.show(this.deleteConfirmationModal);
  }
  deleteUlaznica() {
    if (this.selectedUlaznica) {
      this.shopService.deleteUlaznica(this.selectedUlaznica.ulaznicaId).subscribe(
        response => {
          this.toastr.success('Ulaznica ažurirana!');
          this.loadData();
        },
        error => {
          this.toastr.error('Ne mozete izbrisati ovu ulaznicu, referencirana je u drugim podacima.');
        }
      );
    }

    this.bsModalRef?.hide();
  }
  closeDeleteConfirmationModal() {
    this.bsModalRef?.hide();
  }
  closeModal() {
    this.bsModalRef?.hide();
  }
  ngOnInit(): void {
    this.loadData();
  }
  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
  public loadData() {
    this.subscription = this.shopService.getUlazniceNoParams().subscribe(
      (response: any) => {
        const data = response.data; // Access the 'data' property of the response
        this.dataSource = data;
      },
    
    );
  }
  
}

