import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../services/task.service';
import { Router } from '@angular/router';
import { UserService } from '../../users/services/user.service';
import { User } from '../../users/models/user.model';
import { NotificationService } from 'src/app/core/notifications/notification.service';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {

  form!: FormGroup;
  loading = false;
  users: User[] = [];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private userService: UserService,
    private notifications: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', Validators.required],
      additionalData: [''],
      userId: ['', Validators.required]
    });

    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: data => this.users = data,
      error: err => {
        console.error('Error loading users', err);
        this.notifications.errorFromResponse(err, 'Error loading users.', {
          title: 'Users unavailable'
        });
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.notifications.warning('Complete the required fields before creating the task.', {
        title: 'Validation required'
      });
      return;
    }

    this.loading = true;

    if (this.form.get('additionalData')?.value === '')
      this.form.get('additionalData')?.setValue('{}');

    this.taskService.createTask(this.form.value)
      .subscribe({
        next: () => {
          this.loading = false;
          this.notifications.success('The task was created successfully.', {
            title: 'Task created'
          });
          this.router.navigate(['/tasks']);
        },
        error: err => {
          this.loading = false;
          console.error('Error creating task', err);
          this.notifications.errorFromResponse(err, 'Error creating task.', {
            title: 'Task not created'
          });
        }
      });
  }

  goToTaskList(): void {
    this.router.navigate(['/tasks']);
  }

  get selectedUser(): User | undefined {
    const selectedUserId = this.form?.get('userId')?.value;

    return this.users.find(user => user.id === selectedUserId);
  }
}
