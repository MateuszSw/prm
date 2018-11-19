import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { Pagination, ResultPaginate } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '../../../node_modules/@angular/router';
import { AlertService } from '../_services/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  user: User[];
  pagination: Pagination;


  constructor(private authService: AuthService, private userService: UserService,
    private route: ActivatedRoute, private alertify: AlertService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['users'].score;
      this.pagination = data['users'].pagination;
    });

  }

  loadUsers() {
    this.userService
      .getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null)
      .subscribe((res: ResultPaginate<User[]>) => {
        this.user = res.score;
        this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  changePage(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

}
