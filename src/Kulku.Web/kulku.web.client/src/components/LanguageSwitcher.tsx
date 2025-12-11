'use client'

import Link from 'next/link'
import { JSX, useEffect, useState } from 'react'
import { useTranslations } from 'next-intl'
import { usePathname, useSearchParams } from 'next/navigation'
import { LANGUAGE_HOSTS, mapHostToLanguage } from '@/i18n/hostConfig'
import { type Language } from '@/i18n/language'

const LanguageSwitcher = (): JSX.Element | null => {
  const t = useTranslations('header')
  const [currentHost, setCurrentHost] = useState<string | null>(null)

  const pathname = usePathname()
  const searchParams = useSearchParams()

  useEffect(() => {
    if (typeof window !== 'undefined') {
      setCurrentHost(window.location.host.toLowerCase())
    }
  }, [])

  if (!currentHost) return null

  // Determine current language
  const currentLanguage: Language = mapHostToLanguage(currentHost)
  const targetLanguage: Language = currentLanguage === 'en' ? 'fi' : 'en'

  // First configured host for target language
  const targetHost = LANGUAGE_HOSTS[targetLanguage][0]

  // Build path + query from Next routing
  const search = searchParams.toString()
  const pathWithQuery = search ? `${pathname}?${search}` : pathname

  // Turn into protocol-relative URL
  const href = `//${targetHost}${pathWithQuery}`

  return (
    <Link
      href={href}
      prefetch={false}
      className="text-foreground-muted hover:text-accent"
    >
      {t('oppositeLang')}
    </Link>
  )
}

export default LanguageSwitcher
