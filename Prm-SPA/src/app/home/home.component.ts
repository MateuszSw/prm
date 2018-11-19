import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  register = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  toogle() {
    this.register = true;
  }

  cancelRegisterMode(register: boolean) {
    this.register = register;
  }

}
