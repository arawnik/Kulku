import { JSX } from 'react'

interface HrHeaderProps {
  title: string
  subTitle: string
}

const PageHeader = ({ title, subTitle }: HrHeaderProps): JSX.Element => {
  return (
    <header className="mb-6 space-y-2">
      <div>
        <h1 className="font-orbitron text-accent text-3xl tracking-tight">{title}</h1>
        <p className="text-md text-foreground-muted">{subTitle}</p>
      </div>
      <hr className="text-foreground-muted opacity-25" />
    </header>
  )
}

export default PageHeader
