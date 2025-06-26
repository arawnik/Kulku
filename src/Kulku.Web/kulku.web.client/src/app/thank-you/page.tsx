'use client'

import Head from 'next/head'
import { useAppContext } from '@/app-context'
import { JSX } from 'react'

const ThankYouPage = (): JSX.Element => {
  const { t } = useAppContext()

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
