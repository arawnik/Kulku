import React, { useState, useEffect, JSX } from 'react'
import { Experience } from '@/app/models'
import { formatEraText } from '@/utils/dateUtils'
import { useAppContext } from '@/app-context'

interface ExperienceSectionProps {
  experiences: Experience[] | undefined
  isLoading: boolean
}

const COLLAPSE_STATE_KEY = 'experience_collapsed_states'
const COLLAPSE_STATE_DEFAULT = true

const ExperienceSection = ({ experiences }: ExperienceSectionProps): JSX.Element => {
  const { t } = useAppContext()
  const [isCollapsed, setIsCollapsed] = useState<{ [key: number]: boolean }>({})

  useEffect(() => {
    const storedState = localStorage.getItem(COLLAPSE_STATE_KEY)
    const initialState: { [key: number]: boolean } = storedState ? JSON.parse(storedState) : {}

    const collapseStateWithDefaults =
      experiences?.reduce(
        (accumulator, experience) => {
          accumulator[experience.id] = initialState[experience.id] ?? COLLAPSE_STATE_DEFAULT
          return accumulator
        },
        {} as { [key: number]: boolean }
      ) ?? []

    setIsCollapsed(collapseStateWithDefaults)
  }, [experiences])

  const toggleCollapse = (id: number) => {
    setIsCollapsed((prevState) => {
      const newState = { ...prevState, [id]: !prevState[id] }
      localStorage.setItem(COLLAPSE_STATE_KEY, JSON.stringify(newState))
      return newState
    })
  }

  return (
    <section className="container container-narrow text-center">
      <hr />
      <h2 className="popout-font popout-text">{t('experience')}</h2>
      <hr />
      <div className="timeline">
        {experiences
          ? experiences.map((experience) => (
              <div
                key={experience.id}
                className="timeline-box row"
              >
                <div className="timeline-left col-md-6">
                  <p className="h4">
                    {formatEraText(experience.startDate, experience.endDate, undefined, t('present'))}
                  </p>
                  <p className="h5">{experience.title}</p>
                </div>
                <div className="timeline-right col-md-6">
                  <p className="h4">{experience.company.name}</p>
                  <p>{experience.description}</p>
                  <fieldset className="collapsible">
                    <legend
                      onClick={() => toggleCollapse(experience.id)}
                      style={{ cursor: 'pointer' }}
                    >
                      {t('keywords')}
                      <span className="ms-2">
                        {isCollapsed[experience.id] ? (
                          <i className="bi bi-chevron-compact-down"></i>
                        ) : (
                          <i className="bi bi-chevron-compact-up"></i>
                        )}
                      </span>
                    </legend>
                    <div style={{ display: isCollapsed[experience.id] ? 'none' : 'block' }}>
                      {experience.keywords.map((keyword, keywordIndex) => (
                        <span
                          key={keywordIndex}
                          className="badge list-badge"
                        >
                          {keyword.name}
                        </span>
                      ))}
                    </div>
                  </fieldset>
                </div>
              </div>
            ))
          : // Placeholders
            Array(3)
              .fill(null)
              .map((_, index) => (
                <div
                  key={index}
                  className="timeline-box row"
                >
                  <div className="timeline-left col-md-6">
                    <p className="h4">
                      <span className="placeholder col-4"></span>
                    </p>
                    <p className="h5">
                      <span className="placeholder col-6"></span>
                    </p>
                  </div>
                  <div className="timeline-right col-md-6">
                    <p className="h4">
                      <span className="placeholder col-4"></span>
                    </p>
                    <p>
                      <span className="placeholder col-10 mb-2"></span>
                      <span className="placeholder col-8 mb-2"></span>
                      <span className="placeholder col-9"></span>
                    </p>
                    <fieldset className="collapsible">
                      <legend style={{ cursor: 'pointer' }}>
                        {t('keywords')}
                        <span className="ms-2">
                          <i className="bi bi-chevron-compact-down"></i>
                        </span>
                      </legend>
                      <div style={{ display: COLLAPSE_STATE_DEFAULT ? 'none' : 'block' }}>
                        <span className="badge list-badge placeholder col-2"></span>
                        <span className="badge list-badge placeholder col-3 ms-1"></span>
                        <span className="badge list-badge placeholder col-2 ms-1"></span>
                      </div>
                    </fieldset>
                  </div>
                </div>
              ))}
      </div>
    </section>
  )
}

export default ExperienceSection
