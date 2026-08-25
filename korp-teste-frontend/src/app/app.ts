import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterModule, RouterLinkActive } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet, 
    RouterModule, 
    RouterLinkActive, 
    MenubarModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  items: MenuItem[] = [];

  ngOnInit() {
    this.items = [
      {
        label: 'Estoque',
        icon: 'pi pi-fw pi-box',
        routerLink: '/products'
      },
      {
        label: 'Faturamento',
        icon: 'pi pi-fw pi-file-edit',
        routerLink: '/invoices'
      }
    ];
  }
}