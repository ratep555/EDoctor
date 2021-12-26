import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { SharedModule } from '../shared/shared.module';
import { AccountRoutingModule } from './account-routing.module';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    EmailConfirmationComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent
    ],
  imports: [
    CommonModule,
    SharedModule,
    AccountRoutingModule
  ]
})
export class AccountModule { }
