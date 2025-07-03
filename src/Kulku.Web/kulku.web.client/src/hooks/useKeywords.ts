import { useQuery } from '@tanstack/react-query'
import { Keyword, KeywordType } from '@/app/models'
import { useLanguage } from '@/language-context'
import apiFetch from '@/utils/apiClient'

const useKeywords = (type: KeywordType) => {
  const { language } = useLanguage()
  return useQuery<Keyword[]>({
    queryKey: ['keywords', type, language],
    queryFn: () => apiFetch<Keyword[]>(`/project/keywords/${type}`, language),
  })
}

export default useKeywords
