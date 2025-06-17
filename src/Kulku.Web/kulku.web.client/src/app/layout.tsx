import type { Metadata } from 'next'
import '@styles/App.scss'
import 'bootstrap-icons/font/bootstrap-icons.css'
import { StrictMode } from 'react'
import ClientProviders from '@/components/ClientProviders'

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

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html
      lang="en"
      className="h-100"
      data-bs-theme="dark"
      suppressHydrationWarning
    >
      <head>
        <link
          rel="stylesheet"
          href="https://fonts.googleapis.com/css?family=Orbitron|Source+Sans+Pro&display=swap"
        />
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
