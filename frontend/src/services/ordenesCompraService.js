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

export const updateOrdenCompra = async (id, data) => {
  // Alias temporal o mock si no hay PUT real en el backend, 
  // O podemos llamar a un endpoint de update si existiera.
  console.warn("updateOrdenCompra is not implemented in backend, returning mock");
  return data;
}

export const enviarAsientoContable = async (asientoData) => {
  // En el nuevo backend, la contabilización se hace automáticamente al "recibir" la orden.
  // Pero mantenemos la firma para evitar errores de sintaxis en el store.
  console.warn("enviarAsientoContable mock llamado");
  return { success: true, asiento: asientoData };
}
