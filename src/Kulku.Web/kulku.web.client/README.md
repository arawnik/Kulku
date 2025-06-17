# Important Notices

This project is personal work and is not maintained actively. The core functionalities are set up so updates are on inspiration basis.

This is a [React](https://react.dev) (with [Next.js](https://nextjs.org) framework) app version for my CV frontend.

# Getting Started

## Prerequisites

- [Node.js](https://nodejs.org/en) (developed with v22.15.0)

## Testing locally

1. Install dependencies: `npm install`
2. Run the development server: `npm run dev`
3. Open [http://localhost:3000](http://localhost:3000) with your browser

## Using Docker

0. Install [Docker](https://www.docker.com/) on your machine
1. Build your container: `docker build -t jela-react .`
   1. With build variables `docker build --build-arg NEXT_PUBLIC_API_BASE_URL=http://localhost:8083/api -t jela-react .`
2. Run your container: `docker run -p 3000:3000 jela-react`

### Developing

Prettier

```bash
npx prettier . --write
npx prettier . --check
```
