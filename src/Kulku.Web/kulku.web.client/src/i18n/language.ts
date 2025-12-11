import { getLocale } from 'next-intl/server'

export const DEFAULT_LANGUAGE = 'fi' as const
export const SUPPORTED_LANGUAGES = ['en', 'fi'] as const
export type Language = (typeof SUPPORTED_LANGUAGES)[number]

/**
 * Check language at runtime.
 * @param value Value to be checked.
 * @returns
 */
const isLanguage = (value: string): value is Language =>
  SUPPORTED_LANGUAGES.includes(value as Language)

/**
 * Resolves the currently active language for the request.
 *
 * This function integrates with `next-intl` to obtain the locale that is mapped from
 * the incoming hostname and then validated at runtime to one of the supported languages.
 *
 * This helper should be used inside server components, data loaders, and API
 * client functions whenever language-aware backend requests or localized
 * rendering are needed.
 *
 * @returns A strongly typed `Language` value.
 */
export const getCurrentLanguage = async (): Promise<Language> => {
  const locale = await getLocale()
  return isLanguage(locale) ? locale : DEFAULT_LANGUAGE
}
