'use client'

import { mapHostToLanguage } from './hostConfig'
import { DEFAULT_LANGUAGE, type Language } from './language'

/**
 * Get current language in a client component
 * @returns Current language
 */
export const getClientLanguage = (): Language => {
  if (typeof window === 'undefined') {
    return DEFAULT_LANGUAGE
  }

  return mapHostToLanguage(window.location.host)
}
