import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { User } from 'src/app/shared/models/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  currentUser$: Observable<User>;
  isCollapsed = true;
  user: User;

  constructor(public accountService: AccountService,
              private router: Router)
{
}

ngOnInit(): void {
this.currentUser$ = this.accountService.currentUser$;
}

logout() {
  this.accountService.logout();
}

}
