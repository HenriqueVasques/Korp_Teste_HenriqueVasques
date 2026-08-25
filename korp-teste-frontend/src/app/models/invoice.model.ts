export enum InvoiceStatus {
  Open = 1,
  Closed = 2
}

// Interface usada na CRIAÇÃO (exige a descrição)
export interface InvoiceItemCreateDto {
  productCode: string;
  description: string;
  quantity: number;
}

// Interface usada na RESPOSTA (retorna dados completos do backend)
export interface InvoiceItemResponse {
  id?: number;
  productCode: string;
  description: string;
  quantity: number;
  unitPrice?: number;
  total?: number;
}

export interface CreateInvoice {
  items: InvoiceItemCreateDto[];
}

export interface InvoiceResponse {
  id: number;
  number: number;
  status: InvoiceStatus;
  items: InvoiceItemResponse[];
  issueDate?: string;
  createdAt?: string;
}