import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/core/notifications/notification.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {

  form!: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private notifications: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.notifications.warning('Enter a valid name and email before creating the user.', {
        title: 'Validation required'
      });
      return;
    }

    this.loading = true;

    this.userService.createUser(this.form.value)
      .subscribe({
        next: () => {
          this.loading = false;
          this.notifications.success('The user was created successfully.', {
            title: 'User created'
          });
          this.router.navigate(['/users']);
        },
        error: err => {
          this.loading = false;
          console.error('Error creating user', err);
          this.notifications.errorFromResponse(err, 'Error creating user.', {
            title: 'User not created'
          });
        }
      });
  }

  goToUsersList(): void {
    this.router.navigate(['/users']);
  }

}
