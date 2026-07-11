import { defineStore } from 'pinia'
import { getArticulos, createArticulo, updateArticulo, deleteArticulo } from '../services/articulosService'

export const useArticulosStore = defineStore('articulos', {
  state: () => ({
    articulos: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchArticulos() {
      this.isLoading = true
      try {
        this.articulos = await getArticulos()
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async addArticulo(data) {
      this.isLoading = true
      try {
        const nuevo = await createArticulo(data)
        this.articulos.push(nuevo)
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async editArticulo(id, data) {
      this.isLoading = true
      try {
        const actualizado = await updateArticulo(id, data)
        const index = this.articulos.findIndex(a => a.id === id)
        if (index !== -1) this.articulos[index] = actualizado
      } catch (err) {
        this.error = err.message
      } finally {
        this.isLoading = false
      }
    },
    async removeArticulo(id) {
      this.isLoading = true
      try {
        await deleteArticulo(id)
        this.articulos = this.articulos.filter(a => a.id !== id)
      } catch (err) {
        this.error = err.message
        throw err
      } finally {
        this.isLoading = false
      }
    }
  }
})
