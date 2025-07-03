import { useQuery } from '@tanstack/react-query'
import { Experience } from '@/app/models'
import { useLanguage } from '@/language-context'
import apiFetch from '@/utils/apiClient'

const useExperiences = () => {
  const { language } = useLanguage()
  return useQuery<Experience[]>({
    queryKey: ['experiences', language],
    queryFn: () => apiFetch<Experience[]>('/experience/', language),
  })
}

export default useExperiences
