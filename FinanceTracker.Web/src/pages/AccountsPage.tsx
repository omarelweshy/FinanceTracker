import { useEffect, useState } from 'react'
import { getAccounts, createAccount, updateAccount, deleteAccount } from '../api/accounts'
import type { AccountDto } from '../api/accounts'

const ACCOUNT_TYPES = ['Checking', 'Savings', 'CreditCard', 'Cash', 'Investment']

export default function AccountsPage() {
  const [accounts, setAccounts] = useState<AccountDto[]>([])
  const [showForm, setShowForm] = useState(false)
  const [editing, setEditing] = useState<AccountDto | null>(null)
  const [form, setForm] = useState({ name: '', type: 'Checking', currency: 'USD' })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const load = () => getAccounts().then(setAccounts).catch(() => {})

  useEffect(() => { load() }, [])

  const openCreate = () => { setEditing(null); setForm({ name: '', type: 'Checking', currency: 'USD' }); setError(''); setShowForm(true) }
  const openEdit = (a: AccountDto) => { setEditing(a); setForm({ name: a.name, type: a.type, currency: a.currency }); setError(''); setShowForm(true) }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      if (editing) {
        await updateAccount(editing.id, { name: form.name, type: form.type })
      } else {
        await createAccount(form)
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
    if (!confirm('Soft-delete this account?')) return
    await deleteAccount(id).catch(() => {})
    load()
  }

  return (
    <div className="page">
      <div className="page-header">
        <h1>Accounts</h1>
        <button className="btn-primary" onClick={openCreate}>+ New Account</button>
      </div>

      {showForm && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Account' : 'New Account'}</h2>
            {error && <div className="error-msg">{error}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Name</label>
                <input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} required />
              </div>
              <div className="form-group">
                <label>Type</label>
                <select value={form.type} onChange={e => setForm({ ...form, type: e.target.value })}>
                  {ACCOUNT_TYPES.map(t => <option key={t} value={t}>{t}</option>)}
                </select>
              </div>
              {!editing && (
                <div className="form-group">
                  <label>Currency</label>
                  <input value={form.currency} onChange={e => setForm({ ...form, currency: e.target.value })} maxLength={3} required />
                </div>
              )}
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
            <tr><th>Name</th><th>Type</th><th>Balance</th><th>Currency</th><th>Actions</th></tr>
          </thead>
          <tbody>
            {accounts.length === 0 && <tr><td colSpan={5} className="empty">No accounts yet.</td></tr>}
            {accounts.map(a => (
              <tr key={a.id}>
                <td>{a.name}</td>
                <td><span className="badge">{a.type}</span></td>
                <td className={a.balance < 0 ? 'negative' : ''}>{a.balance.toFixed(2)}</td>
                <td>{a.currency}</td>
                <td>
                  <button className="btn-icon" onClick={() => openEdit(a)}>Edit</button>
                  <button className="btn-icon danger" onClick={() => handleDelete(a.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}
