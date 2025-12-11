'use server'

import { apiFetch } from '@/lib/fetch'
import { getCurrentLanguage } from '@/i18n/language'
import { cookies } from 'next/headers'

export type ContactFormState = {
  status: 'idle' | 'success' | 'error'
  errors: Record<string, string>
}

const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailRegex.test(email)
}

export async function submitContactAction(
  _prevState: ContactFormState,
  formData: FormData
): Promise<ContactFormState> {
  const language = await getCurrentLanguage()

  const name = (formData.get('name') ?? '').toString()
  const email = (formData.get('email') ?? '').toString()
  const subject = (formData.get('subject') ?? '').toString()
  const message = (formData.get('message') ?? '').toString()
  const captchaToken = (formData.get('captchaToken') ?? '').toString()

  const errors: Record<string, string> = {}

  if (!name.trim()) errors.name = 'nameRequired'
  if (!email.trim()) errors.email = 'emailRequired'
  else if (!validateEmail(email)) errors.email = 'emailInvalid'

  if (!subject.trim()) errors.subject = 'subjectRequired'
  if (!message.trim()) errors.message = 'messageRequired'
  if (!captchaToken) errors.captcha = 'reCaptchaMissing'

  if (Object.keys(errors).length > 0) {
    return { status: 'error', errors }
  }

  try {
    await apiFetch<void>('/contact', {
      language,
      method: 'POST',
      body: {
        name,
        email,
        subject,
        message,
        captchaToken,
      },
    })

    // Set a short-lived cookie so /thank-you can be gated
    const cookieStore = await cookies()
    cookieStore.set('contact_submitted', '1', {
      path: '/',
      maxAge: 60,
      httpOnly: true,
    })

    return { status: 'success', errors: {} }
  } catch (error: any) {
    console.error('Contact submit failed', error)

    // If your backend returns ProblemDetails, you can inspect it here
    // and map to field-level translation keys if you want.
    return {
      status: 'error',
      errors: {
        form: 'genericError',
      },
    }
  }
}
