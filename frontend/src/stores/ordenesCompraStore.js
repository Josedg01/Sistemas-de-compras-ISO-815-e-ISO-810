import { defineStore } from 'pinia'
import { getOrdenesCompra, createOrdenCompra, recibirOrdenCompra } from '../services/ordenesCompraService'

export const useOrdenesCompraStore = defineStore('ordenesCompra', {
  state: () => ({
    ordenes: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchOrdenes() {
      this.isLoading = true
      try {
        this.ordenes = await getOrdenesCompra()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async addOrden(data) {
      this.isLoading = true
      try {
        const nuevo = await createOrdenCompra(data)
        this.ordenes.push(nuevo)
      } catch (err) {
        this.error = err.message
        throw err
      } finally {
        this.isLoading = false
      }
    },
    async recibirOrden(numero) {
      this.isLoading = true
      try {
        const actualizado = await recibirOrdenCompra(numero)
        
        const index = this.ordenes.findIndex(o => o.numero === numero || o.id === numero || o.numeroOrden === numero)
        if (index !== -1) {
          this.ordenes[index] = actualizado
        }
        
        return actualizado
      } catch (err) {
        this.error = err.message
        throw err
      } finally {
        this.isLoading = false
      }
    }
  }
})
