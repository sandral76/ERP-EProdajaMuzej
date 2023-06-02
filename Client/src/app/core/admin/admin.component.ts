import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent {
  baseUrl = environment.apiUrl;
  validationErrors: string[] = [];

  constructor(private http: HttpClient) { }

  get404Error() {
    this.http.get(this.baseUrl + 'ulaznica/102').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
  get400Error() {
    this.http.get(this.baseUrl + 'ulaznica/k').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
  get400ValidationError() {
    this.http.get(this.baseUrl + 'ulaznica/k').subscribe({
      next: response => console.log(response),
      error: error => {
        console.error(error);
        this.validationErrors = error.errors;
      }
    })
  }
  get500Error() {
    this.http.delete(this.baseUrl + 'ulaznica/1').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
}
