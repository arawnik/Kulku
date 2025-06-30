import type { Metadata } from 'next'
import { Orbitron, Source_Sans_3 } from 'next/font/google'
import '@styles/app.scss'
import 'bootstrap-icons/font/bootstrap-icons.css'
import { JSX, StrictMode } from 'react'
import ClientProviders from '@/components/ClientProviders'

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

export const metadata: Metadata = {
  title: {
    template: '%s | Jere Junttila',
    default: 'Jere Junttila',
  },
  description: 'Personal CV and portfolio website for Jere Junttila.',
  openGraph: {
    title: 'Jere Junttila',
    locale: 'en',
    description: 'Personal CV and portfolio website for Jere Junttila.',
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
    description: 'Personal CV and portfolio website for Jere Junttila.',
    images: ['https://jerejunttila.fi/static/img/social-bg.jpg'],
  },
}

const RootLayout = ({ children }: { children: React.ReactNode }): JSX.Element => {
  return (
    <html
      lang="en"
      className={`h-100 ${orbitron.variable} ${sourceSans3.variable}`}
      data-bs-theme="dark"
      suppressHydrationWarning
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
          <ClientProviders>{children}</ClientProviders>
        </StrictMode>
      </body>
    </html>
  )
}

export default RootLayout
