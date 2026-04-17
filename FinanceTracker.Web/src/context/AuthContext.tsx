import { createContext, useContext, useState, useEffect } from 'react'
import type { ReactNode } from 'react'
import { getMe } from '../api/auth'
import type { UserDto } from '../api/auth'

interface AuthContextType {
  user: UserDto | null
  token: string | null
  login: (token: string) => void
  logout: () => void
  loading: boolean
}

const AuthContext = createContext<AuthContextType>(null!)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserDto | null>(null)
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'))
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    if (token) {
      getMe().then(setUser).catch(() => { setToken(null); localStorage.removeItem('token') }).finally(() => setLoading(false))
    } else {
      setLoading(false)
    }
  }, [token])

  const login = (t: string) => { localStorage.setItem('token', t); setToken(t) }
  const logout = () => { localStorage.removeItem('token'); setToken(null); setUser(null) }

  return <AuthContext.Provider value={{ user, token, login, logout, loading }}>{children}</AuthContext.Provider>
}

export const useAuth = () => useContext(AuthContext)
