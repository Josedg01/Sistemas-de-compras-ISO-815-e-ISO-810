<script setup>
import { onMounted, ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useProveedoresStore } from '../stores/proveedoresStore'
import { Plus, Edit2, Trash2, X } from '@lucide/vue'

const store = useProveedoresStore()
const { proveedores, isLoading } = storeToRefs(store)

onMounted(() => {
  store.fetchProveedores()
})

const showModal = ref(false)
const isEditing = ref(false)
const form = ref({ id: null, cedulaRnc: '', nombreComercial: '', estado: 'Activo' })

const openModal = (item = null) => {
  if (item) {
    isEditing.value = true
    form.value = { ...item }
  } else {
    isEditing.value = false
    form.value = { id: null, cedulaRnc: '', nombreComercial: '', estado: 'Activo' }
  }
  showModal.value = true
}

const closeModal = () => showModal.value = false

const save = async () => {
  if (isEditing.value) {
    await store.editProveedor(form.value.id, form.value)
  } else {
    await store.addProveedor({ cedulaRnc: form.value.cedulaRnc, nombreComercial: form.value.nombreComercial, estado: form.value.estado })
  }
  closeModal()
}

const remove = async (id) => {
  if (confirm('¿Está seguro de eliminar este proveedor?')) {
    await store.removeProveedor(id)
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
          <tr v-for="item in proveedores" :key="item.id" class="border-b border-gray-50 hover:bg-gray-50 transition-colors">
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
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-1">Cédula / RNC</label>
            <input v-model="form.cedulaRnc" type="text" required class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 outline-none">
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
