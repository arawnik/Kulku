'use client'

import { JSX, ReactNode } from 'react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { AppContextProvider } from '@/app-context'
import { LanguageProvider } from '@/language-context'
import HeaderComponent from '@/components/HeaderComponent'
import FooterComponent from '@/components/FooterComponent'

const queryClient = new QueryClient()

const ClientProviders = ({ children }: { children: ReactNode }): JSX.Element => (
  <LanguageProvider>
    <QueryClientProvider client={queryClient}>
      <AppContextProvider>
        <HeaderComponent />
        {children}
        <FooterComponent />
      </AppContextProvider>
    </QueryClientProvider>
  </LanguageProvider>
)

export default ClientProviders
