import { useQuery } from '@tanstack/react-query'
import apiFetch from '@/utils/apiClient'
import { Project } from '@/app/projects/models'
import { useAppContext } from '@/app-context'

const useProjects = () => {
  const { i18n } = useAppContext()
  return useQuery<Project[]>({
    queryKey: ['projects', i18n.language],
    queryFn: () => apiFetch<Project[]>('/project/'),
  })
}

export default useProjects
