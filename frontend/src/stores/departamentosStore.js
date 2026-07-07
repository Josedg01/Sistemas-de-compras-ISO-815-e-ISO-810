import { defineStore } from 'pinia'
import { getDepartamentos, createDepartamento, updateDepartamento, deleteDepartamento } from '../services/departamentosService'

export const useDepartamentosStore = defineStore('departamentos', {
  state: () => ({
    departamentos: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchDepartamentos() {
      this.isLoading = true
      try {
        this.departamentos = await getDepartamentos()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async addDepartamento(data) {
      this.isLoading = true
      try {
        const nuevo = await createDepartamento(data)
        this.departamentos.push(nuevo)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async editDepartamento(id, data) {
      this.isLoading = true
      try {
        const actualizado = await updateDepartamento(id, data)
        const index = this.departamentos.findIndex(d => d.id === id)
        if (index !== -1) {
          this.departamentos[index] = actualizado
        }
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async removeDepartamento(id) {
      this.isLoading = true
      try {
        await deleteDepartamento(id)
        this.departamentos = this.departamentos.filter(d => d.id !== id)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    }
  }
})
