'use client'

import { JSX, ReactNode } from 'react'
import { AppContextProvider } from '@/app-context'
import { I18nextProvider } from 'react-i18next'
import i18n from '@/utils/i18nClient'
import HeaderComponent from '@/components/HeaderComponent'
import FooterComponent from '@/components/FooterComponent'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

const queryClient = new QueryClient()

const ClientProviders = ({ children }: { children: ReactNode }): JSX.Element => (
  <I18nextProvider i18n={i18n}>
    <AppContextProvider>
      <QueryClientProvider client={queryClient}>
        <HeaderComponent />
        {children}
        <FooterComponent />
      </QueryClientProvider>
    </AppContextProvider>
  </I18nextProvider>
)

export default ClientProviders
