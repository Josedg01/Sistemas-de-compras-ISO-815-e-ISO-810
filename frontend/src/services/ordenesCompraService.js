import api from './api'

const ENDPOINT = '/ordenes-compra'
const ENDPOINT_CONTABILIDAD = '/ws-contabilidad/asientos'

export const getOrdenesCompra = async () => {
  // return (await api.get(ENDPOINT)).data
  return new Promise(resolve => setTimeout(() => resolve([
    { id: 1, numeroOrden: 'OC-2023-001', fechaOrden: '2023-10-15', estado: 'Aprobada', articuloId: 1, cantidad: 5, unidadMedidaId: 1, costoUnitario: 800 },
  ]), 500))
}

export const createOrdenCompra = async (data) => {
  // return await api.post(ENDPOINT, data)
  return new Promise(resolve => setTimeout(() => resolve({ ...data, id: Date.now() }), 500))
}

export const updateOrdenCompra = async (id, data) => {
  // return await api.put(`${ENDPOINT}/${id}`, data)
  return new Promise(resolve => setTimeout(() => resolve(data), 500))
}

export const enviarAsientoContable = async (asientoData) => {
  // Endpoint de integración simulado
  // return await api.post(ENDPOINT_CONTABILIDAD, asientoData)
  console.log("Enviando a Contabilidad: ", asientoData);
  return new Promise(resolve => setTimeout(() => resolve({ success: true, asiento: asientoData }), 800))
}
