import { defineStore } from 'pinia'
import { getAsientosContables, reenviarAsientoContable } from '../services/asientosContablesService'

export const useAsientosContablesStore = defineStore('asientosContables', {
  state: () => ({
    asientos: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchAsientos() {
      this.isLoading = true
      try {
        this.asientos = await getAsientosContables()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async reenviarAsiento(id) {
      this.isLoading = true
      try {
        const actualizado = await reenviarAsientoContable(id)
        const index = this.asientos.findIndex(a => a.id === id)
        if (index !== -1) {
          this.asientos[index] = actualizado
        }
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    }
  }
})
