import type { Language } from '@/language-context'

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || ''

const apiFetch = async <T>(url: string, language: Language, options?: RequestInit): Promise<T> => {
  const headers = new Headers(options?.headers)
  headers.set('Accept-Language', language)
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

export default apiFetch
