import { OrderDetail } from './orderDetail';
export interface Order {

  id: string;          

  orderDate: Date;

  totalPrice: number;

  orderDetails: OrderDetail[];

}