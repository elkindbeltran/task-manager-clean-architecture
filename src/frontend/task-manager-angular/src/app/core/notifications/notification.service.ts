import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationSnackbarComponent } from './notification-snackbar.component';
import { NotificationOptions, NotificationType } from './notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly defaultDurationByType: Record<NotificationType, number> = {
    success: 4200,
    info: 5000,
    warning: 6500,
    error: 8000
  };

  constructor(private snackBar: MatSnackBar) {}

  success(message: string, options?: NotificationOptions): void {
    this.show('success', message, options);
  }

  error(message: string, options?: NotificationOptions): void {
    this.show('error', message, options);
  }

  warning(message: string, options?: NotificationOptions): void {
    this.show('warning', message, options);
  }

  info(message: string, options?: NotificationOptions): void {
    this.show('info', message, options);
  }

  errorFromResponse(error: any, fallbackMessage: string, options?: NotificationOptions): void {
    this.error(this.extractErrorMessage(error, fallbackMessage), options);
  }

  private show(type: NotificationType, message: string, options?: NotificationOptions): void {
    this.snackBar.openFromComponent(NotificationSnackbarComponent, {
      data: {
        type,
        message,
        title: options?.title || this.getDefaultTitle(type)
      },
      duration: options?.duration ?? this.defaultDurationByType[type],
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: ['enterprise-snackbar', `enterprise-snackbar-${type}`]
    });
  }

  private extractErrorMessage(error: any, fallbackMessage: string): string {
    if (error?.error?.Errors?.length) {
      return error.error.Errors
        .map((item: any) => `${item.PropertyName}: ${item.ErrorMessage}`)
        .join('\n');
    }

    if (error?.error?.Message) {
      return error.error.Message;
    }

    if (typeof error?.error === 'string') {
      return error.error;
    }

    if (error?.message) {
      return error.message;
    }

    return fallbackMessage;
  }

  private getDefaultTitle(type: NotificationType): string {
    const titleByType: Record<NotificationType, string> = {
      success: 'Success',
      error: 'Action failed',
      warning: 'Attention required',
      info: 'Information'
    };

    return titleByType[type];
  }
}
