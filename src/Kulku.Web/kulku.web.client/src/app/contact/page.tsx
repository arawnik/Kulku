'use client'

import { JSX, useState } from 'react'
import { useRouter } from 'next/navigation'
import Head from 'next/head'
import Script from 'next/script'
import ReCAPTCHA from 'react-google-recaptcha'
import { useSubmitContactForm } from '@/hooks/useSubmitContactForm'
import { useAppContext } from '@/app-context'
import { ContactForm } from '@/app/contact/models'
import { mapProblemDetailsErrors } from '@/utils/errorUtils'
import { omitKey } from '@/utils/objectUtils'

const ContactPage = (): JSX.Element => {
  const { t } = useAppContext()
  const router = useRouter()

  const [formData, setFormData] = useState<ContactForm>({
    name: '',
    email: '',
    subject: '',
    message: '',
  })
  const [captchaToken, setCaptchaToken] = useState<string | null>(null)
  const [errors, setErrors] = useState<Record<string, string>>({})

  const { mutate, isPending } = useSubmitContactForm()

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    if (!captchaToken) {
      setErrors({ captcha: t('reCaptchaMissing') })
      return
    }

    mutate(
      { ...formData, captchaToken },
      {
        onSuccess: () => {
          setFormData({ name: '', email: '', subject: '', message: '' })
          setCaptchaToken(null)
          setErrors({})
          router.push('/thank-you')
        },
        onError: (error) => {
          setErrors(mapProblemDetailsErrors(error))
        },
      }
    )
  }

  const handleCaptchaChange = (token: string | null) => {
    setCaptchaToken(token)
    if (token) {
      setErrors((prev) => omitKey(prev, 'captcha'))
    }
  }

  return (
    <>
      <Head>
        <title>{t('contactMe')}</title>
      </Head>
      <Script
        src="https://www.google.com/recaptcha/api.js"
        async
        defer
      />
      <main className="container my-5">
        <h1 className="popout-font mb-3">{t('contactMe')}</h1>
        <hr />
        <form
          onSubmit={handleSubmit}
          id="contact-form"
        >
          {/* Name Field */}
          <div className="form-group mb-3">
            <label
              htmlFor="name"
              className="form-label"
            >
              {t('yourName')}
            </label>
            <input
              type="text"
              id="name"
              className={`form-control ${errors.name ? 'is-invalid' : ''}`}
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder={t('name')}
            />
            {errors.name && <div className="invalid-feedback">{errors.name}</div>}
          </div>

          {/* Email Field */}
          <div className="form-group mb-3">
            <label
              htmlFor="email"
              className="form-label"
            >
              {t('yourEmail')}
            </label>
            <input
              type="email"
              id="email"
              className={`form-control ${errors.email ? 'is-invalid' : ''}`}
              value={formData.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              placeholder={t('email')}
            />
            {errors.email && <div className="invalid-feedback">{errors.email}</div>}
          </div>

          {/* Subject Field */}
          <div className="form-group mb-3">
            <label
              htmlFor="subject"
              className="form-label"
            >
              {t('yourSubject')}
            </label>
            <input
              type="text"
              id="subject"
              className={`form-control ${errors.subject ? 'is-invalid' : ''}`}
              value={formData.subject}
              onChange={(e) => setFormData({ ...formData, subject: e.target.value })}
              placeholder={t('subject')}
            />
            {errors.subject && <div className="invalid-feedback">{errors.subject}</div>}
          </div>

          {/* Message Field */}
          <div className="form-group mb-3">
            <label
              htmlFor="message"
              className="form-label"
            >
              {t('yourMessage')}
            </label>
            <textarea
              id="message"
              className={`form-control ${errors.message ? 'is-invalid' : ''}`}
              value={formData.message}
              onChange={(e) => setFormData({ ...formData, message: e.target.value })}
              placeholder={t('message')}
            />
            {errors.message && <div className="invalid-feedback">{errors.message}</div>}
          </div>

          {/* reCAPTCHA Field */}
          <div className="row mb-4">
            <div className="col-md-6">
              <ReCAPTCHA
                sitekey={process.env.NEXT_PUBLIC_RECAPTCHA_SITE_KEY || ''}
                onChange={handleCaptchaChange}
              />
            </div>
            {errors.captcha && (
              <div className="col-md-6 mt-3">
                <div
                  className="alert alert-danger"
                  role="alert"
                >
                  {errors.captcha}
                </div>
              </div>
            )}
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            className="btn btn-primary btn-block"
            disabled={isPending}
          >
            {isPending ? t('sending') : t('send')}
          </button>
        </form>
      </main>
    </>
  )
}

export default ContactPage
