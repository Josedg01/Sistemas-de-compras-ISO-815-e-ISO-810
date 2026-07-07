import api from './api'

const ENDPOINT = '/asientos-contables'

export const getAsientosContables = async (estado, ordenCompraNumero) => {
  const params = {}
  if (estado) params.estado = estado
  if (ordenCompraNumero) params.ordenCompraNumero = ordenCompraNumero
  const response = await api.get(ENDPOINT, { params })
  return response.data
}

export const getAsientoContableById = async (id) => {
  const response = await api.get(`${ENDPOINT}/${id}`)
  return response.data
}

export const reenviarAsientoContable = async (id) => {
  const response = await api.post(`${ENDPOINT}/${id}/reenviar`)
  return response.data
}
