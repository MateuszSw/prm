import { Component, OnInit, Input } from '@angular/core';
import { Message } from '../../_models/message';
import { UserService } from '../../_services/user.service';
import { AuthService } from '../../_services/auth.service';
import { AlertService } from '../../_services/alertify.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-user-messages',
  templateUrl: './user-messages.component.html',
  styleUrls: ['./user-messages.component.css']
})
export class UserMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: Message[];
  createMessage: any = {};

  constructor(private service: UserService, private auth: AuthService,
      private alertify: AlertService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = +this.auth.token.nameid;
    this.service.getConversation(this.auth.token.nameid, this.recipientId)
      .pipe(
        tap(messages => {
          for (let i = 0; i < messages.length; i++) {
            if (messages[i].isRead === false && messages[i].recipientId === currentUserId) {
              this.service.read(currentUserId, messages[i].id);
            }}})
      )
      .subscribe(messages => {
        this.messages = messages;
    }, error => {
      this.alertify.error(error);
    });
  }

  sendMessage() {
    this.createMessage.recipientId = this.recipientId;
    this.service.sendMessage(this.auth.token.nameid, this.createMessage)
      .subscribe((message: Message) => {
        this.messages.unshift(message);
        this.createMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }

}
