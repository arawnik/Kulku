import { apiFetch } from '@/lib/fetch'
import { type Language } from '@/i18n/language'

export type Introduction = {
  title: string
  content: string
  tagline: string
  avatarUrl: string // URL for the avatar image
  smallAvatarUrl: string // URL for the small avatar image
  pdf: string // URL for the PDF file
}

/**
 * Fetches introduction for given language.
 * @param language The language in which introduction data should be returned.
 * @returns A localized introduction.
 */
export const getIntroduction = async (language: Language): Promise<Introduction | undefined> => {
  return apiFetch<Introduction | undefined>('/introduction', {
    language,
    next: {
      tags: ['introduction', `introduction:${language}`],
    },
  })
}
