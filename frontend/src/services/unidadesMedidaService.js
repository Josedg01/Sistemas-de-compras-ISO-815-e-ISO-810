import api from './api'

const ENDPOINT = '/unidades-medida'

export const getUnidadesMedida = async () => {
  // return (await api.get(ENDPOINT)).data
  return new Promise(resolve => setTimeout(() => resolve([
    { id: 1, descripcion: 'Unidad', estado: 'Activo' },
    { id: 2, descripcion: 'Caja', estado: 'Activo' },
    { id: 3, descripcion: 'Kilogramo', estado: 'Activo' },
  ]), 500))
}

export const createUnidadMedida = async (data) => {
  // return await api.post(ENDPOINT, data)
  return new Promise(resolve => setTimeout(() => resolve({ ...data, id: Date.now() }), 500))
}

export const updateUnidadMedida = async (id, data) => {
  // return await api.put(`${ENDPOINT}/${id}`, data)
  return new Promise(resolve => setTimeout(() => resolve(data), 500))
}

export const deleteUnidadMedida = async (id) => {
  // return await api.delete(`${ENDPOINT}/${id}`)
  return new Promise(resolve => setTimeout(() => resolve({ success: true }), 500))
}
