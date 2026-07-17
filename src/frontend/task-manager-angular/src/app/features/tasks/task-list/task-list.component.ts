import { Component, OnInit } from '@angular/core';
import { TaskService } from '../services/task.service';
import { Task } from '../models/task.model';
import { Router } from '@angular/router';
import { User } from '../../users/models/user.model';
import { UserService } from '../../users/services/user.service';
import { Sort } from '@angular/material/sort';
import { NotificationService } from 'src/app/core/notifications/notification.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {

  tasks: Task[] = [];

  selectedStatus: string = '';
  selectedUserId: string = '';
  users: User[] = [];
  showProgressBar: boolean = true;
  sort: string = 'asc';

  displayedColumns: string[] = [
    'createdAt',
    'title',
    'status',
    'userName',
    'additionalData',
    'actions'
  ];

  constructor(
    private taskService: TaskService,
    private userService: UserService,
    private notifications: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTasks();
    this.loadUsers();
  }

  loadUsers(): void {
    this.showProgressBar = true;
    this.userService.getUsers().subscribe({
      next: data => {
        this.showProgressBar = false;
        this.users = data
      },
      error: err => {
        this.showProgressBar = false;
        console.error(err);
        this.notifications.errorFromResponse(err, 'Unable to load users.', {
          title: 'Users unavailable'
        });
      }
    });
  }

  loadTasks(): void {
    this.showProgressBar = true;
    this.taskService.getTasks(
      this.selectedUserId || undefined,
      this.selectedStatus || undefined,
      this.sort
    ).subscribe({
      next: data => {
        this.tasks = data;
        this.showProgressBar = false;
      },
      error: err => {
        this.showProgressBar = false;
        console.error('Error loading tasks', err);
        this.notifications.errorFromResponse(err, 'Unable to load tasks.', {
          title: 'Tasks unavailable'
        });
      }
    });
  }

  onStatusFilterChange(): void {
    this.loadTasks();
  }

  changeStatus(task: Task, status: number): void {
    this.showProgressBar = true;
    this.taskService.changeStatus(task.id, status)
      .subscribe({
        next: () => {
          this.notifications.success('The task status was updated.', {
            title: 'Task updated'
          });
          this.loadTasks();
        },
        error: err => {
          this.showProgressBar = false;
          console.error('Error changing status', err);
          this.notifications.errorFromResponse(err, 'Error changing status.', {
            title: 'Status change blocked'
          });
        }
      });
  }

  goToTaskCreate(): void {
    this.showProgressBar = true;
    this.router.navigate(['/tasks/new']);
  }

  onSortChange(sort: Sort): void {
    this.sort = sort.direction || 'asc';

    this.loadTasks();
  }

  getTaskCountByStatus(status: string): number {
    return this.tasks.filter(task => task.status === status).length;
  }
}
