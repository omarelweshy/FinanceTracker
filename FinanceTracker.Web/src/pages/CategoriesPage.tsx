import { useEffect, useState } from 'react'
import { getCategories, createCategory, updateCategory, deleteCategory } from '../api/categories'
import type { CategoryDto } from '../api/categories'

const CATEGORY_TYPES = ['Income', 'Expense']

export default function CategoriesPage() {
  const [categories, setCategories] = useState<CategoryDto[]>([])
  const [showForm, setShowForm] = useState(false)
  const [editing, setEditing] = useState<CategoryDto | null>(null)
  const [form, setForm] = useState({ name: '', type: 'Expense', icon: '' })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const load = () => getCategories().then(setCategories).catch(() => {})

  useEffect(() => { load() }, [])

  const openCreate = () => { setEditing(null); setForm({ name: '', type: 'Expense', icon: '' }); setError(''); setShowForm(true) }
  const openEdit = (c: CategoryDto) => { setEditing(c); setForm({ name: c.name, type: c.type, icon: c.icon || '' }); setError(''); setShowForm(true) }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      if (editing) {
        await updateCategory(editing.id, { name: form.name, icon: form.icon || undefined })
      } else {
        await createCategory({ name: form.name, type: form.type, icon: form.icon || undefined })
      }
      setShowForm(false)
      load()
    } catch (err: any) {
      setError(err.response?.data?.message || err.response?.data?.errors?.map((e: any) => e.message).join(', ') || 'Failed.')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (c: CategoryDto) => {
    if (c.isDefault) return
    if (!confirm('Delete this category?')) return
    try {
      await deleteCategory(c.id)
      load()
    } catch (err: any) {
      alert(err.response?.data?.message || 'Failed to delete.')
    }
  }

  const income = categories.filter(c => c.type === 'Income')
  const expense = categories.filter(c => c.type === 'Expense')

  return (
    <div className="page">
      <div className="page-header">
        <h1>Categories</h1>
        <button className="btn-primary" onClick={openCreate}>+ New Category</button>
      </div>

      {showForm && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Category' : 'New Category'}</h2>
            {error && <div className="error-msg">{error}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Name</label>
                <input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} required />
              </div>
              {!editing && (
                <div className="form-group">
                  <label>Type</label>
                  <select value={form.type} onChange={e => setForm({ ...form, type: e.target.value })}>
                    {CATEGORY_TYPES.map(t => <option key={t} value={t}>{t}</option>)}
                  </select>
                </div>
              )}
              <div className="form-group">
                <label>Icon (emoji)</label>
                <input value={form.icon} onChange={e => setForm({ ...form, icon: e.target.value })} placeholder="e.g. 🍕" />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn-primary" disabled={loading}>{loading ? 'Saving...' : 'Save'}</button>
                <button type="button" className="btn-secondary" onClick={() => setShowForm(false)}>Cancel</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {[{ label: 'Income', items: income }, { label: 'Expense', items: expense }].map(group => (
        <div key={group.label}>
          <h2 className="group-title">{group.label}</h2>
          <div className="categories-grid">
            {group.items.map(c => (
              <div key={c.id} className={`category-card ${c.isDefault ? 'default' : ''}`}>
                <span className="category-icon">{c.icon || '📁'}</span>
                <span className="category-name">{c.name}</span>
                {c.isDefault && <span className="badge">default</span>}
                {!c.isDefault && (
                  <div className="category-actions">
                    <button className="btn-icon" onClick={() => openEdit(c)}>Edit</button>
                    <button className="btn-icon danger" onClick={() => handleDelete(c)}>Delete</button>
                  </div>
                )}
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
  )
}
