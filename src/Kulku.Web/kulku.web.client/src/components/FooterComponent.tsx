'use client'
import { JSX } from 'react'

const FooterComponent = (): JSX.Element => {
  const currentYear = new Date().getFullYear()

  return (
    <footer className="footer mt-auto py-3 text-center bg-dark">
      <div className="container">
        <span>&copy; Jere Junttila 2018 - {currentYear}</span>
      </div>
    </footer>
  )
}

export default FooterComponent
