'use client'

import { JSX } from 'react'
import { KeywordType } from '@/app/models'
import IntroductionSection from '@/components/sections/IntroductionSection'
import ExperienceSection from '@/components/sections/ExperienceSection'
import EducationSection from '@/components/sections/EducationSection'
import KeywordsSection from '@/components/sections/KeywordsSection'
import { useAppContext } from '@/app-context'
import Head from 'next/head'
import useIntroduction from '@/hooks/useIntroduction'
import useExperiences from '@/hooks/useExperiences'
import useEducations from '@/hooks/useEducations'
import useKeywords from '@/hooks/useKeywords'

const CoverPage = (): JSX.Element => {
  const { t } = useAppContext()

  const { data: intro, isLoading: isLoadingIntro } = useIntroduction()
  const { data: experiences, isLoading: isLoadingExp } = useExperiences()
  const { data: educations, isLoading: isLoadingEdu } = useEducations()
  const { data: skills, isLoading: isLoadingSkills } = useKeywords(KeywordType.Skill)
  const { data: technologies, isLoading: isLoadingTech } = useKeywords(KeywordType.Technology)
  const { data: programmingLanguages, isLoading: isLoadingLang } = useKeywords(KeywordType.Language)

  return (
    <>
      <Head>
        <title>{intro ? intro.title : t('loading')}</title>
      </Head>
      <main>
        <IntroductionSection
          intro={intro}
          isLoading={isLoadingIntro}
        />
        <KeywordsSection
          skills={skills}
          skillsIsLoading={isLoadingSkills}
          technologies={technologies}
          technologiesIsLoading={isLoadingTech}
          programmingLanguages={programmingLanguages}
          programmingLanguagesIsLoading={isLoadingLang}
        />
        <EducationSection
          educations={educations}
          isLoading={isLoadingEdu}
        />
        <ExperienceSection
          experiences={experiences}
          isLoading={isLoadingExp}
        />
      </main>
    </>
  )
}

export default CoverPage
