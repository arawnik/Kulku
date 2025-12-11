import { JSX, ReactNode } from 'react'

export type TimelineItem = {
  id: string
  left: ReactNode
  right: ReactNode
}

interface TimelineProps {
  items: TimelineItem[]
}

const Timeline = ({ items }: TimelineProps): JSX.Element | null => {
  if (!items) return null

  return (
    <section className="relative">
      <div className="bg-accent pointer-events-none absolute top-0 left-4 h-full w-[3px] md:left-1/2 md:-translate-x-1/2" />

      <div className="space-y-4">
        {items.map((item) => {
          if (!item) return null

          return (
            <div
              key={item.id}
              className="relative grid grid-cols-1 gap-x-4 gap-y-1 md:grid-cols-2"
            >
              <span className="bg-background pointer-events-none absolute top-2.5 left-4 h-3 w-3 -translate-x-1/2 rounded-full border border-gray-500 md:left-1/2" />
              <div className="pl-8 text-left md:pr-6 md:pl-0 md:text-right">{item.left}</div>
              <div className="pl-8 md:pl-4">{item.right}</div>
            </div>
          )
        })}
      </div>
    </section>
  )
}

export default Timeline
