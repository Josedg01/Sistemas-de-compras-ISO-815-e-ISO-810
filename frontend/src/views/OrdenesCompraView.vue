<script setup>
import { onMounted, ref, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useOrdenesCompraStore } from '../stores/ordenesCompraStore'
import { useArticulosStore } from '../stores/articulosStore'
import { useUnidadesMedidaStore } from '../stores/unidadesMedidaStore'
import { Plus, Send, X, CheckCircle } from '@lucide/vue'

const store = useOrdenesCompraStore()
const { ordenes, isLoading } = storeToRefs(store)

const articulosStore = useArticulosStore()
const { articulos } = storeToRefs(articulosStore)

const unidadesStore = useUnidadesMedidaStore()
const { unidades } = storeToRefs(unidadesStore)

onMounted(async () => {
  store.fetchOrdenes()
  if (articulos.value.length === 0) articulosStore.fetchArticulos()
  if (unidades.value.length === 0) unidadesStore.fetchUnidades()
})

const showModal = ref(false)
const asientoSuccess = ref(null)

const form = ref({
  numeroOrden: `OC-${new Date().getFullYear()}-${Math.floor(Math.random() * 10000)}`,
  fechaOrden: new Date().toISOString().split('T')[0],
  estado: 'Generada',
  articuloId: '',
  cantidad: 1,
  unidadMedidaId: '',
  costoUnitario: 0
})

// Auto-seleccionar unidad y costo al elegir artículo
const onArticuloChange = () => {
  const art = articulos.value.find(a => a.id === form.value.articuloId)
  if (art) {
    form.value.unidadMedidaId = art.unidadMedidaId
  }
}

const openModal = () => {
  form.value = {
    numeroOrden: `OC-${new Date().getFullYear()}-${Math.floor(Math.random() * 10000)}`,
    fechaOrden: new Date().toISOString().split('T')[0],
    estado: 'Generada',
    articuloId: '',
    cantidad: 1,
    unidadMedidaId: '',
    costoUnitario: 0
  }
  showModal.value = true
}

const closeModal = () => showModal.value = false

const save = async () => {
  await store.addOrden({ ...form.value })
  closeModal()
}

const sendAsiento = async (orden) => {
  if (confirm(`¿Enviar asiento contable para ${orden.numeroOrden}?`)) {
    try {
      const asiento = await store.procesarAsientoContable(orden)
      asientoSuccess.value = asiento
      setTimeout(() => asientoSuccess.value = null, 5000)
    } catch (e) {
      alert("Error al enviar asiento contable.")
    }
  }
}

const getArticuloNombre = (id) => {
  const a = articulos.value.find(x => x.id === id)
  return a ? a.descripcion : 'N/A'
}

const getUnidadNombre = (id) => {
  const u = unidades.value.find(x => x.id === id)
  return u ? u.descripcion : 'N/A'
}

const formatCurrency = (val) => {
  return new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(val)
}
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-800">Órdenes de Compra</h1>
      <button @click="openModal()" class="flex items-center gap-2 bg-emerald-600 hover:bg-emerald-700 text-white px-4 py-2 rounded-lg font-medium transition-colors shadow-sm">
        <Plus class="w-5 h-5" /> Nueva Orden
      </button>
    </div>

    <!-- Alert -->
    <div v-if="asientoSuccess" class="mb-6 p-4 bg-emerald-50 border border-emerald-200 rounded-lg flex items-start gap-3">
      <CheckCircle class="w-5 h-5 text-emerald-600 mt-0.5" />
      <div>
        <h4 class="text-emerald-800 font-semibold">Asiento Contable Enviado Exitosamente</h4>
        <p class="text-emerald-600 text-sm mt-1">Identificador: {{ asientoSuccess.identificadorAsiento }} | Monto: {{ formatCurrency(asientoSuccess.montoAsiento) }}</p>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="isLoading" class="text-center py-10">
      <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-emerald-600 mx-auto"></div>
    </div>

    <!-- Table -->
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
            <th class="py-3 px-6 font-semibold text-right">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in ordenes" :key="item.id" class="border-b border-gray-50 hover:bg-gray-50 transition-colors">
            <td class="py-3 px-6 font-medium text-gray-800">{{ item.numeroOrden }}</td>
            <td class="py-3 px-6 text-gray-600">{{ item.fechaOrden }}</td>
            <td class="py-3 px-6 text-gray-600">{{ getArticuloNombre(item.articuloId) }}</td>
            <td class="py-3 px-6 text-right text-gray-600">{{ item.cantidad }} {{ getUnidadNombre(item.unidadMedidaId) }}</td>
            <td class="py-3 px-6 text-right font-medium text-gray-800">{{ formatCurrency(item.cantidad * item.costoUnitario) }}</td>
            <td class="py-3 px-6 text-center">
              <span :class="[
                'px-3 py-1 rounded-full text-xs font-medium',
                item.estado === 'Contabilizada' ? 'bg-blue-100 text-blue-700' : 'bg-amber-100 text-amber-700'
              ]">
                {{ item.estado }}
              </span>
            </td>
            <td class="py-3 px-6 text-right">
              <button v-if="item.estado !== 'Contabilizada'" @click="sendAsiento(item)" class="text-emerald-600 hover:text-emerald-800 mr-3 p-1 rounded-md hover:bg-emerald-50 transition-colors inline-block" title="Enviar Asiento Contable">
                <Send class="w-4 h-4" />
              </button>
            </td>
          </tr>
          <tr v-if="ordenes.length === 0"><td colspan="7" class="py-8 text-center text-gray-500">No hay órdenes de compra registradas.</td></tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div v-if="showModal" class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 transition-opacity">
      <div class="bg-white rounded-2xl shadow-xl w-full max-w-xl overflow-hidden">
        <div class="px-6 py-4 border-b border-gray-100 flex justify-between items-center">
          <h3 class="text-lg font-bold text-gray-800">Nueva Orden de Compra</h3>
          <button @click="closeModal" class="text-gray-400 hover:text-gray-600"><X class="w-5 h-5" /></button>
        </div>
        <form @submit.prevent="save" class="p-6">
          <div class="grid grid-cols-2 gap-4 mb-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">No. Orden (Auto)</label>
              <input v-model="form.numeroOrden" type="text" readonly class="w-full px-4 py-2 border border-gray-200 bg-gray-50 rounded-lg outline-none text-gray-500">
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Fecha</label>
              <input v-model="form.fechaOrden" type="date" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
            </div>
            <div class="col-span-2">
              <label class="block text-sm font-medium text-gray-700 mb-1">Artículo</label>
              <select v-model="form.articuloId" @change="onArticuloChange" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
                <option value="" disabled>Seleccione un artículo...</option>
                <option v-for="a in articulos" :key="a.id" :value="a.id">{{ a.descripcion }} (Ex: {{ a.existencia }})</option>
              </select>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Cantidad</label>
              <input v-model="form.cantidad" type="number" min="1" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Costo Unitario</label>
              <input v-model="form.costoUnitario" type="number" min="0" step="0.01" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
            </div>
          </div>
          
          <div class="bg-gray-50 p-4 rounded-lg flex justify-between items-center mb-6">
            <span class="text-gray-600">Total de la Orden:</span>
            <span class="text-xl font-bold text-gray-800">{{ formatCurrency(form.cantidad * form.costoUnitario) }}</span>
          </div>

          <div class="flex justify-end gap-3">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-600 hover:bg-gray-100 rounded-lg font-medium">Cancelar</button>
            <button type="submit" class="px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white rounded-lg font-medium">Generar Orden</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
