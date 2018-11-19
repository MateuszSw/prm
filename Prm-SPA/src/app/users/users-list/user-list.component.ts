import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, ResultPaginate } from '../../_models/pagination';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  statusList = [{value: 'Teacher', display: 'Wykładowcy'}, {value: 'Student', display: 'Studenci'}];
  userParams: any = {};
  pagination: Pagination;

  constructor(private service: UserService,  private route: ActivatedRoute, private alertify: AlertService,
   ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].score;
      this.pagination = data['users'].pagination;
    });

    this.userParams.status = this.user.status === 'Student' ? 'Teacher' : 'Student';
    this.userParams.orderBy = 'lastActive';
  }

  changePage(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.service.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: ResultPaginate<User[]>) => {
        this.users = res.score;
        this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
