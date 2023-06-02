import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.scss']
})
export class SideBarComponent implements OnInit{
  sidebarlist:any=[
    {
      label:"Ulaznica",
      link:"/ulaznica",
      active:true
    },
    {
      label:"Izlozba",
      link:"/izlozba",
      active:false
    },
    {
      label:"Muzej",
      link:"/muzej",
      active:false
    }
  ]

  changeRoute(index:number){
    this.sidebarlist.forEach((item:any,i:number) => {
      this.sidebarlist[i].active=false;
    });
    this.sidebarlist[index].active=true;
  }
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

}
