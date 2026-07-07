<script setup>
import { onMounted, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useDepartamentosStore } from '../stores/departamentosStore'
import { useArticulosStore } from '../stores/articulosStore'
import { useOrdenesCompraStore } from '../stores/ordenesCompraStore'
import { useProveedoresStore } from '../stores/proveedoresStore'
import { useEmpleadosStore } from '../stores/empleadosStore'
import { useAsientosContablesStore } from '../stores/asientosContablesStore'
import { Users, Package, ShoppingCart, Factory, Scale, LayoutDashboard } from '@lucide/vue'

const depStore = useDepartamentosStore()
const { departamentos } = storeToRefs(depStore)

const artStore = useArticulosStore()
const { articulos } = storeToRefs(artStore)

const ordStore = useOrdenesCompraStore()
const { ordenes } = storeToRefs(ordStore)

const provStore = useProveedoresStore()
const { proveedores } = storeToRefs(provStore)

const empStore = useEmpleadosStore()
const { empleados } = storeToRefs(empStore)

const asientosStore = useAsientosContablesStore()
const { asientos } = storeToRefs(asientosStore)

onMounted(() => {
  if (departamentos.value.length === 0) depStore.fetchDepartamentos()
  if (articulos.value.length === 0) artStore.fetchArticulos()
  if (ordenes.value.length === 0) ordStore.fetchOrdenes()
  if (proveedores.value.length === 0) provStore.fetchProveedores()
  if (empleados.value.length === 0) empStore.fetchEmpleados()
  if (asientos.value.length === 0) asientosStore.fetchAsientos()
})

const ordenesPendientes = computed(() => {
  return ordenes.value.filter(o => o.estado === 'Pendiente' || o.estado === 'Generada' || o.estado === 'Aprobada').length
})
</script>

<template>
  <div>
    <div class="flex items-center gap-3 mb-6">
      <LayoutDashboard class="w-8 h-8 text-gray-800" />
      <h1 class="text-3xl font-bold text-gray-800">Panel de Control</h1>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      
      <!-- Departamentos -->
      <div class="bg-white rounded-xl shadow-sm p-6 border border-gray-100 hover:shadow-md transition-shadow flex items-start justify-between">
        <div>
          <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wider">Departamentos</h3>
          <p class="text-4xl font-bold text-gray-800 mt-2">{{ departamentos.length }}</p>
        </div>
        <div class="p-3 bg-blue-50 rounded-lg">
          <Users class="w-6 h-6 text-blue-600" />
        </div>
      </div>
      
      <!-- Empleados -->
      <div class="bg-white rounded-xl shadow-sm p-6 border border-gray-100 hover:shadow-md transition-shadow flex items-start justify-between">
        <div>
          <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wider">Empleados</h3>
          <p class="text-4xl font-bold text-gray-800 mt-2">{{ empleados.length }}</p>
        </div>
        <div class="p-3 bg-indigo-50 rounded-lg">
          <Users class="w-6 h-6 text-indigo-600" />
        </div>
      </div>
      
      <!-- Proveedores -->
      <div class="bg-white rounded-xl shadow-sm p-6 border border-gray-100 hover:shadow-md transition-shadow flex items-start justify-between">
        <div>
          <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wider">Proveedores</h3>
          <p class="text-4xl font-bold text-gray-800 mt-2">{{ proveedores.length }}</p>
        </div>
        <div class="p-3 bg-purple-50 rounded-lg">
          <Factory class="w-6 h-6 text-purple-600" />
        </div>
      </div>

      <!-- Artículos -->
      <div class="bg-white rounded-xl shadow-sm p-6 border border-gray-100 hover:shadow-md transition-shadow flex items-start justify-between">
        <div>
          <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wider">Artículos</h3>
          <p class="text-4xl font-bold text-gray-800 mt-2">{{ articulos.length }}</p>
        </div>
        <div class="p-3 bg-emerald-50 rounded-lg">
          <Package class="w-6 h-6 text-emerald-600" />
        </div>
      </div>
      
      <!-- Órdenes Pendientes -->
      <div class="bg-white rounded-xl shadow-sm p-6 border border-gray-100 hover:shadow-md transition-shadow flex items-start justify-between">
        <div>
          <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wider">Órdenes Pendientes</h3>
          <p class="text-4xl font-bold text-amber-500 mt-2">{{ ordenesPendientes }}</p>
        </div>
        <div class="p-3 bg-amber-50 rounded-lg">
          <ShoppingCart class="w-6 h-6 text-amber-600" />
        </div>
      </div>
      
      <!-- Asientos Contables -->
      <div class="bg-white rounded-xl shadow-sm p-6 border border-gray-100 hover:shadow-md transition-shadow flex items-start justify-between">
        <div>
          <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wider">Asientos Contables</h3>
          <p class="text-4xl font-bold text-gray-800 mt-2">{{ asientos.length }}</p>
        </div>
        <div class="p-3 bg-rose-50 rounded-lg">
          <Scale class="w-6 h-6 text-rose-600" />
        </div>
      </div>

    </div>
  </div>
</template>
