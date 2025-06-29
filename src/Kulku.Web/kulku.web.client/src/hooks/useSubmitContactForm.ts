import { useMutation } from '@tanstack/react-query'
import { apiFetch } from '@/utils/apiClient'
import { ContactForm } from '@/app/contact/models'

export function useSubmitContactForm() {
  return useMutation({
    mutationFn: (formData: ContactForm & { captchaToken: string }) =>
      apiFetch('/contact/', {
        method: 'POST',
        body: JSON.stringify(formData),
      }),
  })
}
