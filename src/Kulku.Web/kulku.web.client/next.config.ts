import type { NextConfig } from 'next'
import createNextIntlPlugin from 'next-intl/plugin'

const nextConfig: NextConfig = {
  sassOptions: {
    quietDeps: true, // Suppresses deprecation warnings from dependencies like Bootstrap
  },
  reactStrictMode: true,
  output: 'standalone',
}

const withNextIntl = createNextIntlPlugin()
export default withNextIntl(nextConfig)
