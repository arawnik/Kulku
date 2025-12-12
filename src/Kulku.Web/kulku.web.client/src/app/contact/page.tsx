import type { JSX } from 'react'
import type { Metadata } from 'next'
import { getTranslations } from 'next-intl/server'
import ContactForm from './_components/ContactForm'
import PageHeader from '@/components/PageHeader'

export const generateMetadata = async (): Promise<Metadata> => {
  const t = await getTranslations('contact')
  return {
    title: t('title'),
    description: t('description'),
  }
}

const ContactPage = async (): Promise<JSX.Element> => {
  const t = await getTranslations('contact')

  return (
    <section className="w-full max-w-5xl px-4 py-8">
      <PageHeader
        title={t('title')}
        subTitle={t('subTitle')}
      />

      <ContactForm />
    </section>
  )
}

export default ContactPage
