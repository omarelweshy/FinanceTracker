import client from './client'

export interface TransactionDto {
  id: string
  accountId: string
  accountName: string
  categoryId: string | null
  categoryName: string | null
  type: string
  amount: number
  description: string | null
  transactionDate: string
  createdAt: string
}

export interface PagedResult<T> {
  items: T[]
  total: number
  page: number
  pageSize: number
}

export interface TransactionFilter {
  accountId?: string
  categoryId?: string
  type?: string
  from?: string
  to?: string
  page?: number
  pageSize?: number
}

export const getTransactions = (filter: TransactionFilter = {}) => {
  const params = Object.fromEntries(Object.entries(filter).filter(([, v]) => v !== undefined && v !== ''))
  return client.get<PagedResult<TransactionDto>>('/api/transactions', { params }).then(r => r.data)
}

export const getTransaction = (id: string) =>
  client.get<TransactionDto>(`/api/transactions/${id}`).then(r => r.data)

export const createTransaction = (data: {
  accountId: string
  categoryId: string
  type: string
  amount: number
  transactionDate: string
  description?: string
}) => client.post<TransactionDto>('/api/transactions', data).then(r => r.data)

export const updateTransaction = (id: string, data: {
  amount: number
  categoryId?: string
  transactionDate: string
  description?: string
}) => client.put<TransactionDto>(`/api/transactions/${id}`, data).then(r => r.data)

export const deleteTransaction = (id: string) =>
  client.delete(`/api/transactions/${id}`)
