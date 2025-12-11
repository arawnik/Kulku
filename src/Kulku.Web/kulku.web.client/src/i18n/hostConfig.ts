import { DEFAULT_LANGUAGE, SUPPORTED_LANGUAGES, type Language } from '@/i18n/language'

type LanguageHostMap = Record<Language, string[]>

/**
 * Parse comma-separated host list from env var, falling back to a default list.
 * @param value Comma-separated host list.
 * @param fallback Default list that is used as fallback.
 * @returns Parsed host list.
 */
const parseHosts = (value: string | undefined, fallback: string[]): string[] => {
  if (!value) return fallback

  return value
    .split(',')
    .map((h) => h.trim().toLowerCase())
    .filter(Boolean)
}

/**
 *  * Returns hostname part normalized for matching:
 *   - strips port (:3000)
 *   - trims and lowercases
 * @param host String that will be normalized
 * @returns hostname part normalized for matching
 */
export const normalizeHost = (host: string): string => host.split(':')[0].trim().toLowerCase()

/**
 * Hostnames configured per language.
 * Can be overridden via environment variables at runtime.
 *
 * Convention: the **first** entry is treated as the canonical host
 * for that language (used in metadata, sitemaps, etc.).
 */
export const LANGUAGE_HOSTS: LanguageHostMap = {
  en: parseHosts(process.env.NEXT_PUBLIC_EN_HOSTS, ['en.localhost:3000']),
  fi: parseHosts(process.env.NEXT_PUBLIC_FI_HOSTS, ['localhost:3000', 'fi.localhost:3000']),
}

/**
 * Map an incoming host to a configured language.
 * Values may include ports, but matching ignores ports.
 */
export const mapHostToLanguage = (host: string | null): Language => {
  if (!host) return DEFAULT_LANGUAGE

  const incoming = normalizeHost(host)

  for (const language of SUPPORTED_LANGUAGES) {
    const lang = language as Language

    const matches = LANGUAGE_HOSTS[lang].some((configuredHost) => {
      return normalizeHost(configuredHost) === incoming
    })

    if (matches) return lang
  }

  return DEFAULT_LANGUAGE
}

/**
 * Returns the canonical host for a language.
 */
export const getCanonicalHostForLanguage = (language: Language): string => {
  const hosts = LANGUAGE_HOSTS[language]
  if (!hosts || hosts.length === 0) {
    throw new Error(`No hosts configured for language: ${language}`)
  }

  // First entry is canonical
  return hosts[0]
}

/**
 * Returns the canonical origin (protocol + host) for a language.
 * - Uses http for localhost-style dev hosts
 * - Uses https for everything else
 */
export const getCanonicalOriginForLanguage = (language: Language): string => {
  const host = getCanonicalHostForLanguage(language)
  const proto = process.env.BASE_PROTO ?? 'https'

  return `${proto}://${host}`
}
