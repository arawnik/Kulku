# Kulku.Web.Client

Public-facing [React](https://react.dev) (with [Next.js](https://nextjs.org) framework) front-end site.

## 🚀 Quick Start

### Prerequisites

- Node.js (≥22) & npm or Yarn
- [.NET 10 API backend](../Kulku.Web.Api) running at `https://localhost:7219`
- (Optional) Docker & Docker Compose

### 1. Clone & Configure

```bash
git clone git@github.com:arawnik/Kulku.git
cd src/Kulku.Web/kulku.web.client
```

Copy `.env.template` to `.env` and fill in secrets.

### 2. Install Dependencies

```bash
npm install
```

### 3. Run in Development

```bash
npm run dev
```

The site will be available at `http://localhost:3000`.

Default language subdomains:

```
http://fi.localhost:3000
http://en.localhost:3000
```

### 4. Build & Start

```bash
npm run build
npm run start
```

## 🧩 Architecture Highlights

- Server Components for nearly all data fetching (SEO-friendly, cached, tagged revalidation).
- Typed API fetch layer (apiFetch) ensuring consistent headers and language negotiation.
- Collapsible keyword state persisted in localStorage per language.
- Server Actions for contact form submission:
  - ReCAPTCHA validated server-side
  - Cookie-guarded thank-you page
  - No client-side secrets needed
- Tailwind styling with semantic tokens.
- Reusable UI sections.

## 🌐 Internationalization

This project uses next-intl (App Router) configured for language-by-hostname routing.

Examples:

- Finnish: fi.myname.com
- English: en.myname.com

The active language is derived on the server using headers() and mapped via environment variables. All API requests include a typed language: Language parameter for localized backend responses.

## 🐳 Docker (Optional)

0. Env vars are loaded from the top-level `.env.docker` (by default no SSL, api port 5144).
1. From repo root:
   ```bash
   docker-compose up -d --build client
   ```
