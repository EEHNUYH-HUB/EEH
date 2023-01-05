import { createRouter, createWebHistory } from "vue-router";
import { GetRoutes } from '@/zenc/router/menus'

const routes = GetRoutes()
const router = createRouter({
  history : createWebHistory(), //해시 히스토리 모드(SPA 방식이여서 페이지가 바뀔대 마다 서버에 페이지에 대한 정보를 다시 요청하지 않음)
  routes,
  linkActiveClass: "active",
});

router.beforeEach((to,from,next)=>{
  //console.log("index")
  next()
  //const loggedIn = localStorage.getItem('user');
  // if(to.matched.some(record=>record.meta.requiresAuth) && !loggedIn){ // to.matched는 이동할 route와 match되는 route들
  //   next('/Login') // redirect to login page
  // }else{
  //   next() // direct to 'to'
  // }
})

export default router;
