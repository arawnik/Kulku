'use client'

import { createContext, ReactNode, useMemo, useContext } from 'react'
import { useLocale } from 'next-intl'

export type Language = 'fi' | 'en'

interface LanguageContextProps {
  language: Language
  oppositeLanguage: Language
}

export const LanguageContext = createContext<LanguageContextProps | undefined>(undefined)

type LanguageContextProviderProps = {
  children: ReactNode
}

export const LanguageProvider = ({ children }: LanguageContextProviderProps) => {
  const locale = useLocale() as Language
  const value = useMemo(
    () => ({
      language: locale,
      oppositeLanguage: locale === 'en' ? 'fi' : ('en' as Language),
    }),
    [locale]
  )

  return <LanguageContext.Provider value={value}>{children}</LanguageContext.Provider>
}

export const useLanguage = () => {
  const context = useContext(LanguageContext)
  if (!context) throw new Error('useLanguage must be used within LanguageProvider')
  return context
}
