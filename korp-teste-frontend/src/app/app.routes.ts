import { Routes } from '@angular/router';
import { ProductListComponent } from './pages/product-list/product-list.component';
import { InvoiceListComponent } from './pages/invoice-list/invoice-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'products', pathMatch: 'full' },
  { path: 'products', component: ProductListComponent },
  { path: 'invoices', component: InvoiceListComponent }
];