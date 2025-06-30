import { useQuery } from '@tanstack/react-query'
import apiFetch from '@/utils/apiClient'
import { Introduction } from '@/app/models'
import { useAppContext } from '@/app-context'

const useIntroduction = () => {
  const { i18n } = useAppContext()
  return useQuery<Introduction | undefined>({
    queryKey: ['introduction', i18n.language],
    queryFn: () => apiFetch<Introduction | undefined>('/introduction/'),
  })
}

export default useIntroduction
