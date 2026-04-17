import client from './client'

export interface AccountDto {
  id: string
  name: string
  type: string
  balance: number
  currency: string
  isActive: boolean
  createdAt: string
}

export interface AccountSummaryDto {
  totalBalance: number
  currency: string
  activeAccountsCount: number
}

export const getAccounts = () =>
  client.get<AccountDto[]>('/api/accounts').then(r => r.data)

export const getAccountsSummary = () =>
  client.get<AccountSummaryDto>('/api/accounts/summary').then(r => r.data)

export const getAccount = (id: string) =>
  client.get<AccountDto>(`/api/accounts/${id}`).then(r => r.data)

export const createAccount = (data: { name: string; type: string; currency: string }) =>
  client.post<AccountDto>('/api/accounts', data).then(r => r.data)

export const updateAccount = (id: string, data: { name: string; type: string }) =>
  client.put<AccountDto>(`/api/accounts/${id}`, data).then(r => r.data)

export const deleteAccount = (id: string) =>
  client.delete(`/api/accounts/${id}`)
