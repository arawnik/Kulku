import { useQuery } from '@tanstack/react-query'
import { apiFetch } from '@/utils/apiClient'
import { Experience } from '@/app/models'
import { useAppContext } from '@/app-context'

export function useExperiences() {
  const { i18n } = useAppContext()
  return useQuery<Experience[]>({
    queryKey: ['experiences', i18n.language],
    queryFn: () => apiFetch<Experience[]>('/experience/'),
  })
}
