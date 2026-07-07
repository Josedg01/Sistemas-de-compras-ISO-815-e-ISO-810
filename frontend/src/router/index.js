import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/departamentos',
      name: 'departamentos',
      component: () => import('../views/DepartamentosView.vue'),
    },
    {
      path: '/unidades-medida',
      name: 'unidades-medida',
      component: () => import('../views/UnidadesMedidaView.vue'),
    },
    {
      path: '/proveedores',
      name: 'proveedores',
      component: () => import('../views/ProveedoresView.vue'),
    },
    {
      path: '/articulos',
      name: 'articulos',
      component: () => import('../views/ArticulosView.vue'),
    },
    {
      path: '/ordenes-compra',
      name: 'ordenes-compra',
      component: () => import('../views/OrdenesCompraView.vue'),
    },
    {
      path: '/consulta',
      name: 'consulta',
      component: () => import('../views/ConsultaView.vue'),
    },
    {
      path: '/empleados',
      name: 'empleados',
      component: () => import('../views/EmpleadosView.vue'),
    },
    {
      path: '/asientos-contables',
      name: 'asientos-contables',
      component: () => import('../views/AsientosContablesView.vue'),
    }
  ],
})

export default router
