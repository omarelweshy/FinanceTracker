import client from './client'

export interface AuthResponse {
  userId: string
  email: string
  fullName: string
  currency: string
  token: string
}

export interface UserDto {
  id: string
  email: string
  fullName: string
  currency: string
}

export const register = (data: { email: string; fullName: string; password: string; currency: string }) =>
  client.post<AuthResponse>('/api/auth/register', data).then(r => r.data)

export const login = (data: { email: string; password: string }) =>
  client.post<AuthResponse>('/api/auth/login', data).then(r => r.data)

export const getMe = () =>
  client.get<UserDto>('/api/auth/me').then(r => r.data)
