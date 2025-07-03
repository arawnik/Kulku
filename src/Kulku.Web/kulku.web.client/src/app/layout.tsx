import { JSX, StrictMode } from 'react'
import type { Metadata } from 'next'
import { NextIntlClientProvider } from 'next-intl'
import { getLocale, getTranslations } from 'next-intl/server'
import ClientProviders from '@/components/ClientProviders'
import { Orbitron, Source_Sans_3 } from 'next/font/google'
import 'bootstrap-icons/font/bootstrap-icons.css'
import '@styles/app.scss'

// load & optimize the fonts:
const orbitron = Orbitron({
  subsets: ['latin'],
  weight: ['400', '700'],
  variable: '--font-orbitron',
})
const sourceSans3 = Source_Sans_3({
  subsets: ['latin'],
  weight: ['400', '600'],
  variable: '--font-source-sans-3',
})

export const generateMetadata = async (): Promise<Metadata> => {
  const locale = await getLocale()
  const t = await getTranslations()

  return {
    title: {
      template: '%s | Jere Junttila',
      default: 'Jere Junttila',
    },
    description: t('metaDescription'),
    openGraph: {
      title: 'Jere Junttila',
      locale: locale,
      description: t('metaDescription'),
      type: 'website',
      url: 'https://jerejunttila.fi',
      images: [
        {
          url: 'https://jerejunttila.fi/static/img/social-bg.jpg',
          width: 800,
          height: 640,
          alt: 'Jere Junttila Portfolio',
        },
      ],
    },
    twitter: {
      card: 'summary_large_image',
      title: 'Jere Junttila',
      description: t('metaDescription'),
      images: ['https://jerejunttila.fi/static/img/social-bg.jpg'],
    },
  }
}

const RootLayout = async ({ children }: { children: React.ReactNode }): Promise<JSX.Element> => {
  const locale = await getLocale()

  return (
    <html
      lang={locale}
      className={`h-100 ${orbitron.variable} ${sourceSans3.variable}`}
      data-bs-theme="dark"
    >
      <head>
        {/* Inline script for initial theme setup to prevent flash */}
        <script
          dangerouslySetInnerHTML={{
            __html: `
              (function() {
                const theme = localStorage.getItem('theme') || 'dark';
                document.documentElement.setAttribute('data-bs-theme', theme);
              })();
            `,
          }}
        />
      </head>
      <body className={'d-flex flex-column h-100 flex'}>
        <StrictMode>
          <NextIntlClientProvider>
            <ClientProviders>{children}</ClientProviders>
          </NextIntlClientProvider>
        </StrictMode>
      </body>
    </html>
  )
}

export default RootLayout
