import api from './api'

const ENDPOINT = '/articulos'

export const getArticulos = async () => {
  // return (await api.get(ENDPOINT)).data
  return new Promise(resolve => setTimeout(() => resolve([
    { id: 1, descripcion: 'Laptop Dell Inspiron', marca: 'Dell', unidadMedidaId: 1, existencia: 15, estado: 'Activo' },
    { id: 2, descripcion: 'Resma de Papel A4', marca: 'HP', unidadMedidaId: 2, existencia: 100, estado: 'Activo' },
  ]), 500))
}

export const createArticulo = async (data) => {
  // return await api.post(ENDPOINT, data)
  return new Promise(resolve => setTimeout(() => resolve({ ...data, id: Date.now() }), 500))
}

export const updateArticulo = async (id, data) => {
  // return await api.put(`${ENDPOINT}/${id}`, data)
  return new Promise(resolve => setTimeout(() => resolve(data), 500))
}

export const deleteArticulo = async (id) => {
  // return await api.delete(`${ENDPOINT}/${id}`)
  return new Promise(resolve => setTimeout(() => resolve({ success: true }), 500))
}
