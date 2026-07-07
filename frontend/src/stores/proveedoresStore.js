import { defineStore } from 'pinia'
import { getProveedores, createProveedor, updateProveedor, deleteProveedor } from '../services/proveedoresService'

export const useProveedoresStore = defineStore('proveedores', {
  state: () => ({
    proveedores: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchProveedores() {
      this.isLoading = true
      try {
        this.proveedores = await getProveedores()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async addProveedor(data) {
      this.isLoading = true
      try {
        const nuevo = await createProveedor(data)
        this.proveedores.push(nuevo)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async editProveedor(id, data) {
      this.isLoading = true
      try {
        const actualizado = await updateProveedor(id, data)
        const index = this.proveedores.findIndex(p => p.id === id)
        if (index !== -1) this.proveedores[index] = actualizado
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async removeProveedor(id) {
      this.isLoading = true
      try {
        await deleteProveedor(id)
        this.proveedores = this.proveedores.filter(p => p.id !== id)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    }
  }
})
