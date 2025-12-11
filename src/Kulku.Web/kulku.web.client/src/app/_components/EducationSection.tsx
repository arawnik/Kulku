import { JSX } from 'react'
import { getTranslations } from 'next-intl/server'
import { type Education } from '@/lib/api/education'
import { getYear } from '@/lib/utils/dateUtils'
import Timeline, { type TimelineItem } from '@/components/Timeline'

interface EducationSectionProps {
  educations: Education[] | undefined
}

const EducationSection = async ({
  educations,
}: EducationSectionProps): Promise<JSX.Element | null> => {
  const t = await getTranslations('home')

  if (!educations) return null

  const timelineItems: TimelineItem[] = educations.map((education, index) => {
    return {
      id: index.toString(),
      left: (
        <p className="font-orbitron text-xl leading-tight font-semibold">
          {education.endDate ? getYear(education.endDate) : ''}
        </p>
      ),
      right: (
        <div>
          <p className="text-lg leading-tight font-semibold">
            {education.title}
            {education.institution && education.institution.department
              ? `, ${education.institution.department}`
              : ''}
          </p>
          <p className="text-foreground-muted text-md">{education.institution.name}</p>
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

export default EducationSection
