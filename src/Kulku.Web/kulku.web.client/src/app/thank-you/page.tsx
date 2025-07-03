'use client'

import { JSX } from 'react'
import Head from 'next/head'
import { useTranslations } from 'next-intl'

const ThankYouPage = (): JSX.Element => {
  const t = useTranslations()

  return (
    <>
      <Head>
        <title>{t('thankYou')}</title>
      </Head>
      <main className="container my-5">
        <h2 className="popout-font">{t('thankYou')}</h2>
        <p className="fw-semibold">{t('thankYouMessage')}</p>
      </main>
    </>
  )
}

export default ThankYouPage
