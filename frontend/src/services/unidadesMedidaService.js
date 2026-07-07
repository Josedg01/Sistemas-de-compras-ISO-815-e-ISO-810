import api from './api'

const ENDPOINT = '/unidades-medida'

export const getUnidadesMedida = async (estado) => {
  const params = estado ? { estado } : {}
  const response = await api.get(ENDPOINT, { params })
  return response.data
}

export const getUnidadMedidaById = async (id) => {
  const response = await api.get(`${ENDPOINT}/${id}`)
  return response.data
}

export const createUnidadMedida = async (data) => {
  const response = await api.post(ENDPOINT, data)
  return response.data
}

export const updateUnidadMedida = async (id, data) => {
  const response = await api.put(`${ENDPOINT}/${id}`, data)
  return response.data
}

export const deleteUnidadMedida = async (id) => {
  const response = await api.delete(`${ENDPOINT}/${id}`)
  return response.data
}
