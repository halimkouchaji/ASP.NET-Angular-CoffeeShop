import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/Products';
import { CreateProduct } from '../models/CreateProduct';
import { ProductSales } from '../models/ProductSales';
import { UpdateProduct } from '../models/updateProduct';
@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = "https://localhost:7097/api/Products";


  constructor(private http: HttpClient) { }


  getProducts() {
    return this.http.get<Product[]>(this.apiUrl);
  }
  getProductById(id: string) {
  return this.http.get<Product>(`${this.apiUrl}/${id}`);
}
deleteProduct(id: string) {
  return this.http.delete(`${this.apiUrl}/${id}`);
}
addProduct(product: CreateProduct) {
  return this.http.post<Product>(
    'https://localhost:7097/api/Products',
    product
  );
}

searchByName(name: string) {
  return this.http.get<Product[]>(
    `${this.apiUrl}/searchByName?name=${name}`
  );
}
filterByPrice(min: number, max: number) {
  return this.http.get<Product[]>(
    `${this.apiUrl}/filterByPrice?min=${min}&max=${max}`
  );
}
getLowStock(threshold: number) {
  return this.http.get<Product[]>(
    `${this.apiUrl}/lowStock?threshold=${threshold}`
  );
}
updateStock(id: string, quantity: number) {
  return this.http.put<Product>(
    `${this.apiUrl}/updateStock/${id}?quantity=${quantity}`,
    {}
  );
}
sortByPrice(ascending: boolean) {
  return this.http.get<Product[]>(
    `${this.apiUrl}/sortedByPrice?ascending=${ascending}`
  );
}
getMostSold(count: number) {
  return this.http.get<ProductSales[]>(
    `${this.apiUrl}/mostSold?count=${count}`
  );
}
updateProduct(id: string, product: UpdateProduct) {
  return this.http.put<Product>(
    `${this.apiUrl}/${id}`,
    product
  );
}
}