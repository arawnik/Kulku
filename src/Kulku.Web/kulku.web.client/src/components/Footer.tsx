const Footer = () => {
  return (
    <footer className="border-accent w-full border-t py-6">
      <div className="text-md text-foreground-muted mx-auto max-w-4xl px-4 text-center">
        © 2018 - {new Date().getFullYear()} Jere Junttila
      </div>
    </footer>
  )
}

export default Footer
