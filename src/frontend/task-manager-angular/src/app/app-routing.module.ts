import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { AuthGuard } from '@auth0/auth0-angular';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'tasks',
        pathMatch: 'full'
      },
      {
        path: 'tasks',
        loadChildren: () =>
          import('./features/tasks/tasks.module').then(m => m.TasksModule)
      },
      {
        path: 'users',
        loadChildren: () =>
          import('./features/users/users.module').then(m => m.UsersModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
