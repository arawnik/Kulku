import i18n from '@/utils/i18nClient'
import i18nextConfig from '../../next-i18next.config'

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || ''

export async function apiFetch<T>(url: string, options?: RequestInit): Promise<T> {
  const headers = new Headers(options?.headers)
  headers.set('Accept-Language', i18n?.language || i18nextConfig.i18n.defaultLocale)
  headers.set('Content-Type', 'application/json')

  const response = await fetch(`${API_BASE_URL}${url}`, {
    ...options,
    headers,
  })

  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}))
    throw { status: response.status, ...errorData }
  }

  if (response.status === 204) return null as unknown as T

  return response.json()
}
