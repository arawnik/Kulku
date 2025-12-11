import { JSX } from 'react'
import Image from 'next/image'
import { getTranslations } from 'next-intl/server'
import { getCurrentLanguage, type Language } from '@/i18n/language'
import { getProjects } from '@/lib/api/project'
import { type Metadata } from 'next'
import PageHeader from '@/components/PageHeader'

export const generateMetadata = async (): Promise<Metadata> => {
  const t = await getTranslations('projects')
  return {
    title: t('title'),
    description: t('description'),
  }
}

const ProjectsPage = async (): Promise<JSX.Element> => {
  const t = await getTranslations('projects')
  const language: Language = await getCurrentLanguage()

  const [projects] = await Promise.all([getProjects(language)])

  const sorted = projects.slice().sort((a, b) => a.order - b.order)

  return (
    <section className="w-full max-w-5xl space-y-8 px-4 py-8">
      <PageHeader
        title={t('title')}
        subTitle={t('subTitle')}
      />

      <div className="space-y-12">
        {sorted.map((project, index) => {
          const isOdd = index % 2 === 1
          const textColClass = `space-y-3 ${isOdd ? 'md:order-2' : ''}`
          const imageColClass = `${isOdd ? 'md:order-1' : ''}`

          return (
            <article
              key={project.name}
              className="grid gap-6 md:grid-cols-2 md:items-center"
            >
              <div className={textColClass}>
                <header className="space-y-1">
                  <h3 className="text-xl font-semibold">{project.name}</h3>
                  {project.info && <p className="text-foreground-muted text-sm">{project.info}</p>}
                </header>

                {project.description && (
                  <p
                    className="text-sm leading-relaxed"
                    dangerouslySetInnerHTML={{ __html: project.description }}
                  />
                )}

                {project.keywords?.length ? (
                  <div className="space-y-2">
                    <ul className="flex flex-wrap gap-1.5">
                      {project.keywords
                        .filter((k) => k.display)
                        .sort((a, b) => a.order - b.order)
                        .map((keyword) => (
                          <li
                            key={`${project.name}-${keyword.name}`}
                            className="border-accent rounded-md border px-2 py-0.5 text-[11px] font-medium"
                          >
                            {keyword.name}
                          </li>
                        ))}
                    </ul>
                  </div>
                ) : null}

                {project.url && (
                  <p className="pt-1 text-sm">
                    <a
                      href={project.url}
                      target="_blank"
                      rel="noreferrer"
                      className="text-accent inline-flex items-center gap-1 underline-offset-2 hover:underline"
                    >
                      <span>{t('viewLink')}</span>
                      <span
                        aria-hidden="true"
                        className="text-xs"
                      >
                        ↗
                      </span>
                    </a>
                  </p>
                )}
              </div>
              <div className={imageColClass}>
                <div className="border-accent/40 relative aspect-[5/3] overflow-hidden rounded-md border bg-zinc-200">
                  {project.imageUrl ? (
                    <Image
                      src={`/static/projects/${project.imageUrl}`}
                      alt={`${project.name} showcase`}
                      fill
                      sizes="(min-width: 768px) 40vw, 100vw"
                      className="object-cover"
                      priority={index === 0}
                    />
                  ) : (
                    <div className="text-foreground-muted flex h-full items-center justify-center text-xs">
                      {t('noImage')}
                    </div>
                  )}
                </div>
              </div>
            </article>
          )
        })}
      </div>
    </section>
  )
}

export default ProjectsPage
