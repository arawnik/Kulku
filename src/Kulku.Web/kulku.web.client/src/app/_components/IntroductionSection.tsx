import { type Introduction } from '@/lib/api/introduction'
import { JSX } from 'react'

interface IntroductionSectionProps {
  intro: Introduction | undefined
}

const IntroductionSection = ({ intro }: IntroductionSectionProps): JSX.Element | null => {
  if (!intro) return null

  return (
    <div className="my-4 md:py-4">
      <div className="border-accent mx-auto max-w-xl overflow-hidden rounded-md border md:max-w-5xl md:overflow-visible">
        <div className="md:flex">
          <div className="md:-my-4 md:ms-5 md:shrink-0">
            <img
              className="h-80 w-full object-cover object-[center_8%] transition-transform md:h-full md:w-90 md:scale-100 md:rounded md:object-center"
              src={`/static/profile/${intro.avatarUrl}`}
              alt="Avatar"
            />
          </div>
          <div className="px-5 py-5 lg:px-8">
            <h1 className="text-6xl">Jere Junttila</h1>
            <p className="font-orbitron text-foreground-muted mb-2 text-xl">
              &lt; {intro.title} &gt;
            </p>
            <div
              className="mb-2"
              dangerouslySetInnerHTML={{ __html: intro.content }}
            />
            <p className="text-accent-alt text-lg">{intro.tagline}</p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default IntroductionSection
