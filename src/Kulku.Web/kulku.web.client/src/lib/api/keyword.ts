import { apiFetch } from '@/lib/fetch'
import { type Language } from '@/i18n/language'

export enum KeywordType {
  Language = 'LA', // Programming Language
  Skill = 'SK', // Skill
  Technology = 'TE', // Technology
}

export type Proficiency = {
  name: string
  description: string
  scale: number
  order: number
}

export type Keyword = {
  name: string
  type: KeywordType // LANGUAGE, SKILL, or TECHNOLOGY
  proficiency: Proficiency
  order: number
  display: boolean
}

/**
 * Fetches public, localized keywords for the given type.
 * @param type Keyword category.
 * @param language Language in which keyword data should be returned.
 * @returns A list of localized keywords for the given type.
 */
export const getKeywords = async (type: KeywordType, language: Language): Promise<Keyword[]> => {
  return apiFetch<Keyword[]>(`/project/keywords/${encodeURIComponent(type)}`, {
    language,
    next: {
      tags: ['keywords', `keywordType:${type}`, `keywords:${language}`],
    },
  })
}
