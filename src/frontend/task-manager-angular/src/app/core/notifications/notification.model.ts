export type NotificationType = 'success' | 'error' | 'warning' | 'info';

export interface NotificationData {
  title: string;
  message: string;
  type: NotificationType;
}

export interface NotificationOptions {
  title?: string;
  duration?: number;
}
