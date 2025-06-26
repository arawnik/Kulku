import type { NextConfig } from 'next'

const nextConfig: NextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'jerejunttila.fi',
        port: '',
        pathname: '/media/**',
      },
    ],
  },
  sassOptions: {
    quietDeps: true, // Suppresses deprecation warnings from dependencies like Bootstrap
  },
  reactStrictMode: true,
  output: 'standalone',
}

export default nextConfig
