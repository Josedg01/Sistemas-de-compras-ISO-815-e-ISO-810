import api from './api'

const ENDPOINT = '/proveedores'

export const getProveedores = async (estado) => {
  const params = estado ? { estado } : {}
  const response = await api.get(ENDPOINT, { params })
  return response.data
}

export const getProveedorById = async (id) => {
  const response = await api.get(`${ENDPOINT}/${id}`)
  return response.data
}

export const createProveedor = async (data) => {
  const response = await api.post(ENDPOINT, data)
  return response.data
}

export const updateProveedor = async (id, data) => {
  const response = await api.put(`${ENDPOINT}/${id}`, data)
  return response.data
}

export const deleteProveedor = async (id) => {
  const response = await api.delete(`${ENDPOINT}/${id}`)
  return response.data
}
