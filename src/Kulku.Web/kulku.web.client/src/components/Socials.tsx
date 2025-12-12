import { JSX, ReactNode } from 'react'
import { FaRegFilePdf } from 'react-icons/fa'

export type SocialLink = {
  href: string
  label: string
  icon: ReactNode
}

interface SocialsProps {
  socialLinks: SocialLink[]
  downloadLabel: string
}

const Socials = ({ socialLinks, downloadLabel }: SocialsProps): JSX.Element => {
  return (
    <>
      {socialLinks.map((s) => (
        <a
          key={s.href}
          href={s.href}
          aria-label={s.label}
          title={s.label}
          target="_blank"
          rel="noreferrer"
          className="border-accent/40 hover:bg-accent hover:text-background inline-flex h-8 w-8 items-center justify-center rounded-full border text-sm transition-colors"
        >
          {s.icon}
        </a>
      ))}
      <a
        href="/static/downloads/CV_JereJunttila.pdf"
        title={downloadLabel}
        download
        className="border-accent text-accent hover:bg-accent hover:text-background inline-flex h-8 items-center rounded-md border px-3 transition-colors"
      >
        <FaRegFilePdf className="h-4 w-4" />
      </a>
    </>
  )
}

export default Socials
