import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/RegisterRequest';
import { Router } from '@angular/router';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html'
})
export class RegisterComponent {


  registerData: RegisterRequest = {

    username: '',
    password: '',
    roles: ['USER']

  };


  constructor(
    private authService: AuthService,
    private router: Router
  ){}


  register(){

    this.authService.register(this.registerData)
    .subscribe({

     next:(response)=>{

  console.log(response);

  alert("Registered successfully");

  this.router.navigate(['/products']);

},

      error:(err)=>{

        console.error(err);

        alert("Register failed");

      }

    });

  }

}