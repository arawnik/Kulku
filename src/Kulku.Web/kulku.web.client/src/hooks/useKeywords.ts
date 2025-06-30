import { useQuery } from '@tanstack/react-query'
import apiFetch from '@/utils/apiClient'
import { Keyword, KeywordType } from '@/app/models'
import { useAppContext } from '@/app-context'

const useKeywords = (type: KeywordType) => {
  const { i18n } = useAppContext()
  return useQuery<Keyword[]>({
    queryKey: ['keywords', type, i18n.language],
    queryFn: () => apiFetch<Keyword[]>(`/project/keywords/${type}`),
  })
}

export default useKeywords
