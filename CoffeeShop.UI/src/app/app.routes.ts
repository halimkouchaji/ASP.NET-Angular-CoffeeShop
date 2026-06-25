import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { ProductsComponent } from './components/products/products';
import { RegisterComponent } from './pages/register/register';
import { OrdersComponent } from './components/order/order';
export const routes: Routes = [

  {
    path: 'login',
    component: LoginComponent
  },
{
  path: 'order',
  component: OrdersComponent
},
  {
    path: 'products',
    component: ProductsComponent
  },

  {
    path: '',
    redirectTo: 'register',
    pathMatch: 'full'
  },
  {
  path: 'register',
  component: RegisterComponent
}

];