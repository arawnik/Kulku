import { apiFetch } from '@/lib/fetch'
import { type Language } from '@/i18n/language'
import { type Keyword } from '@/lib/api/keyword'

export type Company = {
  name: string
  description: string
}

export type Experience = {
  id: number
  title: string
  description: string
  company: Company
  keywords: Keyword[]
  startDate: string // Date in string format
  endDate?: string // Date in string format or null
}

/**
 * Fetches the list of experiences for the given language.
 * @param language The language in which experience data should be returned.
 * @returns A list of localized experiences.
 */
export const getExperiences = async (language: Language): Promise<Experience[]> => {
  return apiFetch<Experience[]>('/experience', {
    language,
    next: {
      tags: ['experiences', `experiences:${language}`],
    },
  })
}
