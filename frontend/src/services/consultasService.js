import api from './api'

const ENDPOINT = '/consultas'

export const consultarOrdenesCompra = async (filtros = {}) => {
  const response = await api.get(`${ENDPOINT}/ordenes-compra`, { params: filtros })
  return response.data
}
