'use client'

import React, { createContext, JSX, ReactNode, useCallback, useContext, useEffect, useState } from 'react'

export type Theme = 'light' | 'dark'

interface AppContextProps {
  theme: Theme
  toggleTheme: () => void
}

const AppContext = createContext<AppContextProps | undefined>(undefined)

type AppContextProviderProps = {
  children: ReactNode
}

export const AppContextProvider = ({ children }: AppContextProviderProps): JSX.Element => {
  const [theme, setTheme] = useState<Theme>('dark')
  const [mounted, setMounted] = useState(false)

  useEffect(() => {
    // Initialize Theme
    const storedTheme = (localStorage.getItem('theme') as Theme) || 'dark'
    setTheme(storedTheme)
    document.documentElement.setAttribute('data-bs-theme', storedTheme)

    setMounted(true)
  }, [])

  const toggleTheme = useCallback(() => {
    const newTheme = theme === 'dark' ? 'light' : 'dark'
    setTheme(newTheme)
    localStorage.setItem('theme', newTheme)
    document.documentElement.setAttribute('data-bs-theme', newTheme)
  }, [theme])

  // Prevent hydration error on load
  if (!mounted) {
    return <></>
  }

  return <AppContext.Provider value={{ theme, toggleTheme }}>{children}</AppContext.Provider>
}

export const useAppContext = () => {
  const context = useContext(AppContext)
  if (!context) throw new Error('useAppContext must be used within an AppContextProvider')
  return context
}
