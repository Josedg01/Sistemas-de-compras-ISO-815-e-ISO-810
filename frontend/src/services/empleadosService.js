import api from './api'

const ENDPOINT = '/empleados'

export const getEmpleados = async (estado, departamentoId) => {
  const params = {}
  if (estado) params.estado = estado
  if (departamentoId) params.departamentoId = departamentoId
  const response = await api.get(ENDPOINT, { params })
  return response.data
}

export const getEmpleadoById = async (id) => {
  const response = await api.get(`${ENDPOINT}/${id}`)
  return response.data
}

export const createEmpleado = async (data) => {
  const response = await api.post(ENDPOINT, data)
  return response.data
}

export const updateEmpleado = async (id, data) => {
  const response = await api.put(`${ENDPOINT}/${id}`, data)
  return response.data
}

export const deleteEmpleado = async (id) => {
  const response = await api.delete(`${ENDPOINT}/${id}`)
  return response.data
}
