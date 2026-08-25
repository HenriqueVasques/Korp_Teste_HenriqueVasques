import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';

import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    InputNumberModule
  ],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  displayDialog: boolean = false;
  isEditMode: boolean = false;
  isSubmitting: boolean = false;
  selectedProductId: any = null;
  deletingProductId: any = null;

  newProduct: Product = {
    productCode: '',
    description: '',
    balance: 0
  };

  constructor(
    private productService: ProductService,
    private cdr: ChangeDetectorRef,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (data) => {
        this.ngZone.run(() => {
          this.products = [...data];
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        console.error('Erro ao carregar produtos:', err);
      }
    });
  }

  showDialog(): void {
    this.isEditMode = false;
    this.selectedProductId = null;
    this.newProduct = { productCode: '', description: '', balance: 0 };
    this.isSubmitting = false;
    this.displayDialog = true;
    this.cdr.detectChanges();
  }

  editProduct(product: any): void {
    this.isEditMode = true;
    this.selectedProductId = product.id ?? product.productId ?? product.productCode;

    this.newProduct = { 
      productCode: product.productCode, 
      description: product.description, 
      balance: product.balance 
    };

    this.isSubmitting = false;
    this.displayDialog = true;
    this.cdr.detectChanges();
  }

  closeDialog(): void {
    this.displayDialog = false;
    this.isSubmitting = false;
    this.cdr.detectChanges();
  }

  saveProduct(): void {
    if (!this.newProduct.productCode || !this.newProduct.description) {
      alert('Preencha todos os campos obrigatórios.');
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    if (this.isEditMode && this.selectedProductId) {
      const updateDto = {
        productCode: this.newProduct.productCode,
        description: this.newProduct.description,
        balance: Number(this.newProduct.balance)
      };

      this.productService.update(this.selectedProductId, updateDto).subscribe({
        next: () => {
          this.ngZone.run(() => {
            this.isSubmitting = false;
            this.displayDialog = false;
            this.loadProducts();
          });
        },
        error: (err) => {
          this.ngZone.run(() => {
            this.isSubmitting = false;
            this.cdr.detectChanges();
            console.error('Erro ao atualizar produto:', err);
            alert(err.error?.message || err.error?.error || 'Erro ao atualizar produto.');
          });
        }
      });
    } else {
      const createPayload = {
        productCode: this.newProduct.productCode,
        description: this.newProduct.description,
        balance: Number(this.newProduct.balance) || 0
      };

      this.productService.create(createPayload as any).subscribe({
        next: () => {
          this.ngZone.run(() => {
            this.isSubmitting = false;
            this.displayDialog = false;
            this.loadProducts();
          });
        },
        error: (err) => {
          this.ngZone.run(() => {
            this.isSubmitting = false;
            this.cdr.detectChanges();
            console.error('Erro ao salvar produto:', err);
            alert(err.error?.message || err.error?.error || 'Erro ao cadastrar produto.');
          });
        }
      });
    }
  }

  deleteProduct(product: any): void {
    const id = product.id ?? product.productId;

    if (!id) {
      alert('Erro: ID numérico do produto não foi encontrado.');
      return;
    }

    if (!confirm(`Deseja realmente excluir o produto "${product.description}"?`)) {
      return;
    }

    this.deletingProductId = id;
    this.cdr.detectChanges();

    this.productService.delete(id).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.products = this.products.filter(p => (p.id ?? p.productId) !== id);
          this.deletingProductId = null;
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.deletingProductId = null;
          this.cdr.detectChanges();
          console.error('Erro ao excluir produto:', err);
          alert(err.error?.error || err.error?.message || 'Erro ao excluir produto.');
        });
      }
    });
  }
}