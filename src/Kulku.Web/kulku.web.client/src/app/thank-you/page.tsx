'use server'

import { JSX } from 'react'
import { Metadata } from 'next'
import { getTranslations } from 'next-intl/server'
import { cookies } from 'next/headers'
import { redirect } from 'next/navigation'

export const generateMetadata = async (): Promise<Metadata> => {
  const t = await getTranslations('thankYou')
  return {
    title: t('title'),
    description: t('description'),
  }
}

const ThankYouPage = async (): Promise<JSX.Element> => {
  const t = await getTranslations('thankYou')
  const cookieStore = await cookies()
  const submitted = cookieStore.get('contact_submitted')

  // Prevent direct access
  if (!submitted) {
    redirect('/contact')
  }

  return (
    <section className="w-full max-w-5xl space-y-8 px-4 py-8">
      <h2 className="font-orbitron mb-3 text-3xl">{t('title')}</h2>
      <p className="text-foreground-muted text-lg">{t('message')}</p>
    </section>
  )
}

export default ThankYouPage
