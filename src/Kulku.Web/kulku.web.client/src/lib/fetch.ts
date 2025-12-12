import { type Language } from '@/i18n/language'

export type ApiFetchOptions = Omit<RequestInit, 'headers' | 'body'> & {
  language: Language
  body?: unknown
  next?: {
    revalidate?: number | false
    tags?: string[]
  }
  headers?: Record<string, string>
}

const getBaseUrl = (): string => {
  const baseUrl = process.env.API_BASE_URL
  if (!baseUrl) {
    const msg = '[apiFetch] Missing required environment variable: API_BASE_URL'
    console.error(msg)
    throw new Error(msg)
  }
  return baseUrl
}

/**
 * Performs a typed request to the backend API with consistent headers,
 * language negotiation, optional caching directives, and structured logging.
 *
 * The function wraps the native `fetch` API and enforces:
 * - JSON request body (when provided)
 * - Standard Accept / Content-Type headers
 * - Automatic inclusion of `Accept-Language`
 * - Uniform error handling and logging for diagnostics
 * - Optional Next.js caching configuration via `next` options
 *
 * @template TResponse Expected response type returned by the API.
 * @param path Relative API path (e.g. `/project` or `/contact`).
 * @param options Request configuration including HTTP method, language,
 *                optional body, custom headers, and caching hints.
 * @returns A parsed JSON response of type `TResponse`, or `undefined` for 204 responses.
 * @throws Error when the HTTP request fails or the server returns a non-OK status.
 */
export const apiFetch = async <TResponse>(
  path: string,
  options: ApiFetchOptions
): Promise<TResponse> => {
  const { language, headers, body, ...rest } = options
  const baseUrl = getBaseUrl()
  const url = `${baseUrl}${path}`

  try {
    const res = await fetch(url, {
      ...rest,
      body: body === undefined ? undefined : JSON.stringify(body),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Accept-Language': language,
        ...(headers ?? {}),
      },
    })

    if (!res.ok) {
      let responseSample: string | undefined
      try {
        const text = await res.text()
        responseSample = text.slice(0, 500)
      } catch {
        responseSample = undefined
      }

      const logPayload = {
        level: 'error',
        type: 'apiFetch',
        method: rest.method,
        url,
        status: res.status,
        statusText: res.statusText,
        language,
        bodySent: body ?? null,
        responseSample,
      }

      console.error(JSON.stringify(logPayload))
      throw new Error(
        `[apiFetch] ${rest.method} ${url} failed with ${res.status} ${res.statusText}`
      )
    }

    if (res.status === 204) {
      return undefined as TResponse
    }

    return (await res.json()) as TResponse
  } catch (error: unknown) {
    const err = error as any
    const code = err?.cause?.code ?? err?.code
    const message = err?.message

    const logPayload = {
      level: 'error',
      type: 'apiFetch',
      method: rest.method,
      url,
      language,
      code,
      message,
    }

    console.error(JSON.stringify(logPayload))

    // Throw a new error that includes URL + code, so Next’s error overlay / logs
    // show something useful instead of just "fetch failed"
    throw new Error(
      `[apiFetch] ${rest.method} ${url} failed (code=${code ?? 'unknown'} message=${message ?? 'n/a'})`
    )
  }
}
