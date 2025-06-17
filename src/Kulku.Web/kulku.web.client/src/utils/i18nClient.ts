'use client'

import i18n from 'i18next'
import { initReactI18next } from 'react-i18next'
import resourcesToBackend from 'i18next-resources-to-backend'
import i18nextConfig from '../../next-i18next.config'

i18n
  .use(initReactI18next)
  .use(resourcesToBackend((lng: string, ns: string) => import(`../../public/locales/${lng}/${ns}.json`)))
  .init({
    lng: typeof window !== 'undefined' ? localStorage.getItem('language') || 'en' : 'en',
    fallbackLng: i18nextConfig.i18n.defaultLocale,
    ns: ['common'],
    defaultNS: 'common',
    interpolation: {
      escapeValue: false,
    },
  })

export default i18n
