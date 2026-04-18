import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

const navItems = [
  { to: '/', label: 'Dashboard', icon: '📊' },
  { to: '/accounts', label: 'Accounts', icon: '🏦' },
  { to: '/categories', label: 'Categories', icon: '🏷️' },
  { to: '/transactions', label: 'Transactions', icon: '💸' },
  { to: '/transfers', label: 'Transfers', icon: '↔️' },
]

export default function Layout({ children }: { children: React.ReactNode }) {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()

  const handleLogout = () => { logout(); navigate('/login') }

  return (
    <div className="app">
      <aside className="sidebar">
        <div className="sidebar-brand">💰 Finance Tracker</div>
        <nav className="sidebar-nav">
          {navItems.map(item => (
            <Link
              key={item.to}
              to={item.to}
              className={`sidebar-link ${location.pathname === item.to ? 'active' : ''}`}
            >
              <span className="sidebar-icon">{item.icon}</span>
              {item.label}
            </Link>
          ))}
        </nav>
        <div className="sidebar-footer">
          <div className="sidebar-user">
            <div className="sidebar-avatar">{user?.fullName?.[0]?.toUpperCase()}</div>
            <div className="sidebar-user-info">
              <div className="sidebar-user-name">{user?.fullName}</div>
              <div className="sidebar-user-email">{user?.email}</div>
            </div>
          </div>
          <button className="sidebar-logout" onClick={handleLogout}>Logout</button>
        </div>
      </aside>
      <main className="main-content">{children}</main>
    </div>
  )
}
