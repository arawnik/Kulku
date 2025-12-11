'use client'

import { JSX, useEffect, useState } from 'react'
import { useTranslations } from 'next-intl'
import { type Experience } from '@/lib/api/experience'
import { formatEraText } from '@/lib/utils/dateUtils'
import Timeline, { type TimelineItem } from '@/components/Timeline'

interface ExperienceSectionProps {
  experiences: Experience[] | undefined
}

const COLLAPSE_STATE_KEY = 'experience_collapsed_states'
const COLLAPSE_STATE_DEFAULT = true

type CollapseState = Record<string, boolean>

const ExperienceSection = ({ experiences }: ExperienceSectionProps): JSX.Element | null => {
  const t = useTranslations('home')
  const [collapseState, setCollapseState] = useState<CollapseState>({})

  if (!experiences || experiences.length === 0) return null

  // Initialize collapse state from localStorage + defaults
  useEffect(() => {
    if (!experiences) return

    const stored =
      typeof window !== 'undefined' ? window.localStorage.getItem(COLLAPSE_STATE_KEY) : null

    const initial: CollapseState = stored ? JSON.parse(stored) : {}

    const withDefaults: CollapseState = experiences.reduce((acc, experience) => {
      const key = String(experience.id)
      acc[key] = initial[key] ?? COLLAPSE_STATE_DEFAULT
      return acc
    }, {} as CollapseState)

    setCollapseState(withDefaults)
  }, [experiences])

  const toggleCollapse = (id: Experience['id']) => {
    setCollapseState((prev) => {
      const key = String(id)
      const next: CollapseState = {
        ...prev,
        [key]: !prev[key],
      }

      if (typeof window !== 'undefined') {
        window.localStorage.setItem(COLLAPSE_STATE_KEY, JSON.stringify(next))
      }

      return next
    })
  }

  const timelineItems: TimelineItem[] = experiences.map((experience, index) => {
    const key = String(experience.id ?? index)
    const isCollapsed = collapseState[key] ?? COLLAPSE_STATE_DEFAULT
    const visibleKeywords = experience.keywords.filter((k) => k.display)

    return {
      id: key,
      left: (
        <div>
          <p className="font-orbitron text-xl leading-tight font-semibold">
            {formatEraText(experience.startDate, experience.endDate, undefined, t('present'))}
          </p>
          <p className="text-lg">{experience.title}</p>
        </div>
      ),
      right: (
        <div>
          <p className="text-xl font-semibold">{experience.company.name}</p>
          <p className="text-md text-foreground-muted leading-tight">{experience.description}</p>
          {visibleKeywords.length > 0 && (
            <fieldset className="border-accent/40 mt-1 rounded-md border px-3 pt-1 pb-2">
              <legend className="text-accent text-md px-1 font-semibold tracking-wide">
                <button
                  type="button"
                  onClick={() => toggleCollapse(experience.id)}
                  className="inline-flex cursor-pointer items-center gap-2"
                >
                  <span>{t('keywords')}</span>
                  <span
                    className={`transition-transform ${isCollapsed ? '' : 'rotate-180'}`}
                    aria-hidden="true"
                  >
                    ▾
                  </span>
                </button>
              </legend>

              <div className={isCollapsed ? 'hidden' : 'mt-1 flex flex-wrap gap-2'}>
                {visibleKeywords.map((keyword) => (
                  <span
                    key={keyword.name}
                    className="border-accent rounded-md border px-2 py-0.5 text-[11px] font-medium"
                  >
                    {keyword.name}
                  </span>
                ))}
              </div>
            </fieldset>
          )}
        </div>
      ),
    }
  })

  return (
    <div className="mx-auto my-6 max-w-5xl">
      <Timeline items={timelineItems} />
    </div>
  )
}

export default ExperienceSection
