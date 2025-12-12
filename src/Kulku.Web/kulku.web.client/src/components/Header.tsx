'use client'

import { useState, JSX } from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { useTranslations } from 'next-intl'
import { FaBars, FaHamburger, FaGithub, FaLinkedin } from 'react-icons/fa'
import LanguageSwitcher from '@/components/LanguageSwitcher'
import Socials, { type SocialLink } from '@/components/Socials'

const navItems = [
  { href: '/', key: 'cover' },
  { href: '/projects', key: 'projects' },
  { href: '/contact', key: 'contact' },
]

const socialLinks: SocialLink[] = [
  { href: 'https://github.com/arawnik', label: 'GitHub', icon: <FaGithub className="h-4 w-4" /> },
  {
    href: 'https://linkedin.com/in/jerejunttila',
    label: 'LinkedIn',
    icon: <FaLinkedin className="h-4 w-4" />,
  },
]

const Header = (): JSX.Element => {
  const t = useTranslations('header')
  const pathname = usePathname()
  const [mobileOpen, setMobileOpen] = useState(false)

  const isActive = (href: string) => (href === '/' ? pathname === '/' : pathname.startsWith(href))

  return (
    <header className="border-accent/40 bg-background/90 sticky top-0 z-40 w-full border-b backdrop-blur">
      <div className="mx-auto flex max-w-5xl items-center justify-between px-4 py-3">
        {/* DESKTOP */}
        <nav className="text-md hidden flex-1 items-center gap-6 md:flex">
          {navItems.map((item) => {
            const active = isActive(item.href)
            return (
              <Link
                key={item.href}
                href={item.href}
                className={`font-medium font-semibold whitespace-nowrap transition-colors ${
                  active
                    ? 'text-accent hover:text-accent-alt'
                    : 'text-foreground-muted hover:text-foreground'
                }`}
              >
                {t(item.key)}
              </Link>
            )
          })}
        </nav>
        <div className="hidden items-center gap-3 md:flex">
          <LanguageSwitcher />

          <div className="flex items-center gap-2">
            <Socials
              socialLinks={socialLinks}
              downloadLabel={t('downloadableCV')}
            />
          </div>
        </div>

        {/* MOBILE */}
        <div className="flex w-full items-center justify-between md:hidden">
          <nav className="text-md items-center gap-6 flex">
            {navItems.map((item) => {
              const active = isActive(item.href)
              return (
                <Link
                  key={item.href}
                  href={item.href}
                  className={`py-1 text-center font-medium font-semibold transition-colors ${active ? 'text-accent' : 'text-foreground-muted hover:text-foreground'
                    }`}
                  onClick={() => setMobileOpen(false)}
                >
                  {t(item.key)}
                </Link>
              )
            })}
          </nav>

          <button
            type="button"
            onClick={() => setMobileOpen((v) => !v)}
            aria-label="Toggle navigation"
            className={`border-accent/60 hover:bg-accent hover:text-background/90 flex h-10 w-10 items-center justify-center rounded-md border transition-colors ${
              mobileOpen
                ? 'bg-accent text-background hover:bg-accent-alt'
                : 'hover:bg-accent hover:text-background/90'
            }`}
          >
            {mobileOpen ? (
              <FaHamburger className="text-foreground-muted h-5 w-5 transition-transform duration-200" />
            ) : (
              <FaBars className="text-foreground-muted h-5 w-5 transition-transform duration-200" />
            )}
          </button>
        </div>
      </div>
      {mobileOpen && (
        <div className="border-accent/40 bg-background border-t md:hidden">
          <div className="mx-auto flex max-w-5xl flex-col gap-4 px-4 py-4 text-sm">
            <div className="flex justify-center">
              <LanguageSwitcher />
            </div>

            <hr className="text-foreground-muted opacity-25" />

            <div className="flex flex-wrap items-center justify-center gap-3 pt-1">
              <Socials
                socialLinks={socialLinks}
                downloadLabel={t('downloadableCV')}
              />
            </div>
          </div>
        </div>
      )}
    </header>
  )
}

export default Header
