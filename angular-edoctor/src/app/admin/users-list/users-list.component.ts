import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/shared/models/user';
import { MyParams } from 'src/app/shared/models/userparams';
import { AdminService } from '../admin.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { RolesModalComponent } from 'src/app/shared/components/roles-modal/roles-modal.component';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  users: Partial<User[]>;
  bsModalRef: BsModalRef;
  myParams = new MyParams();
  totalCount: number;

  constructor(private adminService: AdminService,
              private modalService: BsModalService,
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.adminService.getUsers(this.myParams)
    .subscribe(response => {
      this.users = response.data;
      this.myParams.page = response.page;
      this.myParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.myParams.query = this.searchTerm.nativeElement.value;
    this.getUsers();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.getUsers();
  }

  onPageChanged(event: any) {
    if (this.myParams.page !== event) {
      this.myParams.page = event;
      this.getUsers();
    }
}

openRolesModal(user: User) {
  const config = {
    class: 'modal-dialog-centered',
    initialState: {
      user,
      roles: this.getRolesArray(user)
    }
  };
  this.bsModalRef = this.modalService.show(RolesModalComponent, config);
  this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
    const rolesToUpdate = {
      roles: [...values.filter(el => el.checked === true).map(el => el.name)]
    };
    if (rolesToUpdate) {
      this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe(() => {
        user.roles = [...rolesToUpdate.roles];
      });
    }
  });
}

private getRolesArray(user) {
  const roles = [];
  const userRoles = user.roles;
  const availableRoles: any[] = [
    {name: 'Admin', value: 'Admin'},
    {name: 'Doctor', value: 'Doctor'},
    {name: 'Patient', value: 'Patient'}
  ];
  availableRoles.forEach(role => {
    let isMatch = false;
    for (const userRole of userRoles) {
      if (role.name === userRole) {
        isMatch = true;
        role.checked = true;
        roles.push(role);
        break;
      }
    }
    if (!isMatch) {
      role.checked = false;
      roles.push(role);
    }
  });
  return roles;
}

unlockUser(userId: number) {
  Swal.fire({
    title: 'Are you sure you want to unlock this user?',
    text: 'You can always lock it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, unlock it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, forget about it'
  }).then((result) => {
    if (result.value) {
    this.adminService.unlockUser(userId)
      .subscribe(
        res => {
          this.getUsers();
          this.toastr.success('User Unlocked!!');
        },
        err => { console.log(err);
        });
      }
    });
    }

lockUser(userId: number) {
  Swal.fire({
    title: 'Are you sure want to lock this user?',
    text: 'You can always unlock it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, lock it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, forget about it'
  }).then((result) => {
    if (result.value) {
    this.adminService.lockUser(userId)
      .subscribe(
        res => {
          this.getUsers();
          this.toastr.error('User Locked!!');
        },
        err => { console.log(err);
        });
      }
    });
    }

}
