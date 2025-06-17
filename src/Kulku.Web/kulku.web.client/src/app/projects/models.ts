import { Keyword } from '../models'

export interface Project {
  name: string
  info: string
  description: string
  url: string
  order: number
  imageUrl: string
  keywords: Keyword[]
}
