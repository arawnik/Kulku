'use client'

import Head from 'next/head'
import Image from 'next/image'
import { useAppContext } from '@/app-context'
import useProjects from '@/hooks/useProjects'
import { JSX } from 'react'

const ProjectsPage = (): JSX.Element => {
  const { t } = useAppContext()
  const { data: projects, isLoading, error } = useProjects()

  if (error) {
    return <div className="container">{t('errorLoadingData')}</div>
  }

  return (
    <>
      <Head>
        <title>{t('projects')}</title>
      </Head>
      <main className="container">
        <h2 className="popout-font">{t('projects')}</h2>

        <div className="featurette-container">
          {isLoading
            ? Array(2)
                .fill(null)
                .map((_, index) => (
                  <div key={index}>
                    <hr className="featurette-divider" />
                    <div className={`row featurette ${index % 2 !== 0 ? 'order-md-2' : ''}`}>
                      <div className={`col-md-7 ${index % 2 !== 0 ? 'order-md-2' : ''} d-flex flex-column`}>
                        <div className="align-items-stretch mb-auto">
                          <h3 className="featurette-heading">
                            <span className="main-header placeholder col-4"></span>{' '}
                            <span className="text-muted placeholder col-3"></span>
                          </h3>
                          <p>
                            <span className="placeholder col-8 mb-2"></span>
                            <span className="placeholder col-6 mb-2"></span>
                            <span className="placeholder col-7"></span>
                          </p>
                        </div>
                        <div className="align-items-end">
                          <div className="info-block">
                            <span className="badge list-badge placeholder col-2"></span>
                            <span className="badge list-badge placeholder col-3 ms-1"></span>
                            <span className="badge list-badge placeholder col-2 ms-1"></span>
                          </div>
                          <p className="mt-2">
                            <span className="placeholder col-3"></span>
                          </p>
                        </div>
                      </div>
                      <div className={`col-md-5 ${index % 2 !== 0 ? 'order-md-1' : ''}`}>
                        <div
                          className="placeholder"
                          style={{ width: '100%', height: '300px', backgroundColor: '#a3a6ad' }}
                        ></div>
                      </div>
                    </div>
                  </div>
                ))
            : projects?.map((project, index) => (
                <div key={index}>
                  <hr className="featurette-divider" />
                  <div className={`row featurette ${index % 2 !== 0 ? 'order-md-2' : ''}`}>
                    <div className={`col-md-7 ${index % 2 !== 0 ? 'order-md-2' : ''} d-flex flex-column`}>
                      <div className="align-items-stretch mb-auto">
                        <h3 className="featurette-heading">
                          <span className="main-header">{project.name}</span>{' '}
                          <span className="text-muted">{project.info}</span>
                        </h3>
                        <p dangerouslySetInnerHTML={{ __html: project.description }}></p>
                      </div>
                      <div className="align-items-end">
                        <div className="info-block">
                          {project.keywords.map((keyword, keywordIndex) => (
                            <span
                              key={keywordIndex}
                              className="badge list-badge"
                            >
                              {keyword.name}
                            </span>
                          ))}
                        </div>
                        <a
                          href={project.url}
                          target="_blank"
                          rel="noopener noreferrer"
                        >
                          <i className="bi bi-box-arrow-up-right"></i> {t('checkItOut')}
                        </a>
                      </div>
                    </div>
                    <div className={`col-md-5 ${index % 2 !== 0 ? 'order-md-1' : ''}`}>
                      <Image
                        className="border"
                        src={`https://jerejunttila.fi/media/${project.imageUrl}`}
                        alt={`${project.name} showcase`}
                        width={500}
                        height={300}
                        priority
                      />
                    </div>
                  </div>
                </div>
              ))}
        </div>
      </main>
    </>
  )
}

export default ProjectsPage
