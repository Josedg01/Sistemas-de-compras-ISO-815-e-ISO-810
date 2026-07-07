import api from './api'

const ENDPOINT = '/departamentos'

export const getDepartamentos = async () => {
  // Cuando el backend esté listo, descomentar la siguiente línea y borrar el mock
  // const response = await api.get(ENDPOINT)
  // return response.data

  // --- MOCK DATA ---
  return new Promise(resolve => {
    setTimeout(() => {
      resolve([
        { id: 1, nombre: 'Recursos Humanos', estado: 'Activo' },
        { id: 2, nombre: 'Tecnología', estado: 'Activo' },
        { id: 3, nombre: 'Mantenimiento', estado: 'Inactivo' },
      ])
    }, 500)
  })
}

export const createDepartamento = async (data) => {
  // return await api.post(ENDPOINT, data)
  return new Promise(resolve => setTimeout(() => resolve({ ...data, id: Date.now() }), 500))
}

export const updateDepartamento = async (id, data) => {
  // return await api.put(`${ENDPOINT}/${id}`, data)
  return new Promise(resolve => setTimeout(() => resolve(data), 500))
}

export const deleteDepartamento = async (id) => {
  // return await api.delete(`${ENDPOINT}/${id}`)
  return new Promise(resolve => setTimeout(() => resolve({ success: true }), 500))
}
