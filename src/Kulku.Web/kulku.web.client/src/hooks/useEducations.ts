import { useQuery } from '@tanstack/react-query'
import { Education } from '@/app/models'
import { useLanguage } from '@/language-context'
import apiFetch from '@/utils/apiClient'

const useEducations = () => {
  const { language } = useLanguage()
  return useQuery<Education[]>({
    queryKey: ['educations', language],
    queryFn: () => apiFetch<Education[]>('/education/', language),
  })
}

export default useEducations
