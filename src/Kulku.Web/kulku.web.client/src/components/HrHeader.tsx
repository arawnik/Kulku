import { JSX, ReactNode } from 'react'

interface HrHeaderProps {
  children: ReactNode
}

const HrHeader = ({ children }: HrHeaderProps): JSX.Element => {
  return (
    <div className="my-6">
      <hr className="text-foreground-muted opacity-25" />
      <h2 className="text-accent font-orbitron my-4 text-center text-4xl">{children}</h2>
      <hr className="text-foreground-muted opacity-25" />
    </div>
  )
}

export default HrHeader
