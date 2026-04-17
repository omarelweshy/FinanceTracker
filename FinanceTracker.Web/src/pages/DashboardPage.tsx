import { useEffect, useState } from 'react'
import { useAuth } from '../context/AuthContext'
import { getAccountsSummary } from '../api/accounts'
import type { AccountSummaryDto } from '../api/accounts'
import { Link } from 'react-router-dom'

export default function DashboardPage() {
  const { user } = useAuth()
  const [summary, setSummary] = useState<AccountSummaryDto | null>(null)

  useEffect(() => {
    getAccountsSummary().then(setSummary).catch(() => {})
  }, [])

  return (
    <div className="page">
      <h1>Welcome, {user?.fullName} 👋</h1>
      <div className="cards-grid">
        <div className="card">
          <div className="card-label">Total Balance</div>
          <div className="card-value">{summary ? `${summary.totalBalance.toFixed(2)} ${summary.currency}` : '—'}</div>
        </div>
        <div className="card">
          <div className="card-label">Active Accounts</div>
          <div className="card-value">{summary?.activeAccountsCount ?? '—'}</div>
        </div>
        <div className="card">
          <div className="card-label">Currency</div>
          <div className="card-value">{user?.currency ?? '—'}</div>
        </div>
      </div>
      <div className="quick-links">
        <Link to="/accounts" className="btn-primary">Manage Accounts</Link>
        <Link to="/categories" className="btn-secondary">Manage Categories</Link>
      </div>
    </div>
  )
}
