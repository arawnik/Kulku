import { apiFetch } from '@/lib/fetch'
import { type Language } from '@/i18n/language'
import { type Keyword } from '@/lib/api/keyword'

export type Project = {
  name: string
  info: string
  description: string
  url: string
  order: number
  imageUrl: string
  keywords: Keyword[]
}

/**
 * Fetches the list of projects for the given language.
 * @param language The language in which project data should be returned.
 * @returns A list of localized projects.
 */
export const getProjects = async (language: Language): Promise<Project[]> => {
  return apiFetch<Project[]>('/project', {
    language,
    next: {
      tags: ['projects', `projects:${language}`],
    },
  })
}
