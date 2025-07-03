import { useQuery } from '@tanstack/react-query'
import { Introduction } from '@/app/models'
import { useLanguage } from '@/language-context'
import apiFetch from '@/utils/apiClient'

const useIntroduction = () => {
  const { language } = useLanguage()
  return useQuery<Introduction | undefined>({
    queryKey: ['introduction', language],
    queryFn: () => apiFetch<Introduction | undefined>('/introduction/', language),
  })
}

export default useIntroduction
