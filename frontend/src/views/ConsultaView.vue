<script setup>
import { onMounted, ref, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useOrdenesCompraStore } from '../stores/ordenesCompraStore'
import { useArticulosStore } from '../stores/articulosStore'
import { Search, FileDown } from '@lucide/vue'

const ordenesStore = useOrdenesCompraStore()
const { ordenes, isLoading } = storeToRefs(ordenesStore)

const articulosStore = useArticulosStore()
const { articulos } = storeToRefs(articulosStore)

const filtros = ref({
  fechaDesde: '',
  fechaHasta: '',
  articuloId: '',
  estado: ''
})

onMounted(async () => {
  if (ordenes.value.length === 0) await ordenesStore.fetchOrdenes()
  if (articulos.value.length === 0) await articulosStore.fetchArticulos()
})

const ordenesFiltradas = computed(() => {
  return ordenes.value.filter(o => {
    let match = true
    if (filtros.value.fechaDesde && o.fechaOrden < filtros.value.fechaDesde) match = false
    if (filtros.value.fechaHasta && o.fechaOrden > filtros.value.fechaHasta) match = false
    if (filtros.value.articuloId && o.articuloId !== filtros.value.articuloId) match = false
    if (filtros.value.estado && o.estado !== filtros.value.estado) match = false
    return match
  })
})

const formatCurrency = (val) => new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(val)
const getArticuloNombre = (id) => {
  const a = articulos.value.find(x => x.id === id)
  return a ? a.descripcion : 'N/A'
}

const exportCSV = () => {
  const headers = ['No. Orden', 'Fecha', 'Articulo', 'Cantidad', 'Costo Unit', 'Total', 'Estado']
  const rows = ordenesFiltradas.value.map(o => [
    o.numeroOrden,
    o.fechaOrden,
    getArticuloNombre(o.articuloId),
    o.cantidad,
    o.costoUnitario,
    (o.cantidad * o.costoUnitario).toFixed(2),
    o.estado
  ])
  const csvContent = "data:text/csv;charset=utf-8," + [headers.join(','), ...rows.map(e => e.join(','))].join("\n")
  const encodedUri = encodeURI(csvContent)
  const link = document.createElement("a")
  link.setAttribute("href", encodedUri)
  link.setAttribute("download", "consulta_ordenes.csv")
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
}
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-800">Consulta de Órdenes</h1>
      <button @click="exportCSV" class="flex items-center gap-2 bg-slate-800 hover:bg-slate-900 text-white px-4 py-2 rounded-lg font-medium transition-colors shadow-sm">
        <FileDown class="w-5 h-5" /> Exportar a CSV
      </button>
    </div>

    <!-- Filtros -->
    <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-100 mb-6 flex flex-wrap gap-4 items-end">
      <div class="flex-1 min-w-[200px]">
        <label class="block text-sm font-medium text-gray-700 mb-1">Fecha Desde</label>
        <input v-model="filtros.fechaDesde" type="date" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
      </div>
      <div class="flex-1 min-w-[200px]">
        <label class="block text-sm font-medium text-gray-700 mb-1">Fecha Hasta</label>
        <input v-model="filtros.fechaHasta" type="date" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
      </div>
      <div class="flex-1 min-w-[200px]">
        <label class="block text-sm font-medium text-gray-700 mb-1">Artículo</label>
        <select v-model="filtros.articuloId" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
          <option value="">Todos</option>
          <option v-for="a in articulos" :key="a.id" :value="a.id">{{ a.descripcion }}</option>
        </select>
      </div>
      <div class="flex-1 min-w-[200px]">
        <label class="block text-sm font-medium text-gray-700 mb-1">Estado</label>
        <select v-model="filtros.estado" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
          <option value="">Todos</option>
          <option value="Generada">Generada</option>
          <option value="Aprobada">Aprobada</option>
          <option value="Contabilizada">Contabilizada</option>
        </select>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="isLoading" class="text-center py-10">
      <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-emerald-600 mx-auto"></div>
    </div>

    <!-- Resultados -->
    <div v-else class="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
      <table class="w-full text-left border-collapse">
        <thead>
          <tr class="bg-gray-50 text-gray-600 text-sm border-b border-gray-100">
            <th class="py-3 px-6 font-semibold">No. Orden</th>
            <th class="py-3 px-6 font-semibold">Fecha</th>
            <th class="py-3 px-6 font-semibold">Artículo</th>
            <th class="py-3 px-6 font-semibold text-right">Cant.</th>
            <th class="py-3 px-6 font-semibold text-right">Total</th>
            <th class="py-3 px-6 font-semibold text-center">Estado</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in ordenesFiltradas" :key="item.id" class="border-b border-gray-50 hover:bg-gray-50 transition-colors">
            <td class="py-3 px-6 font-medium text-gray-800">{{ item.numeroOrden }}</td>
            <td class="py-3 px-6 text-gray-600">{{ item.fechaOrden }}</td>
            <td class="py-3 px-6 text-gray-600">{{ getArticuloNombre(item.articuloId) }}</td>
            <td class="py-3 px-6 text-right text-gray-600">{{ item.cantidad }}</td>
            <td class="py-3 px-6 text-right font-medium text-gray-800">{{ formatCurrency(item.cantidad * item.costoUnitario) }}</td>
            <td class="py-3 px-6 text-center">
              <span :class="[
                'px-3 py-1 rounded-full text-xs font-medium',
                item.estado === 'Contabilizada' ? 'bg-blue-100 text-blue-700' : (item.estado === 'Aprobada' ? 'bg-emerald-100 text-emerald-700' : 'bg-amber-100 text-amber-700')
              ]">
                {{ item.estado }}
              </span>
            </td>
          </tr>
          <tr v-if="ordenesFiltradas.length === 0"><td colspan="6" class="py-8 text-center text-gray-500">No hay resultados para esta búsqueda.</td></tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
