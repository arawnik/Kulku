import { JSX } from 'react'
import { Introduction } from '@/app/models'

interface IntroductionSectionProps {
  intro: Introduction | undefined
  isLoading: boolean
}

const IntroductionSection = ({ intro }: IntroductionSectionProps): JSX.Element => (
  <section>
    <div className="container container-narrow text-center">
      <p className="aphorism">
        “Your present circumstances don&apos;t determine where you can go; they merely determine where you start”
      </p>
      <p className="author">&mdash; Nido Qubein</p>
    </div>
    <div className="container container-narrow my-5">
      {intro ? (
        <div className="row py-0 d-flex flex-wrap align-items-center justify-content-center justify-content-md-between rounded-2 well">
          <div className="col-md-6 col-lg-5 p-0">
            <div className="avatar-image ms-md-3 ms-lg-4 ms-auto me-auto">
              <picture>
                <source
                  media="(max-width:767px)"
                  srcSet={'/static/profile/' + intro.smallAvatarUrl}
                  width="400"
                  height="208"
                />
                <img
                  src={'/static/profile/' + intro.avatarUrl}
                  className="img-fluid"
                  alt="Avatar"
                  width="400"
                  height="673"
                />
              </picture>
            </div>
          </div>
          <div className="col-md-6 col-lg-7 p-3 mt-md-3 align-self-md-center">
            <h1 className="easy-font display-4 mb-0">Jere Junttila</h1>
            <p className="popout-font header-subinfo">&lt; {intro.title} &gt;</p>
            <div dangerouslySetInnerHTML={{ __html: intro.content }} />
            <p className="lead">{intro.tagline}</p>
          </div>
        </div>
      ) : (
        // Placeholders
        <div className="row py-0 d-flex flex-wrap align-items-center justify-content-center justify-content-md-between rounded-2 well">
          <div className="col-md-6 col-lg-5 p-0">
            <div className="avatar-image ms-md-3 ms-lg-4 ms-auto me-auto">
              <div
                className="placeholder col-12"
                style={{ height: '673px', width: '100%', backgroundColor: '#a3a6ad' }}
              ></div>
            </div>
          </div>
          <div className="col-md-6 col-lg-7 p-3 mt-md-3 align-self-md-center">
            <h1 className="easy-font display-4 mb-0">
              <span className="placeholder col-6"></span>
            </h1>
            <p className="popout-font header-subinfo">
              <span className="placeholder col-2 mb-2"></span>
              <span className="placeholder col-2 mb-2"></span>
            </p>
            <div className="content-placeholder">
              <p className="mb-2">
                <span className="placeholder col-10 mb-2"></span>
                <span className="placeholder col-8 mb-2"></span>
                <span className="placeholder col-9 mb-2"></span>
              </p>
              <p className="mb-2">
                <span className="placeholder col-9 mb-2"></span>
                <span className="placeholder col-7 mb-2"></span>
                <span className="placeholder col-8 mb-2"></span>
              </p>
              <p className="mb-2">
                <span className="placeholder col-10 mb-2"></span>
                <span className="placeholder col-6 mb-2"></span>
              </p>
            </div>
            <p className="lead">
              <span className="placeholder col-8"></span>
            </p>
          </div>
        </div>
      )}
    </div>
  </section>
)

export default IntroductionSection
