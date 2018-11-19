import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { User } from '../../_models/user';
import { BsModalRef, BsModalService } from '../../../../node_modules/ngx-bootstrap';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: User[];
  bsModal: BsModalRef;

  constructor(private adminService: AdminService,
    private modalService: BsModalService) { }

  ngOnInit() {
    this.getRoles();
  }

  getRoles() {
    this.adminService.getRoles().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      console.log(error);
    });
  }

  editRolesModal(user: User) {
    const initialState = {
      user,
      roles: this.getRolesArray(user)
    };
    this.bsModal = this.modalService.show(RolesModalComponent, {initialState});
    this.bsModal.content.updateSelectedRoles.subscribe((values) => {
      const rolesToUpdate = {
        roleNames: [...values.filter(el => el.check === true).map(el => el.name)]
      };
      if (rolesToUpdate) {
        this.adminService.updateRoles(user, rolesToUpdate).subscribe(() => {
          user.roles = [...rolesToUpdate.roleNames];
        }, error => {
          console.log(error);
        });
      }
    });
  }

  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Student', value: 'Student'},
      {name: 'Teacher', value: 'Teacher'},
    ];

    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;
      for (let j = 0; j < userRoles.length; j++) {
        if (availableRoles[i].name === userRoles[j]) {
          isMatch = true;
          availableRoles[i].check = true;
          roles.push(availableRoles[i]);
          break;
        }
      }
      if (!isMatch) {
        availableRoles[i].check = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }

  deleteUser(user: User) {
    this.adminService.deleteUser(user).subscribe(() => {
      this.getRoles();
    }, error => {
      console.log(error);
    });
  }

}
