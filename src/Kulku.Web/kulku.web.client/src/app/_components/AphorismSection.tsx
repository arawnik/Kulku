import { JSX } from 'react'

const AphorismSection = (): JSX.Element => {
  return (
    <div className="md:mx-auto mx-2 my-4 max-w-4xl">
      <p className="text-md text-accent-alt text-center">
        “Your present circumstances don&apos;t determine where you can go; they merely determine
        where you start”
      </p>
      <p className="text-foreground-muted text-center text-sm italic">&mdash; Nido Qubein</p>
    </div>
  )
}

export default AphorismSection
