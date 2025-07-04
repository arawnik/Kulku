import { headers } from 'next/headers'

export const getBaseUrl = async (): Promise<string> => {
  const h = await headers()
  const host = h.get('host') || 'localhost:3000'
  const proto = h.get('x-forwarded-proto') || 'https'

  return `${proto}://${host}`
}
