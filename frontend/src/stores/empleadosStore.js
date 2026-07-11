import { defineStore } from 'pinia'
import { getEmpleados, createEmpleado, updateEmpleado, deleteEmpleado } from '../services/empleadosService'

export const useEmpleadosStore = defineStore('empleados', {
  state: () => ({
    empleados: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchEmpleados() {
      this.isLoading = true
      try {
        this.empleados = await getEmpleados()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async addEmpleado(data) {
      this.isLoading = true
      try {
        const nuevo = await createEmpleado(data)
        this.empleados.push(nuevo)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async editEmpleado(id, data) {
      this.isLoading = true
      try {
        const actualizado = await updateEmpleado(id, data)
        const index = this.empleados.findIndex(e => e.id === id)
        if (index !== -1) {
          this.empleados[index] = actualizado
        }
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async removeEmpleado(id) {
      this.isLoading = true
      try {
        await deleteEmpleado(id)
        this.empleados = this.empleados.filter(e => e.id !== id)
      } catch (err) {
        this.error = err.message
        throw err
      } finally {
        this.isLoading = false
      }
    }
  }
})
