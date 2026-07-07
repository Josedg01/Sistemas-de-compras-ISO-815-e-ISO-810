import api from './api'

const ENDPOINT = '/proveedores'

export const getProveedores = async () => {
  // return (await api.get(ENDPOINT)).data
  return new Promise(resolve => setTimeout(() => resolve([
    { id: 1, cedulaRnc: '101001001', nombreComercial: 'Ferretería Popular', estado: 'Activo' },
    { id: 2, cedulaRnc: '101001002', nombreComercial: 'Tech Solutions SA', estado: 'Activo' },
  ]), 500))
}

export const createProveedor = async (data) => {
  // return await api.post(ENDPOINT, data)
  return new Promise(resolve => setTimeout(() => resolve({ ...data, id: Date.now() }), 500))
}

export const updateProveedor = async (id, data) => {
  // return await api.put(`${ENDPOINT}/${id}`, data)
  return new Promise(resolve => setTimeout(() => resolve(data), 500))
}

export const deleteProveedor = async (id) => {
  // return await api.delete(`${ENDPOINT}/${id}`)
  return new Promise(resolve => setTimeout(() => resolve({ success: true }), 500))
}
