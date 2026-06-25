export interface OrderDetail {
  id?: string;
  orderId?: string;
  productId: string;
  quantity: number;
  unitPrice: number;
}

export interface CreateOrder {
  id?: string;
  orderDate: string;
  totalPrice: number;
  orderDetails: OrderDetail[];
}