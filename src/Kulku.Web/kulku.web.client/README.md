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
# or
yarn install
```


### 3. Run in Development
```bash
npm run dev
# or
yarn dev
```
The site will be available at `http://localhost:3000`.


### 4. Build & Start
```bash
npm run build
npm run start
# or
yarn build
yarn start
```


## 🐳 Docker (Optional)

0. Env vars are loaded from the top-level `.env.docker` (by default no SSL, api port 5144).
1. From repo root:
   ```bash
   docker-compose up -d --build client
   ```
