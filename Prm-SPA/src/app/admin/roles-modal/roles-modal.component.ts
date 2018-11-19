import { OnInit, Output, Component, EventEmitter } from '@angular/core';
import { BsModalRef } from '../../../../node_modules/ngx-bootstrap';
import { User } from '../../_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  user: User;
  @Output() update = new EventEmitter();
  roles: any[];

  constructor(public bsModal: BsModalRef) {}

  ngOnInit() {
  }

  updateRoles() {
    this.update.emit(this.roles);
    this.bsModal.hide();
  }

}
