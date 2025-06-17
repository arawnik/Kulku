'use client'

import React, { createContext, JSX, ReactNode, useCallback, useContext, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import i18n from '@/utils/i18nClient'

type Language = 'fi' | 'en'
type Theme = 'light' | 'dark'

interface AppContextProps {
  t: (key: string) => string
  i18n: typeof i18n
  setLanguage: (language: Language) => void
  oppositeLanguage: Language
  theme: Theme
  toggleTheme: () => void
}

const AppContext = createContext<AppContextProps | undefined>(undefined)

type AppContextProviderProps = {
  children: ReactNode
}

export const AppContextProvider = ({ children }: AppContextProviderProps): JSX.Element => {
  const { t } = useTranslation('common')
  const [language, setLanguageState] = useState<Language>('en')
  const [theme, setTheme] = useState<Theme>('dark')
  const [mounted, setMounted] = useState(false)

  useEffect(() => {
    // Initialize Language
    const storedLanguage = (localStorage.getItem('language') as Language) || 'en'
    i18n.changeLanguage(storedLanguage)
    setLanguageState(storedLanguage)

    // Initialize Theme
    const storedTheme = (localStorage.getItem('theme') as Theme) || 'dark'
    setTheme(storedTheme)
    document.documentElement.setAttribute('data-bs-theme', storedTheme)

    setMounted(true)
  }, [i18n])

  const setLanguage = useCallback(
    (lang: Language) => {
      i18n.changeLanguage(lang)
      localStorage.setItem('language', lang)
      setLanguageState(lang)
    },
    [i18n]
  )

  const toggleTheme = useCallback(() => {
    const newTheme = theme === 'dark' ? 'light' : 'dark'
    setTheme(newTheme)
    localStorage.setItem('theme', newTheme)
    document.documentElement.setAttribute('data-bs-theme', newTheme)
  }, [theme])

  const oppositeLanguage: Language = language === 'en' ? 'fi' : 'en'

  // Prevent hydration error on load
  if (!mounted) {
    return <></>
  }

  return (
    <AppContext.Provider value={{ t, i18n, setLanguage, oppositeLanguage, theme, toggleTheme }}>
      {children}
    </AppContext.Provider>
  )
}

export const useAppContext = () => {
  const context = useContext(AppContext)
  if (!context) throw new Error('useAppContext must be used within an AppContextProvider')
  return context
}
