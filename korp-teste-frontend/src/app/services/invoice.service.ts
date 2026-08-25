import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateInvoice, InvoiceResponse } from '../models/invoice.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
    
  private readonly apiUrl = 'https://localhost:7059/api/invoice';

  constructor(private http: HttpClient) {}

  getAll(): Observable<InvoiceResponse[]> {
    return this.http.get<InvoiceResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<InvoiceResponse> {
    return this.http.get<InvoiceResponse>(`${this.apiUrl}/${id}`);
  }

  create(invoice: CreateInvoice): Observable<InvoiceResponse> {
    return this.http.post<InvoiceResponse>(this.apiUrl, invoice);
  }


  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  closeInvoice(id: number): Observable<InvoiceResponse> {
    return this.http.put<InvoiceResponse>(`${this.apiUrl}/${id}/close`, {});
  }
}