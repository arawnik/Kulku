'use client'

import { JSX } from 'react'
import Link from 'next/link'
import Image from 'next/image'
import { useTranslations } from 'next-intl'
import { setCookie } from 'cookies-next'
import { useRouter, usePathname } from 'next/navigation'
import { useAppContext } from '@/app-context'
import { useLanguage } from '@/language-context'

const HeaderComponent = (): JSX.Element => {
  const { theme, toggleTheme } = useAppContext()
  const t = useTranslations()
  const router = useRouter()
  const pathname = usePathname()
  const { oppositeLanguage } = useLanguage()

  const navLinkClass = (path: string) => (pathname === path ? 'active' : '')

  const switchLanguage = () => {
    setCookie('language', oppositeLanguage, {
      path: '/',
      maxAge: 60 * 60 * 24 * 365, // 1 year, maximum age for cookie
    })
    localStorage.setItem('language', oppositeLanguage)
    router.refresh()
  }

  return (
    <header>
      <nav className="px-3 py-1 text-bg-dark fixed-top main-nav">
        <div className="container container-narrow">
          <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between">
            {/* Navigation Links */}
            <ul className="nav col-12 col-md-auto justify-content-center my-md-0 text-center text-small">
              <li>
                <Link
                  href="/"
                  className={`nav-link text-secondary ${navLinkClass('/')}`}
                >
                  <i className="bi bi-mid bi-fire d-block mb-0"></i>
                  {t('intro')}
                </Link>
              </li>
              <li>
                <Link
                  href="/projects"
                  className={`nav-link text-secondary ${navLinkClass('/projects')}`}
                >
                  <i className="bi bi-mid bi-building-gear d-block mb-0"></i>
                  {t('projects')}
                </Link>
              </li>
              <li>
                <Link
                  href="/contact"
                  className={`nav-link text-secondary ${navLinkClass('/contact')}`}
                >
                  <i className="bi bi-mid bi-envelope-paper d-block mb-0"></i>
                  {t('contactMe')}
                </Link>
              </li>
            </ul>

            <ul className="nav col-12 col-md-auto justify-content-center my-md-0 text-center">
              {/* Theme Switch */}
              <li>
                <div className="d-flex">
                  <label
                    htmlFor="changeThemeSwitch"
                    className="form-check-label"
                    data-bs-toggle="tooltip"
                    title={t('lightMode')}
                  >
                    <i className="bi bi-mid bi-lightbulb"></i>
                  </label>
                  <div className="form-switch form-check ms-2">
                    <input
                      className="form-check-input"
                      onChange={toggleTheme}
                      type="checkbox"
                      role="switch"
                      id="changeThemeSwitch"
                      checked={theme === 'dark'}
                      data-bs-toggle="tooltip"
                      title={t('switchTheme')}
                    />
                  </div>
                  <label
                    htmlFor="changeThemeSwitch"
                    className="form-check-label"
                    data-bs-toggle="tooltip"
                    title={t('darkMode')}
                  >
                    <i className="bi bi-mid bi-moon"></i>
                  </label>
                </div>
              </li>

              {/* Language Switch */}
              <li>
                <div
                  className="d-flex ms-2"
                  data-bs-toggle="tooltip"
                  title={t('oppositeLang')}
                  onClick={switchLanguage}
                  style={{ cursor: 'pointer' }}
                >
                  <Image
                    src={`/img/${oppositeLanguage}.svg`}
                    height={26}
                    width={32}
                    alt={oppositeLanguage}
                  />
                </div>
              </li>
            </ul>

            {/* Social Media Links */}
            <ul className="nav col-12 col-md-auto justify-content-center my-md-0 text-center">
              <li>
                <a
                  href="https://www.linkedin.com/in/jerejunttila"
                  className="nav-link text-secondary"
                  target="_blank"
                  rel="noopener noreferrer"
                  title={t('linkedInProfile')}
                >
                  <i className="bi bi-mid bi-linkedin d-block mb-0"></i>
                </a>
              </li>
              <li>
                <a
                  href="https://twitter.com/JereJunttila"
                  className="nav-link text-secondary"
                  target="_blank"
                  rel="noopener noreferrer"
                  title={t('twitterProfile')}
                >
                  <i className="bi bi-mid bi-twitter d-block mb-0"></i>
                </a>
              </li>
              <li>
                <a
                  href="https://github.com/arawnik"
                  className="nav-link text-secondary"
                  target="_blank"
                  rel="noopener noreferrer"
                  title={t('githubProfile')}
                >
                  <i className="bi bi-mid bi-github d-block mb-0"></i>
                </a>
              </li>
              <li>
                <a
                  href="https://jerejunttila.fi/media/pdf/CV_JereJunttila.pdf"
                  className="nav-link text-secondary"
                  target="_blank"
                  rel="noopener noreferrer"
                  title={t('downloadableCV')}
                >
                  <i className="bi bi-mid bi-file-earmark-pdf d-block mb-0"></i>
                </a>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>
  )
}

export default HeaderComponent
