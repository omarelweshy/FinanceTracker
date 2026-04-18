import client from './client'
import type { PagedResult } from './transactions'

export interface TransferDto {
  id: string
  fromTransactionId: string
  toTransactionId: string
  fromAccountId: string
  fromAccountName: string
  toAccountId: string
  toAccountName: string
  amount: number
  note: string | null
  createdAt: string
}

export const getTransfers = (page = 1, pageSize = 20) =>
  client.get<PagedResult<TransferDto>>('/api/transfers', { params: { page, pageSize } }).then(r => r.data)

export const createTransfer = (data: {
  fromAccountId: string
  toAccountId: string
  amount: number
  transferDate: string
  note?: string
}) => client.post<TransferDto>('/api/transfers', data).then(r => r.data)
