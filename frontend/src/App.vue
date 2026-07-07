<script setup>
import { RouterView, RouterLink } from 'vue-router'
import { LayoutDashboard, Users, Scale, Factory, Package, ShoppingCart, Search } from '@lucide/vue'
</script>

<template>
  <div class="flex h-screen bg-gray-100 text-gray-900 font-sans">
    <!-- Sidebar -->
    <aside class="w-64 bg-slate-900 text-white flex flex-col shadow-xl">
      <div class="p-6 text-center border-b border-slate-700">
        <h1 class="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-emerald-400 to-cyan-400">
          ComprasApp
        </h1>
      </div>
      <nav class="flex-1 px-4 py-6 space-y-2 overflow-y-auto">
        <RouterLink to="/" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <LayoutDashboard class="w-5 h-5" /> Inicio
        </RouterLink>
        <RouterLink to="/departamentos" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <Users class="w-5 h-5" /> Departamentos
        </RouterLink>
        <RouterLink to="/unidades-medida" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <Scale class="w-5 h-5" /> U. de Medida
        </RouterLink>
        <RouterLink to="/proveedores" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <Factory class="w-5 h-5" /> Proveedores
        </RouterLink>
        <RouterLink to="/articulos" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <Package class="w-5 h-5" /> Artículos
        </RouterLink>
        <RouterLink to="/ordenes-compra" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <ShoppingCart class="w-5 h-5" /> Órdenes Compra
        </RouterLink>
        <RouterLink to="/consulta" class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors hover:bg-slate-800" active-class="bg-emerald-600 text-white hover:bg-emerald-700">
          <Search class="w-5 h-5" /> Consulta
        </RouterLink>
      </nav>
    </aside>

    <!-- Main content -->
    <main class="flex-1 flex flex-col overflow-hidden">
      <!-- Top header -->
      <header class="h-16 bg-white border-b border-gray-200 flex items-center px-8 justify-between shadow-sm">
        <h2 class="text-lg font-semibold text-gray-700">Sistema de Gestión de Compras</h2>
        <div class="flex items-center gap-4">
          <div class="w-8 h-8 rounded-full bg-emerald-100 flex items-center justify-center text-emerald-700 font-bold">
            U
          </div>
        </div>
      </header>
      
      <!-- Content Area -->
      <div class="flex-1 overflow-y-auto p-8 bg-gray-50">
        <div class="max-w-7xl mx-auto">
          <RouterView v-slot="{ Component }">
            <transition name="fade" mode="out-in">
              <component :is="Component" />
            </transition>
          </RouterView>
        </div>
      </div>
    </main>
  </div>
</template>

<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
