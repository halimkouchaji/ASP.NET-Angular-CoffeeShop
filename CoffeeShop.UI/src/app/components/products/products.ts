import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, JsonPipe } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/Products';
import { FormsModule } from '@angular/forms';
import { CreateProduct } from '../../models/CreateProduct';
import { RouterLink } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { CreateOrder } from '../../models/orderDetail';

export interface OrderItem {
  product: Product;
  quantity: number;
}
@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, JsonPipe, FormsModule, RouterLink],
  templateUrl: './products.html'
})
export class ProductsComponent implements OnInit {

  products: Product[] = [];
selectedProducts: OrderItem[] = [];
  constructor(
  private productService: ProductService,
  private orderService: OrderService,
  private cdr: ChangeDetectorRef
) {}
  ngOnInit() {
    this.productService.getProducts()
      .subscribe({
        next: (data) => {
          this.products = data;
          this.cdr.detectChanges();
          console.log('AFTER ASSIGN:', this.products.length);
        }
      });
  }
newProduct: CreateProduct = {
  name: '',
  price: 0,
  description: ''
};
searchName: string = '';

minPrice: number = 0;
maxPrice: number = 100;

threshold: number = 5;

ascending: boolean = true;

mostSoldCount: number = 5;
addProduct() {
  this.productService.addProduct(this.newProduct)
    .subscribe({
      next: (product) => {
        this.products.push(product);

        this.newProduct = {
          name: '',
          price: 0,
          description: ''
        };
      },
      error: (err) => console.error(err)
    });
}
  deleteProduct(id: string) {
    this.productService.deleteProduct(id)
      .subscribe({
        next: () => {
          this.products = this.products.filter(
            p => p.id !== id
          );
        },
        error: (err) => {
          console.error(err);
        }
      });
  }
  searchProducts() {
  this.productService.searchByName(this.searchName)
    .subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (err) => console.error(err)
    });
}
filterProducts() {
  this.productService
    .filterByPrice(this.minPrice, this.maxPrice)
    .subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (err) => console.error(err)
    });
}
addToOrder(product: Product) {

  const existing = this.selectedProducts.find(
    x => x.product.id === product.id
  );

  if (existing) {
    existing.quantity++;
  }
  else {
    this.selectedProducts.push({
      product: product,
      quantity: 1
    });
  }
}
increaseQuantity(item: OrderItem) {
  item.quantity++;
}
decreaseQuantity(item: OrderItem) {

  if (item.quantity > 1) {
    item.quantity--;
  }
}
removeFromOrder(item: OrderItem) {

  this.selectedProducts =
    this.selectedProducts.filter(
      x => x.product.id !== item.product.id
    );
}

getLowStock() {
  this.productService
    .getLowStock(this.threshold)
    .subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (err) => console.error(err)
    });
}
sortProducts() {
  this.productService
    .sortByPrice(this.ascending)
    .subscribe({
      next: (data) => {
        this.products = data;
      }
    });
}
createOrder() {

  const order: CreateOrder = {

    orderDate: new Date().toISOString(),

    totalPrice: this.selectedProducts.reduce(
      (sum, item) =>
        sum + (item.product.price * item.quantity),
      0
    ),

    orderDetails: this.selectedProducts.map(item => ({
      productId: item.product.id,
      quantity: item.quantity,
      unitPrice: item.product.price
    }))
  };

  this.orderService.createOrder(order)
    .subscribe({
      next: (response) => {

        console.log('Order Created', response);

        alert('Order Created Successfully');

        this.selectedProducts = [];
      },

      error: (err) => {
        console.error(err);
      }
    });
}
}