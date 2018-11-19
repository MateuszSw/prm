import { ErrorComponent } from './error/error.component';
import { ArticleResolver } from './_resolvers/article-edit.resolver';
import { ArticleEditComponent } from './articles/article-edit/article-edit.component';
import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserListComponent } from './users/users-list/user-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { ArticleListComponent } from './articles/article-list/article-list.component';
import { ArticleListResolver } from './_resolvers/article-list.resolver';
import { ArticleAddComponent } from './articles/article-add/article-add.component';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { ArticleDetailComponent } from './articles/article-detail/article-detail.component';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path: 'error', component: ErrorComponent},
            {path: 'users', component: UserListComponent,
                resolve: {users: UserListResolver}},
            {path: 'users/:id', component: UserDetailComponent,
                resolve: {user: UserDetailResolver}},
            {path: 'user/edit', component: UserEditComponent,
                resolve: {user: UserEditResolver}},
                {path: 'articles', component: ArticleListComponent,
                resolve: {articles: ArticleListResolver}},
                {path: 'articles/add', component: ArticleAddComponent},
                {path: 'articles/edit/:id', component: ArticleEditComponent,
                    resolve: {article: ArticleResolver}},
                {path: 'articles/:id', component: ArticleDetailComponent,
                resolve: {article: ArticleResolver}},
            {path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
            {path: 'lists', component: ListsComponent, resolve: {users: ListsResolver}},
            {path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin']}},
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
