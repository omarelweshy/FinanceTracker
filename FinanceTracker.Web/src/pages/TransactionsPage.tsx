import { useEffect, useState } from 'react'
import { getTransactions, createTransaction, updateTransaction, deleteTransaction } from '../api/transactions'
import type { TransactionDto, TransactionFilter } from '../api/transactions'
import { getAccounts } from '../api/accounts'
import type { AccountDto } from '../api/accounts'
import { getCategories } from '../api/categories'
import type { CategoryDto } from '../api/categories'

const TRANSACTION_TYPES = ['Income', 'Expense']
const TYPE_COLORS: Record<string, string> = {
  Income: '#27ae60',
  Expense: '#e74c3c',
  TransferIn: '#667eea',
  TransferOut: '#f39c12',
}

const toInputDate = (iso: string) => iso.slice(0, 10)
const today = () => new Date().toISOString().slice(0, 10)

const isTransfer = (type: string) => type === 'TransferIn' || type === 'TransferOut'

export default function TransactionsPage() {
  const [items, setItems] = useState<TransactionDto[]>([])
  const [total, setTotal] = useState(0)
  const [accounts, setAccounts] = useState<AccountDto[]>([])
  const [categories, setCategories] = useState<CategoryDto[]>([])
  const [showForm, setShowForm] = useState(false)
  const [editing, setEditing] = useState<TransactionDto | null>(null)
  const [form, setForm] = useState({
    accountId: '',
    categoryId: '',
    type: 'Expense',
    amount: '',
    transactionDate: today(),
    description: '',
  })
  const [filter, setFilter] = useState<TransactionFilter>({ page: 1, pageSize: 20 })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const load = (f = filter) =>
    getTransactions(f).then(r => { setItems(r.items); setTotal(r.total) }).catch(() => {})

  useEffect(() => {
    load()
    getAccounts().then(setAccounts).catch(() => {})
    getCategories().then(setCategories).catch(() => {})
  }, [])

  const filteredCategories = categories.filter(c => c.type === form.type)

  const openCreate = () => {
    setEditing(null)
    setForm({ accountId: accounts[0]?.id ?? '', categoryId: '', type: 'Expense', amount: '', transactionDate: today(), description: '' })
    setError('')
    setShowForm(true)
  }

  const openEdit = (t: TransactionDto) => {
    setEditing(t)
    setForm({
      accountId: t.accountId,
      categoryId: t.categoryId ?? '',
      type: t.type,
      amount: String(t.amount),
      transactionDate: toInputDate(t.transactionDate),
      description: t.description ?? '',
    })
    setError('')
    setShowForm(true)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      if (editing) {
        await updateTransaction(editing.id, {
          amount: parseFloat(form.amount),
          categoryId: form.categoryId || undefined,
          transactionDate: new Date(form.transactionDate).toISOString(),
          description: form.description || undefined,
        })
      } else {
        await createTransaction({
          accountId: form.accountId,
          categoryId: form.categoryId,
          type: form.type,
          amount: parseFloat(form.amount),
          transactionDate: new Date(form.transactionDate).toISOString(),
          description: form.description || undefined,
        })
      }
      setShowForm(false)
      load()
    } catch (err: any) {
      setError(err.response?.data?.message || err.response?.data?.errors?.map((e: any) => e.message).join(', ') || 'Failed.')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!confirm('Delete this transaction?')) return
    await deleteTransaction(id).catch(() => {})
    load()
  }

  const applyFilter = (updates: Partial<TransactionFilter>) => {
    const next = { ...filter, ...updates, page: 1 }
    setFilter(next)
    load(next)
  }

  const goPage = (page: number) => {
    const next = { ...filter, page }
    setFilter(next)
    load(next)
  }

  const totalPages = Math.max(1, Math.ceil(total / (filter.pageSize ?? 20)))

  return (
    <div className="page">
      <div className="page-header">
        <h1>Transactions</h1>
        <button className="btn-primary" onClick={openCreate}>+ New Transaction</button>
      </div>

      <div className="filter-bar">
        <select value={filter.accountId ?? ''} onChange={e => applyFilter({ accountId: e.target.value || undefined })}>
          <option value="">All Accounts</option>
          {accounts.map(a => <option key={a.id} value={a.id}>{a.name}</option>)}
        </select>
        <select value={filter.type ?? ''} onChange={e => applyFilter({ type: e.target.value || undefined })}>
          <option value="">All Types</option>
          <option value="Income">Income</option>
          <option value="Expense">Expense</option>
          <option value="TransferIn">Transfer In</option>
          <option value="TransferOut">Transfer Out</option>
        </select>
        <select value={filter.categoryId ?? ''} onChange={e => applyFilter({ categoryId: e.target.value || undefined })}>
          <option value="">All Categories</option>
          {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
        </select>
        <input type="date" value={filter.from ?? ''} onChange={e => applyFilter({ from: e.target.value || undefined })} placeholder="From" />
        <input type="date" value={filter.to ?? ''} onChange={e => applyFilter({ to: e.target.value || undefined })} placeholder="To" />
      </div>

      {showForm && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Transaction' : 'New Transaction'}</h2>
            {error && <div className="error-msg">{error}</div>}
            <form onSubmit={handleSubmit}>
              {!editing && (
                <div className="form-group">
                  <label>Account</label>
                  <select value={form.accountId} onChange={e => setForm({ ...form, accountId: e.target.value })} required>
                    <option value="">Select account</option>
                    {accounts.map(a => <option key={a.id} value={a.id}>{a.name} ({a.currency})</option>)}
                  </select>
                </div>
              )}
              {!editing && (
                <div className="form-group">
                  <label>Type</label>
                  <select value={form.type} onChange={e => setForm({ ...form, type: e.target.value, categoryId: '' })}>
                    {TRANSACTION_TYPES.map(t => <option key={t} value={t}>{t}</option>)}
                  </select>
                </div>
              )}
              <div className="form-group">
                <label>Category</label>
                <select value={form.categoryId} onChange={e => setForm({ ...form, categoryId: e.target.value })}>
                  <option value="">No category</option>
                  {(editing ? categories : filteredCategories).map(c => (
                    <option key={c.id} value={c.id}>{c.icon ? `${c.icon} ` : ''}{c.name}</option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Amount</label>
                <input
                  type="number"
                  min="0.01"
                  step="0.01"
                  value={form.amount}
                  onChange={e => setForm({ ...form, amount: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Date</label>
                <input
                  type="date"
                  value={form.transactionDate}
                  onChange={e => setForm({ ...form, transactionDate: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Description</label>
                <input value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn-primary" disabled={loading}>{loading ? 'Saving...' : 'Save'}</button>
                <button type="button" className="btn-secondary" onClick={() => setShowForm(false)}>Cancel</button>
              </div>
            </form>
          </div>
        </div>
      )}

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Account</th>
              <th>Category</th>
              <th>Type</th>
              <th>Amount</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 && <tr><td colSpan={7} className="empty">No transactions yet.</td></tr>}
            {items.map(t => (
              <tr key={t.id}>
                <td>{toInputDate(t.transactionDate)}</td>
                <td>{t.accountName}</td>
                <td>{t.categoryName ?? <span style={{ color: '#aaa' }}>—</span>}</td>
                <td>
                  <span className="badge" style={{ background: `${TYPE_COLORS[t.type]}18`, color: TYPE_COLORS[t.type] }}>
                    {t.type}
                  </span>
                </td>
                <td className={t.type === 'Expense' || t.type === 'TransferOut' ? 'negative' : 'positive'}>
                  {t.type === 'Expense' || t.type === 'TransferOut' ? '-' : '+'}{t.amount.toFixed(2)}
                </td>
                <td>{t.description ?? <span style={{ color: '#aaa' }}>—</span>}</td>
                <td>
                  {!isTransfer(t.type) && (
                    <>
                      <button className="btn-icon" onClick={() => openEdit(t)}>Edit</button>
                      <button className="btn-icon danger" onClick={() => handleDelete(t.id)}>Delete</button>
                    </>
                  )}
                  {isTransfer(t.type) && <span style={{ color: '#aaa', fontSize: '0.8rem' }}>via transfer</span>}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {totalPages > 1 && (
        <div className="pagination">
          <button className="btn-secondary" onClick={() => goPage((filter.page ?? 1) - 1)} disabled={(filter.page ?? 1) <= 1}>← Prev</button>
          <span>Page {filter.page ?? 1} of {totalPages} ({total} total)</span>
          <button className="btn-secondary" onClick={() => goPage((filter.page ?? 1) + 1)} disabled={(filter.page ?? 1) >= totalPages}>Next →</button>
        </div>
      )}
    </div>
  )
}
