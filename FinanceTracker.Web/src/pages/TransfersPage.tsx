import { useEffect, useState } from 'react'
import { getTransfers, createTransfer } from '../api/transfers'
import type { TransferDto } from '../api/transfers'
import { getAccounts } from '../api/accounts'
import type { AccountDto } from '../api/accounts'

const toInputDate = (iso: string) => iso.slice(0, 10)
const today = () => new Date().toISOString().slice(0, 10)

export default function TransfersPage() {
  const [items, setItems] = useState<TransferDto[]>([])
  const [total, setTotal] = useState(0)
  const [page, setPage] = useState(1)
  const pageSize = 20
  const [accounts, setAccounts] = useState<AccountDto[]>([])
  const [showForm, setShowForm] = useState(false)
  const [form, setForm] = useState({ fromAccountId: '', toAccountId: '', amount: '', transferDate: today(), note: '' })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const load = (p = page) =>
    getTransfers(p, pageSize).then(r => { setItems(r.items); setTotal(r.total) }).catch(() => {})

  useEffect(() => {
    load()
    getAccounts().then(setAccounts).catch(() => {})
  }, [])

  const openCreate = () => {
    setForm({ fromAccountId: accounts[0]?.id ?? '', toAccountId: accounts[1]?.id ?? '', amount: '', transferDate: today(), note: '' })
    setError('')
    setShowForm(true)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (form.fromAccountId === form.toAccountId) {
      setError('Source and destination accounts must be different.')
      return
    }
    setError('')
    setLoading(true)
    try {
      await createTransfer({
        fromAccountId: form.fromAccountId,
        toAccountId: form.toAccountId,
        amount: parseFloat(form.amount),
        transferDate: new Date(form.transferDate).toISOString(),
        note: form.note || undefined,
      })
      setShowForm(false)
      load()
    } catch (err: any) {
      setError(err.response?.data?.message || err.response?.data?.errors?.map((e: any) => e.message).join(', ') || 'Failed.')
    } finally {
      setLoading(false)
    }
  }

  const goPage = (p: number) => { setPage(p); load(p) }
  const totalPages = Math.max(1, Math.ceil(total / pageSize))

  return (
    <div className="page">
      <div className="page-header">
        <h1>Transfers</h1>
        <button className="btn-primary" onClick={openCreate}>+ New Transfer</button>
      </div>

      {showForm && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>New Transfer</h2>
            {error && <div className="error-msg">{error}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>From Account</label>
                <select value={form.fromAccountId} onChange={e => setForm({ ...form, fromAccountId: e.target.value })} required>
                  <option value="">Select account</option>
                  {accounts.map(a => (
                    <option key={a.id} value={a.id}>{a.name} — {a.currency} {a.balance.toFixed(2)}</option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>To Account</label>
                <select value={form.toAccountId} onChange={e => setForm({ ...form, toAccountId: e.target.value })} required>
                  <option value="">Select account</option>
                  {accounts.filter(a => a.id !== form.fromAccountId).map(a => (
                    <option key={a.id} value={a.id}>{a.name} — {a.currency} {a.balance.toFixed(2)}</option>
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
                  value={form.transferDate}
                  onChange={e => setForm({ ...form, transferDate: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Note</label>
                <input value={form.note} onChange={e => setForm({ ...form, note: e.target.value })} placeholder="Optional" />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn-primary" disabled={loading}>{loading ? 'Transferring...' : 'Transfer'}</button>
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
              <th>From</th>
              <th>To</th>
              <th>Amount</th>
              <th>Note</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 && <tr><td colSpan={5} className="empty">No transfers yet.</td></tr>}
            {items.map(t => (
              <tr key={t.id}>
                <td>{toInputDate(t.createdAt)}</td>
                <td>
                  <span className="transfer-account from">{t.fromAccountName}</span>
                </td>
                <td>
                  <span className="transfer-account to">{t.toAccountName}</span>
                </td>
                <td className="transfer-amount">{t.amount.toFixed(2)}</td>
                <td>{t.note ?? <span style={{ color: '#aaa' }}>—</span>}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {totalPages > 1 && (
        <div className="pagination">
          <button className="btn-secondary" onClick={() => goPage(page - 1)} disabled={page <= 1}>← Prev</button>
          <span>Page {page} of {totalPages} ({total} total)</span>
          <button className="btn-secondary" onClick={() => goPage(page + 1)} disabled={page >= totalPages}>Next →</button>
        </div>
      )}
    </div>
  )
}
