import { useAppContext } from '@/app-context'
import { Education } from '@/app/models'
import { getYear } from '@/utils/dateUtils'
import { JSX } from 'react'

interface EducationSectionProps {
  educations: Education[] | undefined
  isLoading: boolean
}

const EducationSection = ({ educations }: EducationSectionProps): JSX.Element => {
  const { t } = useAppContext()

  return (
    <section className="container container-narrow text-center">
      <hr />
      <h2 className="popout-font popout-text">{t('education')}</h2>
      <hr />
      <div className="timeline">
        {educations
          ? educations.map((education, index) => (
              <div
                key={index}
                className="timeline-box row"
              >
                <div className="timeline-left col-md-6">
                  <p className="h4">{getYear(education.endDate)}</p>
                </div>
                <div className="timeline-right col-md-6">
                  <p className="h5">
                    {education.title}
                    {education.institution && education.institution.department
                      ? `, ${education.institution.department}`
                      : ''}
                  </p>
                  <p>{education.institution.name}</p>
                </div>
              </div>
            ))
          : // Placeholders
            Array(2)
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
                  </div>
                  <div className="timeline-right col-md-6">
                    <p className="h5">
                      <span className="placeholder col-6"></span>
                      <span className="placeholder col-3 ms-1"></span>
                    </p>
                    <p>
                      <span className="placeholder col-6"></span>
                    </p>
                  </div>
                </div>
              ))}
      </div>
    </section>
  )
}

export default EducationSection
