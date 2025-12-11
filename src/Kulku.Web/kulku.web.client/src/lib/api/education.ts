import { apiFetch } from '@/lib/fetch'
import { type Language } from '@/i18n/language'

export type Institution = {
  name: string
  department?: string
  description: string
}

export type Education = {
  title: string
  description: string
  institution: Institution
  startDate: string
  endDate?: string
}

/**
 * Fetches the list of educations for the given language.
 * @param language The language in which education data should be returned.
 * @returns A list of localized educations.
 */
export const getEducations = async (language: Language): Promise<Education[]> => {
  return apiFetch<Education[]>('/education', {
    language,
    next: {
      tags: ['educations', `educations:${language}`],
    },
  })
}
