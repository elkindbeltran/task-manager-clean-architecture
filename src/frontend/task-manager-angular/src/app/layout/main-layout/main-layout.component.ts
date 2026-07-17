import { Component, HostListener, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnInit {

  isAuthenticated = false;
  isNavigationCollapsed = false;

  constructor(
    public auth: AuthService
  ) {}

  ngOnInit(): void {
    this.syncNavigationState();

    this.auth.isAuthenticated$
      .subscribe(isAuthenticated => {
        this.isAuthenticated = isAuthenticated;
      });
  }

  @HostListener('window:resize')
  onWindowResize(): void {
    this.syncNavigationState();
  }

  logout() {
    this.auth.logout({
      logoutParams: {
        returnTo: window.location.origin
      }
    });
  }

  toggleNavigation(): void {
    this.isNavigationCollapsed = !this.isNavigationCollapsed;
  }

  closeNavigationOnSmallScreen(): void {
    if (window.innerWidth <= 900) {
      this.isNavigationCollapsed = true;
    }
  }

  private syncNavigationState(): void {
    if (window.innerWidth <= 900) {
      this.isNavigationCollapsed = true;
    }
  }
}
