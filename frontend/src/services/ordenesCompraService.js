import api from './api'

const ENDPOINT = '/ordenes-compra'

export const getOrdenesCompra = async (filtros = {}) => {
  const response = await api.get(ENDPOINT, { params: filtros })
  return response.data
}

export const getOrdenCompraByNumero = async (numero) => {
  const response = await api.get(`${ENDPOINT}/${numero}`)
  return response.data
}

export const createOrdenCompra = async (data) => {
  const response = await api.post(ENDPOINT, data)
  return response.data
}

export const aprobarOrdenCompra = async (numero) => {
  const response = await api.post(`${ENDPOINT}/${numero}/aprobar`)
  return response.data
}

export const recibirOrdenCompra = async (numero) => {
  const response = await api.post(`${ENDPOINT}/${numero}/recibir`)
  return response.data
}

export const cancelarOrdenCompra = async (numero) => {
  const response = await api.post(`${ENDPOINT}/${numero}/cancelar`)
  return response.data
}
