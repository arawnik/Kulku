import { useMutation } from '@tanstack/react-query'
import { ContactForm } from '@/app/contact/models'
import { useLanguage } from '@/language-context'
import apiFetch from '@/utils/apiClient'

const useSubmitContactForm = () => {
  const { language } = useLanguage()

  return useMutation({
    mutationFn: (formData: ContactForm & { captchaToken: string }) =>
      apiFetch('/contact/', language, {
        method: 'POST',
        body: JSON.stringify(formData),
      }),
  })
}

export default useSubmitContactForm
