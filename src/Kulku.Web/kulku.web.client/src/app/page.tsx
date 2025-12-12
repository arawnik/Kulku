import { JSX } from 'react'
import { getTranslations } from 'next-intl/server'
import { getCurrentLanguage, type Language } from '@/i18n/language'
import { getKeywords, KeywordType } from '@/lib/api/keyword'
import { getIntroduction } from '@/lib/api/introduction'
import { getEducations } from '@/lib/api/education'
import { getExperiences } from '@/lib/api/experience'
import HrHeader from '@/components/HrHeader'
import AphorismSection from './_components/AphorismSection'
import IntroductionSection from './_components/IntroductionSection'
import KeywordSection from './_components/KeywordSection'
import EducationSection from './_components/EducationSection'
import ExperienceSection from './_components/ExperienceSection'

const CoverPage = async (): Promise<JSX.Element> => {
  const t = await getTranslations('home')
  const language: Language = await getCurrentLanguage()

  const [
    introduction,
    languageKeywords,
    skillKeywords,
    technologyKeywords,
    educations,
    experiences,
  ] = await Promise.all([
    getIntroduction(language),
    getKeywords(KeywordType.Language, language),
    getKeywords(KeywordType.Skill, language),
    getKeywords(KeywordType.Technology, language),
    getEducations(language),
    getExperiences(language),
  ])

  return (
    <div>
      <AphorismSection />
      <IntroductionSection intro={introduction} />

      <KeywordSection
        languageKeywords={languageKeywords}
        skillKeywords={skillKeywords}
        technologyKeywords={technologyKeywords}
      />

      <HrHeader>{t('education')}</HrHeader>
      <EducationSection educations={educations} />

      <HrHeader>{t('experience')}</HrHeader>
      <ExperienceSection experiences={experiences} />
    </div>
  )
}

export default CoverPage
