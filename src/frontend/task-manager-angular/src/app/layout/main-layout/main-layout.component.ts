import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnInit {

  isAuthenticated = false;

  constructor(
    public auth: AuthService
  ) {}

  ngOnInit(): void {

    this.auth.isAuthenticated$
      .subscribe(isAuthenticated => {
        this.isAuthenticated = isAuthenticated;
      });
  }

  logout() {
    this.auth.logout({
      logoutParams: {
        returnTo: window.location.origin
      }
    });
  }  
}