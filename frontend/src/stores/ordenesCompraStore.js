import { defineStore } from 'pinia'
import { getOrdenesCompra, createOrdenCompra, updateOrdenCompra, enviarAsientoContable } from '../services/ordenesCompraService'

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
      } finally {
        this.isLoading = false
      }
    },
    async procesarAsientoContable(orden) {
      this.isLoading = true
      try {
        // Preparar payload para Contabilidad
        const asientoData = {
          identificadorAsiento: `AS-${Date.now()}`,
          descripcion: `Compra OC ${orden.numeroOrden}`,
          identificadorTipoInventario: 1, // Fijo o configurable
          cuentaContable: '4-1000-01', // Fijo de ejemplo
          tipoMovimiento: 'DB',
          fechaAsiento: new Date().toISOString().split('T')[0],
          montoAsiento: orden.cantidad * orden.costoUnitario,
          estado: 'Procesado'
        }
        
        await enviarAsientoContable(asientoData)
        
        // Actualizar estado de la orden a 'Contabilizada'
        const actualizado = await updateOrdenCompra(orden.id, { ...orden, estado: 'Contabilizada' })
        const index = this.ordenes.findIndex(o => o.id === orden.id)
        if (index !== -1) this.ordenes[index] = actualizado
        
        return asientoData
      } catch (err) {
        this.error = err.message
        throw err
      } finally {
        this.isLoading = false
      }
    }
  }
})
