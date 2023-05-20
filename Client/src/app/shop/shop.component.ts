import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Ulaznica } from '../shared/models/ulaznice';
import { ShopService } from './shop.service';
import { Muzej } from '../shared/models/muzej';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm? : ElementRef;
  ulaznice: Ulaznica[] = [];
  muzejs: Muzej[] = [];
  selectedBrandProducts: Muzej[] = [];
  //grad: string | undefined;
  //sortSelected='name';
  shopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical', value: 'izlozba' },
    { name: 'Price:Low to high', value: 'priceAsc' },
    { name: 'Price:High to low', value: 'priceDesc' }];
  totalCount = 0;
 
  constructor(private shopService: ShopService) { }
  ngOnInit(): void {
    this.getUlaznice();
    this.getMuzejs();
  }
  getUlaznice() {
    this.shopService.getUlaznice(this.shopParams).subscribe({
      next: response => {
        this.ulaznice = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: error => console.log(error)
    })
  }
  getMuzejs() {
    this.shopService.getProductsByGrad().subscribe({
      next: response => this.muzejs = [{ muzejId: 0, grad: 'Svi gradovi' }, ...response],
      error: error => console.log(error)
    })
  }
  onMuzejSelected(grad: string) {
    this.shopParams.pageNumber=1;
    this.shopParams.grad = grad;
    this.shopParams.pageNumber=1;
    this.getU();
    this.shopParams.pageNumber=1;
  }
  getU() {
    this.shopService.getProductsByBrandName(this.shopParams).subscribe({
      next: response => {
        this.ulaznice = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: error => console.log(error)
    })
  }
    
  onSortSelected(event: any) {
    this.shopParams.sort = event.target.value;
    //this.getUlaznice();
    this.getU();
  }
  onPageChanged(event:any){
    if(this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber = event;
      this.getU();
   }

  }
 
  onSearch(){
    this.shopParams.search=this.searchTerm?.nativeElement.value;
    this.shopParams.pageNumber=1;
    this.getU();
  }

  onReset(){
    if(this.searchTerm) this.searchTerm.nativeElement.value='';
    this.shopParams=new ShopParams();
    this.getUlaznice();
  }
}
