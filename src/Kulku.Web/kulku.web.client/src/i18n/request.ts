import { headers } from 'next/headers'
import { getRequestConfig } from 'next-intl/server'
import { mapHostToLanguage } from '@/i18n/hostConfig'

/**
 * Get config for current request
 */
export default getRequestConfig(async () => {
  const headerStore = await headers()
  const host = headerStore.get('x-forwarded-host') ?? headerStore.get('host')

  const language = mapHostToLanguage(host)

  return {
    locale: language,
    messages: (await import(`../../messages/${language}.json`)).default,
  }
})
