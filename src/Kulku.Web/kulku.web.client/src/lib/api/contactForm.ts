import { apiFetch } from '@/lib/fetch'
import { type Language } from '@/i18n/language'

export type ContactForm = {
  name: string
  email: string
  subject: string
  message: string
}

/**
 * Submits a localized contact request
 * @param payload Complete form payload including the CAPTCHA token.
 * @param language The language used for backend validation messages.
 * @returns A promise that resolves once the backend acknowledges the submission.
 */
export const submitContactForm = async (
  payload: ContactForm & { captchaToken: string },
  language: Language
): Promise<void> => {
  return apiFetch('/contact', {
    language,
    method: 'POST',
    body: payload,
  })
}
