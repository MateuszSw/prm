import { ArticleDetailComponent } from './articles/article-detail/article-detail.component';
import { ArticleResolver } from './_resolvers/article-edit.resolver';
import { ArticleEditComponent } from './articles/article-edit/article-edit.component';
import { ArticleService } from './_services/article.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule, TabsModule, BsDatepickerModule, PaginationModule, ButtonsModule, ModalModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import {TimeAgoPipe} from 'time-ago-pipe';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorProvider } from './_services/error.interceptor';
import { AlertService } from './_services/alertify.service';
import { UserListComponent } from './users/users-list/user-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { UserCardComponent } from './users/user-card/user-card.component';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { UserMessagesComponent } from './users/user-messages/user-messages.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';

import { AdminService } from './_services/admin.service';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';
import { ArticleListComponent } from './articles/article-list/article-list.component';
import { ArticleListResolver } from './_resolvers/article-list.resolver';
import {ArticleAddComponent } from './articles/article-add/article-add.component';
import { ErrorComponent } from './error/error.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      UserListComponent,
      ListsComponent,
      MessagesComponent,
      UserCardComponent,
      UserDetailComponent,
      UserEditComponent,
      TimeAgoPipe,
      UserMessagesComponent,
      AdminPanelComponent,
      HasRoleDirective,
      UserManagementComponent,
      RolesModalComponent,
      ArticleListComponent,
      ArticleDetailComponent,
      ArticleAddComponent,
      ArticleEditComponent,
      ErrorComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      CKEditorModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      ButtonsModule.forRoot(),
      PaginationModule.forRoot(),
      TabsModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      ModalModule.forRoot(),
      NgxGalleryModule,
      FileUploadModule,
      JwtModule.forRoot({
        config: {
          tokenGetter: tokenGetter,
          whitelistedDomains: ['localhost:5000'],
          blacklistedRoutes: ['localhost:5000/api/auth']
        }
      })
  ],
  providers: [
      AuthService,
      //ErrorProvider,
      AlertService,
      AuthGuard,
      UserService,
      UserDetailResolver,
      UserListResolver,
      UserEditResolver,
      ListsResolver,
      MessagesResolver,
      AdminService,
      ArticleService,
      ArticleListResolver,
      ArticleResolver
    ],
  entryComponents: [
    RolesModalComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
