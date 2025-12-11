import type { Metadata } from 'next'
import { Geist_Mono, Orbitron, Source_Sans_3 } from 'next/font/google'
import { JSX, ReactNode } from 'react'
import { NextIntlClientProvider } from 'next-intl'
import { getLocale, getMessages, getTranslations } from 'next-intl/server'
import { getCurrentLanguage, SUPPORTED_LANGUAGES, type Language } from '@/i18n/language'
import { getCanonicalOriginForLanguage } from '@/i18n/hostConfig'
import Header from '@/components/Header'
import Footer from '@/components/Footer'
import './globals.css'

const sourceSans3 = Source_Sans_3({
  subsets: ['latin'],
  weight: ['400', '600'],
  variable: '--font-source-sans-3',
})
const geistMono = Geist_Mono({
  subsets: ['latin'],
  variable: '--font-geist-mono',
})
const orbitron = Orbitron({
  subsets: ['latin'],
  weight: ['400', '700'],
  variable: '--font-orbitron',
})

export const generateMetadata = async (): Promise<Metadata> => {
  const language: Language = await getCurrentLanguage()
  const t = await getTranslations('meta')

  const baseOrigin = getCanonicalOriginForLanguage(language)
  const imageUrl = `${baseOrigin}/static/social/social-bg.webp`

  // Build language alternates from existing host config
  const languageAlternates: Record<string, string> = {}
  for (const lang of SUPPORTED_LANGUAGES) {
    languageAlternates[lang] = getCanonicalOriginForLanguage(lang as Language)
  }

  return {
    title: {
      template: '%s | Jere Junttila',
      default: 'Jere Junttila',
    },
    description: t('description'),
    metadataBase: new URL(baseOrigin),

    alternates: {
      canonical: baseOrigin,
      languages: languageAlternates,
    },

    openGraph: {
      title: 'Jere Junttila',
      locale: language,
      description: t('description'),
      type: 'website',
      url: baseOrigin,
      images: [
        {
          url: imageUrl,
          width: 800,
          height: 640,
          alt: 'Jere Junttila Portfolio',
        },
      ],
    },
    twitter: {
      card: 'summary_large_image',
      title: 'Jere Junttila',
      description: t('description'),
      images: [imageUrl],
    },
  }
}

type RootProps = Readonly<{
  children: ReactNode
}>

const RootLayout = async ({ children }: RootProps): Promise<JSX.Element> => {
  const locale = await getLocale()
  const messages = await getMessages()

  return (
    <html
      lang={locale}
      className={`${sourceSans3.variable} ${geistMono.variable} ${orbitron.variable}`}
    >
      <body className="bg-background text-foreground font-sans antialiased">
        <NextIntlClientProvider
          locale={locale}
          messages={messages}
        >
          <Header />
          <main className="flex min-h-screen justify-center">{children}</main>
          <Footer />
        </NextIntlClientProvider>
      </body>
    </html>
  )
}

export default RootLayout
