import { useAppContext } from '@/app-context'
import { Keyword } from '@/app/models'
import { JSX } from 'react'

interface KeywordsSectionProps {
  skills: Keyword[] | undefined
  technologies: Keyword[] | undefined
  programmingLanguages: Keyword[] | undefined
  skillsIsLoading: boolean
  technologiesIsLoading: boolean
  programmingLanguagesIsLoading: boolean
}

const KeywordsSection = ({ programmingLanguages, skills, technologies }: KeywordsSectionProps): JSX.Element => {
  const { t } = useAppContext()

  return (
    <section className="container container-narrow text-center">
      <div className="row">
        {/* Programming Languages Section */}
        <div className="col-md-6">
          <h2 className="popout-font popout-text h3">{t('programmingLanguages')}</h2>
          <div className="info-block">
            {programmingLanguages ? (
              programmingLanguages.map((language, index) => (
                <div key={index}>
                  <p>{language.name}</p>
                  <div className="progress info-progress position-relative">
                    <div
                      className="progress-bar progress-bar-striped"
                      role="progressbar"
                      style={{ width: `${language.proficiency?.scale || 0}%` }}
                      aria-label={language.proficiency?.name}
                      aria-valuenow={language.proficiency?.scale || 0}
                      aria-valuemin={0}
                      aria-valuemax={100}
                    ></div>
                    <div className="align-middle justify-content-center d-flex position-absolute w-100 progress-text">
                      {language.proficiency?.name}
                    </div>
                  </div>
                </div>
              ))
            ) : (
              // Placeholders
              <>
                {[...Array(3)].map((_, i) => (
                  <div
                    className="mb-3"
                    key={i}
                  >
                    <p className="placeholder col-4"></p>
                    <div className="progress info-progress position-relative">
                      <div
                        className="progress-bar progress-bar-striped"
                        role="progressbar"
                        style={{ width: '100%' }}
                        aria-valuemin={0}
                        aria-valuemax={100}
                      ></div>
                      <div className="align-middle justify-content-center d-flex position-absolute w-100 progress-text placeholder col-4"></div>
                    </div>
                  </div>
                ))}
              </>
            )}
          </div>
        </div>

        {/* Skills Section */}
        <div className="col-md-6">
          <h2 className="popout-font popout-text h3">{t('skills')}</h2>
          <div className="info-block">
            {skills ? (
              skills.map((skill, index) => (
                <span
                  key={index}
                  className="badge list-badge"
                >
                  {skill.name}
                </span>
              ))
            ) : (
              // Placeholders
              <>
                {[...Array(10)].map((_, i) => (
                  <span
                    key={i}
                    className="badge list-badge placeholder col-2 mx-1"
                  >
                    &nbsp;
                  </span>
                ))}
              </>
            )}
          </div>

          {/* Technologies Section */}
          <h2 className="popout-font popout-text h3">{t('technologies')}</h2>
          <div className="info-block">
            {technologies ? (
              technologies.map((technology, index) => (
                <span
                  key={index}
                  className="badge list-badge"
                >
                  {technology.name}
                </span>
              ))
            ) : (
              // Placeholders
              <>
                {[...Array(10)].map((_, i) => (
                  <span
                    key={i}
                    className="badge list-badge placeholder col-2 mx-1"
                  >
                    &nbsp;
                  </span>
                ))}
              </>
            )}
          </div>
        </div>
      </div>
    </section>
  )
}

export default KeywordsSection
