import { defineStore } from 'pinia'
import { getUnidadesMedida, createUnidadMedida, updateUnidadMedida, deleteUnidadMedida } from '../services/unidadesMedidaService'

export const useUnidadesMedidaStore = defineStore('unidadesMedida', {
  state: () => ({
    unidades: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchUnidades() {
      this.isLoading = true
      try {
        this.unidades = await getUnidadesMedida()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async addUnidad(data) {
      this.isLoading = true
      try {
        const nuevo = await createUnidadMedida(data)
        this.unidades.push(nuevo)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async editUnidad(id, data) {
      this.isLoading = true
      try {
        const actualizado = await updateUnidadMedida(id, data)
        const index = this.unidades.findIndex(u => u.id === id)
        if (index !== -1) this.unidades[index] = actualizado
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async removeUnidad(id) {
      this.isLoading = true
      try {
        await deleteUnidadMedida(id)
        this.unidades = this.unidades.filter(u => u.id !== id)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    }
  }
})
