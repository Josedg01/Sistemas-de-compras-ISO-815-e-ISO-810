import api from './api'

const ENDPOINT = '/departamentos'

export const getDepartamentos = async (estado) => {
  const params = estado ? { estado } : {}
  const response = await api.get(ENDPOINT, { params })
  return response.data
}

export const getDepartamentoById = async (id) => {
  const response = await api.get(`${ENDPOINT}/${id}`)
  return response.data
}

export const createDepartamento = async (data) => {
  const response = await api.post(ENDPOINT, data)
  return response.data
}

export const updateDepartamento = async (id, data) => {
  const response = await api.put(`${ENDPOINT}/${id}`, data)
  return response.data
}

export const deleteDepartamento = async (id) => {
  const response = await api.delete(`${ENDPOINT}/${id}`)
  return response.data
}
