<script setup>
import { onMounted, ref, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useProveedoresStore } from '../stores/proveedoresStore'
import { Plus, Edit2, Trash2, X, Search } from '@lucide/vue'

const store = useProveedoresStore()
const { proveedores, isLoading } = storeToRefs(store)

onMounted(() => {
  store.fetchProveedores()
})

const searchQuery = ref('')
const filteredProveedores = computed(() => {
  if (!searchQuery.value) return proveedores.value
  const q = searchQuery.value.toLowerCase()
  return proveedores.value.filter(p => 
    p.nombreComercial.toLowerCase().includes(q) ||
    p.cedulaRnc.toLowerCase().includes(q) ||
    p.estado.toLowerCase().includes(q) ||
    p.id.toString().includes(q)
  )
})

const showModal = ref(false)
const isEditing = ref(false)
const form = ref({ id: null, tipoDocumento: 'RNC', cedulaRnc: '', nombreComercial: '', estado: 'Activo' })
const formError = ref('')

const openModal = (item = null) => {
  formError.value = ''
  if (item) {
    isEditing.value = true
    const tipoDoc = (item.cedulaRnc && item.cedulaRnc.replace(/-/g, '').length === 11) ? 'Cedula' : 'RNC'
    form.value = { ...item, tipoDocumento: tipoDoc }
  } else {
    isEditing.value = false
    form.value = { id: null, tipoDocumento: 'RNC', cedulaRnc: '', nombreComercial: '', estado: 'Activo' }
  }
  showModal.value = true
}

const closeModal = () => showModal.value = false

const save = async () => {
  formError.value = ''

  try {
    if (isEditing.value) {
      await store.editProveedor(form.value.id, form.value)
    } else {
      await store.addProveedor({
        tipoDocumento: form.value.tipoDocumento,
        cedulaRnc: form.value.cedulaRnc,
        nombreComercial: form.value.nombreComercial,
        estado: form.value.estado
      })
    }
    closeModal()
  } catch (error) {
    formError.value = error.response?.data?.message
      || "Error al guardar el proveedor. Verifique los datos o revise la consola."
  }
}

const remove = async (id) => {
  if (confirm('¿Está seguro de eliminar este proveedor?')) {
    try {
      await store.removeProveedor(id)
    } catch (error) {
      alert(error.response?.data?.message || "Error al eliminar el proveedor.")
    }
  }
}
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-800">Proveedores</h1>
      <button @click="openModal()" class="flex items-center gap-2 bg-emerald-600 hover:bg-emerald-700 text-white px-4 py-2 rounded-lg font-medium transition-colors shadow-sm">
        <Plus class="w-5 h-5" /> Nuevo Proveedor
      </button>
    </div>

    <!-- Loading -->
    <div v-if="isLoading" class="text-center py-10">
      <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-emerald-600 mx-auto"></div>
    </div>

    <!-- Table -->
    <div v-else class="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
      <div class="p-4 border-b border-gray-100 flex items-center gap-3">
        <div class="relative w-full max-w-md">
          <Search class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input type="text" v-model="searchQuery" placeholder="Buscar proveedores..." class="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
        </div>
      </div>
      <table class="w-full text-left border-collapse">
        <thead>
          <tr class="bg-gray-50 text-gray-600 text-sm border-b border-gray-100">
            <th class="py-3 px-6 font-semibold">ID</th>
            <th class="py-3 px-6 font-semibold">Cédula / RNC</th>
            <th class="py-3 px-6 font-semibold">Nombre Comercial</th>
            <th class="py-3 px-6 font-semibold">Estado</th>
            <th class="py-3 px-6 font-semibold text-right">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in filteredProveedores" :key="item.id" class="border-b border-gray-50 hover:bg-gray-50 transition-colors">
            <td class="py-3 px-6 text-gray-600">{{ item.id }}</td>
            <td class="py-3 px-6 font-medium text-gray-600">{{ item.cedulaRnc }}</td>
            <td class="py-3 px-6 font-medium text-gray-800">{{ item.nombreComercial }}</td>
            <td class="py-3 px-6">
              <span :class="['px-3 py-1 rounded-full text-xs font-medium', item.estado === 'Activo' ? 'bg-emerald-100 text-emerald-700' : 'bg-red-100 text-red-700']">
                {{ item.estado }}
              </span>
            </td>
            <td class="py-3 px-6 text-right">
              <button @click="openModal(item)" class="text-blue-600 hover:text-blue-800 mr-3 p-1 rounded-md hover:bg-blue-50 transition-colors inline-block"><Edit2 class="w-4 h-4" /></button>
              <button @click="remove(item.id)" class="text-red-600 hover:text-red-800 p-1 rounded-md hover:bg-red-50 transition-colors inline-block"><Trash2 class="w-4 h-4" /></button>
            </td>
          </tr>
          <tr v-if="proveedores.length === 0"><td colspan="5" class="py-8 text-center text-gray-500">No hay proveedores registrados.</td></tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div v-if="showModal" class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 transition-opacity">
      <div class="bg-white rounded-2xl shadow-xl w-full max-w-md overflow-hidden">
        <div class="px-6 py-4 border-b border-gray-100 flex justify-between items-center">
          <h3 class="text-lg font-bold text-gray-800">{{ isEditing ? 'Editar Proveedor' : 'Nuevo Proveedor' }}</h3>
          <button @click="closeModal" class="text-gray-400 hover:text-gray-600"><X class="w-5 h-5" /></button>
        </div>
        <form @submit.prevent="save" class="p-6">
          <div v-if="formError" class="mb-4 px-4 py-3 rounded-lg bg-red-50 border border-red-200 text-red-700 text-sm">
            {{ formError }}
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Tipo de Documento</label>
            <div class="flex gap-4">
              <label class="flex items-center gap-2 cursor-pointer">
                <input type="radio" v-model="form.tipoDocumento" value="RNC" class="text-emerald-600 focus:ring-emerald-500">
                <span class="text-gray-700 text-sm">RNC (9 dígitos)</span>
              </label>
              <label class="flex items-center gap-2 cursor-pointer">
                <input type="radio" v-model="form.tipoDocumento" value="Cedula" class="text-emerald-600 focus:ring-emerald-500">
                <span class="text-gray-700 text-sm">Cédula (11 dígitos)</span>
              </label>
            </div>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-1">Cédula / RNC</label>
            <input v-model="form.cedulaRnc" type="text" :maxlength="form.tipoDocumento === 'RNC' ? 9 : 11" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-1">Nombre Comercial</label>
            <input v-model="form.nombreComercial" type="text" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
          </div>
          <div class="mb-6">
            <label class="block text-sm font-medium text-gray-700 mb-1">Estado</label>
            <select v-model="form.estado" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
              <option value="Activo">Activo</option>
              <option value="Inactivo">Inactivo</option>
            </select>
          </div>
          <div class="flex justify-end gap-3">
            <button type="button" @click="closeModal" class="px-4 py-2 text-gray-600 hover:bg-gray-100 rounded-lg font-medium">Cancelar</button>
            <button type="submit" class="px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white rounded-lg font-medium">Guardar</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
