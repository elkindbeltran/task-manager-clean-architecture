import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { NotificationData } from './notification.model';

@Component({
  selector: 'app-notification-snackbar',
  templateUrl: './notification-snackbar.component.html',
  styleUrls: ['./notification-snackbar.component.css']
})
export class NotificationSnackbarComponent {
  readonly iconByType = {
    success: 'check_circle',
    error: 'error',
    warning: 'warning',
    info: 'info'
  };

  constructor(
    @Inject(MAT_SNACK_BAR_DATA) public data: NotificationData,
    private snackBarRef: MatSnackBarRef<NotificationSnackbarComponent>
  ) {}

  dismiss(): void {
    this.snackBarRef.dismiss();
  }
}
