import { type Keyword } from '@/lib/api/keyword'
import { getTranslations } from 'next-intl/server'
import { JSX } from 'react'

interface KeywordSectionnProps {
  languageKeywords: Keyword[] | undefined
  skillKeywords: Keyword[] | undefined
  technologyKeywords: Keyword[] | undefined
}

const KeywordSection = async ({
  languageKeywords,
  skillKeywords,
  technologyKeywords,
}: KeywordSectionnProps): Promise<JSX.Element> => {
  const t = await getTranslations('keywords')

  const renderKeywordPills = (
    title: string,
    keywords: Keyword[] | undefined
  ): JSX.Element | null => {
    if (!keywords) return null

    const visible = keywords.filter((k) => k.display).sort((a, b) => a.order - b.order)
    if (!visible.length) return null

    return (
      <section className="space-y-2">
        <h2 className="font-orbitron text-accent text-2xl tracking-wide">{title}</h2>
        <ul className="flex flex-wrap gap-2">
          {visible.map((keyword) => (
            <li
              key={keyword.name}
              className="border-accent rounded-md border px-3 py-1 text-xs font-medium"
            >
              {keyword.name}
            </li>
          ))}
        </ul>
      </section>
    )
  }

  const renderKeywordBars = (title: string, keywords?: Keyword[]): JSX.Element | null => {
    if (!keywords) return null

    const visible = keywords.filter((k) => k.display).sort((a, b) => a.order - b.order)

    if (!visible.length) return null

    return (
      <section className="space-y-2">
        <h2 className="font-orbitron text-accent text-2xl tracking-wide">{title}</h2>

        <div className="space-y-3 md:me-3">
          {visible.map((keyword) => {
            const scale = keyword.proficiency?.scale ?? 0
            const percent = Math.max(0, Math.min(100, scale))

            const label = keyword.proficiency?.name ?? `${percent.toFixed(0)}%`

            return (
              <div
                key={keyword.name}
                className="space-y-1"
              >
                <div className="text-md font-orbitron text-center">{keyword.name}</div>
                <div className="bg-background-muted relative h-4.5 w-full overflow-hidden rounded-md">
                  <div
                    className="bg-accent h-full"
                    style={{
                      width: `${percent}%`,
                      background:
                        'linear-gradient(90deg, var(--color-accent-alt) 0%, var(--color-accent) 100%)',
                    }}
                  />
                  <span className="absolute inset-0 flex items-center justify-center text-sm text-heading-dark drop-shadow-[0_1px_1px_rgba(0,0,0,0.7)]">
                    {label}
                  </span>
                </div>
              </div>
            )
          })}
        </div>
      </section>
    )
  }

  return (
    <section className="mx-auto grid max-w-xl space-y-6 p-5 md:max-w-5xl md:grid-cols-2">
      <div className="space-y-6">
        {renderKeywordBars(t('programmingLanguages'), languageKeywords)}
      </div>

      <div className="space-y-6">
        {renderKeywordPills(t('skills'), skillKeywords)}
        {renderKeywordPills(t('technologies'), technologyKeywords)}
      </div>
    </section>
  )
}

export default KeywordSection
