import { useQuery } from '@tanstack/react-query'
import { Project } from '@/app/projects/models'
import { useLanguage } from '@/language-context'
import apiFetch from '@/utils/apiClient'

const useProjects = () => {
  const { language } = useLanguage()
  return useQuery<Project[]>({
    queryKey: ['projects', language],
    queryFn: () => apiFetch<Project[]>('/project/', language),
  })
}

export default useProjects
