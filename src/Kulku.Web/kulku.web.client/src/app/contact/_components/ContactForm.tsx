'use client'

import { JSX, useEffect, useState } from 'react'
import { useTranslations } from 'next-intl'
import { useRouter } from 'next/navigation'
import { useActionState } from 'react'
import { useFormStatus } from 'react-dom'
import ReCAPTCHA from 'react-google-recaptcha'
import { submitContactAction, type ContactFormState } from '@/app/contact/actions'
import { getClientLanguage } from '@/i18n/clientLanguage'

const initialState: ContactFormState = {
  status: 'idle',
  errors: {},
}

const SubmitButton = (): JSX.Element => {
  const t = useTranslations('contact')
  const { pending } = useFormStatus()

  return (
    <button
      type="submit"
      disabled={pending}
      className="border-accent bg-accent text-md hover:bg-accent-alt inline-flex w-full cursor-pointer items-center justify-center rounded-md border px-4 py-2 font-semibold transition disabled:cursor-not-allowed disabled:opacity-70"
    >
      {pending ? t('sending') : t('send')}
    </button>
  )
}

const ContactForm = (): JSX.Element => {
  const t = useTranslations('contact')
  const language = getClientLanguage()
  const router = useRouter()

  // Local controlled values
  const [values, setValues] = useState({
    name: '',
    email: '',
    subject: '',
    message: '',
  })

  const [captchaToken, setCaptchaToken] = useState<string | null>(null)

  const [state, formAction] = useActionState(submitContactAction, initialState)
  const err = state.errors

  // Redirect & clear fields on success
  useEffect(() => {
    if (state.status === 'success') {
      setValues({ name: '', email: '', subject: '', message: '' })
      setCaptchaToken(null)
      router.push('/thank-you')
    }
  }, [state.status, router])

  const handleChange =
    (field: keyof typeof values) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      const { value } = e.target
      setValues((prev) => ({ ...prev, [field]: value }))
    }

  const handleCaptchaChange = (token: string | null) => {
    setCaptchaToken(token)
  }

  return (
    <form
      action={formAction}
      className="space-y-4"
    >
      <input
        type="hidden"
        name="captchaToken"
        value={captchaToken ?? ''}
      />

      {err.form && (
        <div className="rounded-md border border-red-300 bg-red-50 px-3 py-2 text-xs text-red-800">
          {t(err.form)}
        </div>
      )}

      {/* Name */}
      <div className="space-y-1">
        <label
          htmlFor="name"
          className="block text-sm font-medium"
        >
          {t('nameLabel')}
        </label>
        <input
          id="name"
          name="name"
          type="text"
          value={values.name}
          onChange={handleChange('name')}
          placeholder={t('namePlaceholder')}
          className={`w-full rounded-md border px-3 py-2 text-sm transition outline-none ${
            err.name
              ? 'border-red-400 focus:border-red-500 focus:ring-1 focus:ring-red-400'
              : 'border-accent/40 focus:border-accent focus:ring-accent/60 focus:ring-1'
          } `}
          aria-invalid={!!err.name}
        />
        {err.name && <p className="text-xs text-red-600">{t(err.name)}</p>}
      </div>

      {/* Email */}
      <div className="space-y-1">
        <label
          htmlFor="email"
          className="block text-sm font-medium"
        >
          {t('emailLabel')}
        </label>
        <input
          id="email"
          name="email"
          type="email"
          value={values.email}
          onChange={handleChange('email')}
          placeholder={t('emailPlaceholder')}
          className={`w-full rounded-md border px-3 py-2 text-sm transition outline-none ${
            err.email
              ? 'border-red-400 focus:border-red-500 focus:ring-1 focus:ring-red-400'
              : 'border-accent/40 focus:border-accent focus:ring-accent/60 focus:ring-1'
          } `}
          aria-invalid={!!err.email}
        />
        {err.email && <p className="text-xs text-red-600">{t(err.email)}</p>}
      </div>

      {/* Subject */}
      <div className="space-y-1">
        <label
          htmlFor="subject"
          className="block text-sm font-medium"
        >
          {t('subjectLabel')}
        </label>
        <input
          id="subject"
          name="subject"
          type="text"
          value={values.subject}
          onChange={handleChange('subject')}
          placeholder={t('subjectPlaceholder')}
          className={`w-full rounded-md border px-3 py-2 text-sm transition outline-none ${
            err.subject
              ? 'border-red-400 focus:border-red-500 focus:ring-1 focus:ring-red-400'
              : 'border-accent/40 focus:border-accent focus:ring-accent/60 focus:ring-1'
          } `}
          aria-invalid={!!err.subject}
        />
        {err.subject && <p className="text-xs text-red-600">{t(err.subject)}</p>}
      </div>

      {/* Message */}
      <div className="space-y-1">
        <label
          htmlFor="message"
          className="block text-sm font-medium"
        >
          {t('messageLabel')}
        </label>
        <textarea
          id="message"
          name="message"
          rows={5}
          value={values.message}
          onChange={handleChange('message')}
          placeholder={t('messagePlaceholder')}
          className={`w-full resize-y rounded-md border px-3 py-2 text-sm transition outline-none ${
            err.message
              ? 'border-red-400 focus:border-red-500 focus:ring-1 focus:ring-red-400'
              : 'border-accent/40 focus:border-accent focus:ring-accent/60 focus:ring-1'
          } `}
          aria-invalid={!!err.message}
        />
        {err.message && <p className="text-xs text-red-600">{t(err.message)}</p>}
      </div>

      <div className="space-y-2">
        <ReCAPTCHA
          sitekey={process.env.NEXT_PUBLIC_RECAPTCHA_SITE_KEY ?? ''}
          hl={language}
          theme="light"
          onChange={handleCaptchaChange}
        />
        {err.captcha && <p className="text-sm text-red-600">{t(err.captcha)}</p>}
      </div>

      {/* Submit */}
      <div className="pt-2">
        <SubmitButton />
      </div>
    </form>
  )
}

export default ContactForm
