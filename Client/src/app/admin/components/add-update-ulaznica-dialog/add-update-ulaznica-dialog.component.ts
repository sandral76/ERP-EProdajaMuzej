import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ShopService } from 'src/app/shop/shop.service';

export interface UlaznicaAdd {
  ulaznicaId: number | undefined;
  cenaUlaznice: number | undefined;
  izlozbaId: number | string | undefined;
  dostupna: boolean | undefined;
}
@Component({
  selector: 'app-add-update-ulaznica-dialog',
  templateUrl: './add-update-ulaznica-dialog.component.html',
  styleUrls: ['./add-update-ulaznica-dialog.component.scss']
})

export class AddUpdateUlaznicaDialogComponent {
  ulaznicaId: number | undefined;
  cenaUlaznice: number | undefined;
  izlozbaId: number | string | undefined;
  dostupna!: string;
  constructor(public bsModalRef: BsModalRef, public shopService: ShopService, private toastr: ToastrService) { }

  closeModal() {
    this.bsModalRef.hide();
  }

  addUlaznica() {
    const ulaznica = {
      ulaznicaId: this.ulaznicaId,
      cenaUlaznice: this.cenaUlaznice,
      izlozbaId: this.izlozbaId,
      dostupna: JSON.parse(this.dostupna)
    };
    this.shopService.addUlaznica(ulaznica).subscribe(
      response => {
        this.toastr.success('Nova ulaznica dodata!');
        this.closeModal();
      },
      error => {
        console.log('Greska:', error);
      }
    );
  }
  updateUlaznica() {
    if (this.ulaznicaId) {
      const ulaznica = {
        ulaznicaId: this.ulaznicaId,
        cenaUlaznice: this.cenaUlaznice,
        izlozbaId: this.izlozbaId,
        dostupna: JSON.parse(this.dostupna)
      };
      this.shopService.updateUlaznica(ulaznica, this.ulaznicaId).subscribe(
        response => {
          this.toastr.success('Ulaznica ažurirana!');
          this.closeModal();
        },
        error => {
          console.log('Greska:', error);
        }
      );
    }
  }
}

