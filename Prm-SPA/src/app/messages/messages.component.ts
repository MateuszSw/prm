import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination, ResultPaginate } from '../_models/pagination';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '../../../node_modules/@angular/router';
import { AlertService } from '../_services/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  cointainer = 'Unread';

  constructor(private service: UserService, private auth: AuthService,
    private route: ActivatedRoute, private alertify: AlertService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data['messages'].score;
      this.pagination = data['messages'].pagination;
    });
  }

  loadMessages() {
    this.service.getMessages(this.auth.token.nameid, this.pagination.currentPage,
      this.pagination.itemsPerPage, this.cointainer)
      .subscribe((score: ResultPaginate<Message[]>) => {
        this.messages = score.score;
        this.pagination = score.pagination;
      }, error => {
        this.alertify.error(error);
      });
  }

  deleteMessage(id: number) {
    this.alertify.confirm('Jesteś pewny że chcesz usunąć wiadomość?', () => {
      this.service.deleteMessage(id, this.auth.token.nameid).subscribe(() => {
        this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        this.alertify.success('Wiadomość usunieta');
      }, error => {
        this.alertify.error('błąd');
      });
    });
  }

  changePage(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

}
