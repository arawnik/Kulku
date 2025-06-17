import type { NextConfig } from 'next'

const nextConfig: NextConfig = {
  sassOptions: {
    quietDeps: true, // Suppresses deprecation warnings from dependencies like Bootstrap
  },
  reactStrictMode: true,
  output: 'standalone',
}

export default nextConfig
