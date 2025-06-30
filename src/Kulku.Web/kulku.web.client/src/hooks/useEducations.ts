import { useQuery } from '@tanstack/react-query'
import apiFetch from '@/utils/apiClient'
import { Education } from '@/app/models'
import { useAppContext } from '@/app-context'

const useEducations = () => {
  const { i18n } = useAppContext()
  return useQuery<Education[]>({
    queryKey: ['educations', i18n.language],
    queryFn: () => apiFetch<Education[]>('/education/'),
  })
}

export default useEducations
