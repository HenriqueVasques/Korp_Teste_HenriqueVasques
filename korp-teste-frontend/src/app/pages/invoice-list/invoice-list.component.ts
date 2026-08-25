import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TagModule } from 'primeng/tag';

import { InvoiceService } from '../../services/invoice.service';
import { CreateInvoice, InvoiceResponse, InvoiceStatus } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    TagModule
  ],
  templateUrl: './invoice-list.component.html',
  styleUrl: './invoice-list.component.css'
})
export class InvoiceListComponent implements OnInit {
  invoices: InvoiceResponse[] = [];
  displayDialog: boolean = false;
  isSubmitting: boolean = false;
  printingInvoiceId: number | null = null;
  deletingInvoiceId: number | null = null;
  InvoiceStatus = InvoiceStatus;

  newInvoice: CreateInvoice = {
    items: [{ productCode: '', description: '', quantity: 1 }]
  };

  constructor(
    private invoiceService: InvoiceService,
    private cdr: ChangeDetectorRef,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.invoiceService.getAll().subscribe({
      next: (data) => {
        this.ngZone.run(() => {
          this.invoices = data;
          this.cdr.markForCheck();
        });
      },
      error: (err) => console.error('Erro ao carregar faturas:', err)
    });
  }

  showDialog(): void {
    this.newInvoice = {
      items: [{ productCode: '', description: '', quantity: 1 }]
    };
    this.isSubmitting = false;
    this.displayDialog = true;
    this.cdr.markForCheck();
  }

  addItem(): void {
    this.newInvoice.items.push({ productCode: '', description: '', quantity: 1 });
  }

  removeItem(index: number): void {
    if (this.newInvoice.items.length > 1) {
      this.newInvoice.items.splice(index, 1);
    }
  }

  saveInvoice(): void {
    if (this.isSubmitting) return;

    if (this.newInvoice.items.some(i => !i.productCode || !i.description)) {
      alert('Preencha o código e a descrição de todos os produtos.');
      return;
    }

    const payload = {
      items: this.newInvoice.items.map(item => ({
        productCode: item.productCode,
        description: item.description,
        quantity: Number(item.quantity) || 1
      }))
    };

    this.isSubmitting = true;

    this.invoiceService.create(payload as any).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.isSubmitting = false;
          this.displayDialog = false;
          this.loadInvoices();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.isSubmitting = false;
          this.cdr.markForCheck();
        });
        console.error('Erro ao criar fatura:', err);
        const errorMsg = err.error?.errors 
          ? JSON.stringify(err.error.errors) 
          : (err.error?.message || err.error?.detail || 'Erro ao criar fatura');
        alert(errorMsg);
      }
    });
  }

  printInvoice(id: number): void {
    this.printingInvoiceId = id;
    this.cdr.markForCheck();

    this.invoiceService.closeInvoice(id).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.printingInvoiceId = null;
          this.loadInvoices();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.printingInvoiceId = null;
          this.cdr.markForCheck();
        });
        console.error('Erro ao imprimir e fechar fatura:', err);
        alert(err.error?.error || 'Erro ao processar impressão e abater estoque');
      }
    });
  }

  deleteInvoice(id: number): void {
    if (!confirm('Tem certeza que deseja excluir esta fatura?')) {
      return;
    }

    this.deletingInvoiceId = id;
    this.cdr.markForCheck();

    this.invoiceService.delete(id).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.deletingInvoiceId = null;
          this.loadInvoices();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.deletingInvoiceId = null;
          this.cdr.markForCheck();
        });
        console.error('Erro ao excluir fatura:', err);
        alert(err.error?.message || err.error?.error || 'Erro ao excluir fatura');
      }
    });
  }
}