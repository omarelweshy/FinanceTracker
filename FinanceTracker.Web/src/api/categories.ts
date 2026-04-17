import client from './client'

export interface CategoryDto {
  id: string
  name: string
  type: string
  icon: string | null
  isDefault: boolean
}

export const getCategories = () =>
  client.get<CategoryDto[]>('/api/categories').then(r => r.data)

export const createCategory = (data: { name: string; type: string; icon?: string }) =>
  client.post<CategoryDto>('/api/categories', data).then(r => r.data)

export const updateCategory = (id: string, data: { name: string; icon?: string }) =>
  client.put<CategoryDto>(`/api/categories/${id}`, data).then(r => r.data)

export const deleteCategory = (id: string) =>
  client.delete(`/api/categories/${id}`)
