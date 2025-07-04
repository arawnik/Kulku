import { cookies, headers } from 'next/headers'
import { getRequestConfig } from 'next-intl/server'
import type { Language } from '@/language-context'

const SUPPORTED_LANGUAGES: Language[] = ['en', 'fi']

export default getRequestConfig(async () => {
  const cookieStore = await cookies()
  const headerStore = await headers()

  const cookieLang = cookieStore.get('language')?.value

  let locale: Language | undefined

  // Use cookie if it's in supported list
  if (cookieLang && SUPPORTED_LANGUAGES.includes(cookieLang as Language)) {
    locale = cookieLang as Language
  }

  // If no supported cookie value, use Accept-Language from browser
  if (!locale) {
    const acceptLangHeader = headerStore.get('accept-language')
    if (acceptLangHeader) {
      const preferredLang = acceptLangHeader.split(',')[0].split('-')[0] as Language
      if (SUPPORTED_LANGUAGES.includes(preferredLang)) {
        locale = preferredLang
      }
    }
  }

  // Default fallback
  if (!locale) {
    locale = 'en'
  }
  return {
    locale,
    messages: (await import(`../../messages/${locale}.json`)).default,
  }
})
