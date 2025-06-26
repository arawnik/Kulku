import { ProblemDetails } from '@/app/models'

export const mapProblemDetailsErrors = (error: unknown): Record<string, string> => {
  if (typeof error === 'object' && error !== null && 'errors' in error) {
    const problem = error as ProblemDetails
    const { errors } = problem

    if (!errors || typeof errors !== 'object') {
      return {}
    }

    return Object.entries(errors).reduce<Record<string, string>>((acc, [field, messages]) => {
      if (Array.isArray(messages) && messages.length > 0) {
        acc[field] = messages[0]
      }
      return acc
    }, {})
  }

  return {}
}
