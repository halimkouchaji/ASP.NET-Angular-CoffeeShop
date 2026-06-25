import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/loginRequest';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {


  loginData: LoginRequest = {
    username: '',
    password: ''
  };


  constructor(
    private authService: AuthService,
    private router: Router
  ){}



  login(){

    this.authService.login(this.loginData)
    .subscribe({

      next:(response)=>{

        console.log(response);

        this.authService.saveToken(response.token);

        this.router.navigate(['/products']);

      },

      error:(err)=>{

        console.error(err);

        alert("Invalid username or password");

      }

    });

  }

}