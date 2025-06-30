export interface Introduction {
  title: string
  content: string
  tagline: string
  avatarUrl: string // URL for the avatar image
  smallAvatarUrl: string // URL for the small avatar image
  pdf: string // URL for the PDF file
}

export interface Company {
  name: string
  description: string
}

export interface Institution {
  name: string
  department?: string
  description: string
}

export interface Proficiency {
  name: string
  description: string
  scale: number
  order: number
}

export enum KeywordType {
  Language = 'LA', // Programming Language
  Skill = 'SK', // Skill
  Technology = 'TE', // Technology
}

export interface Keyword {
  name: string
  type: KeywordType // LANGUAGE, SKILL, or TECHNOLOGY
  proficiency: Proficiency
  order: number
  display: boolean
}

export interface Experience {
  id: number
  title: string
  description: string
  company: Company
  keywords: Keyword[]
  startDate: string // Date in string format
  endDate?: string // Date in string format or null
}

export interface Education {
  title: string
  description: string
  institution: Institution
  startDate: string
  endDate?: string
}

export interface ProblemDetails {
  type: string
  title: string
  status: number
  errors?: Record<string, string[]>
}
