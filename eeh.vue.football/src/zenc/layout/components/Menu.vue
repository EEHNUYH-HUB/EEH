<template>
   <n-layout-sider bordered 
                show-trigger="arrow-circle"
                collapsed-trigger-style="top: 240px; right: 25px;"
                trigger-style="top: 240px;"
                collapse-mode="transform" 
                :collapsed="collapsed"
                @collapse="collapsed = true"
        @expand="collapsed = false"
                >
    <n-scrollbar>
      <n-menu v-model:value="menuObj.ActiveKey" 
      :collapsed="collapsed" :collapsed-width="64"
       :collapsed-icon-size="22" :options="menuObj.Menus" />
    </n-scrollbar>  
  </n-layout-sider>
</template>
<script setup>
import {ref,watch} from "vue"
import { GetMenuObjRef } from "@/zenc/router/menus";
import {  useRoute } from "vue-router";
const collapsed = ref(false);
const activeKey = ref(null);
const route = useRoute();

const menuObj = GetMenuObjRef(1,100,route);
watch(()=>route.fullPath, () => {
  menuObj.value = GetMenuObjRef(1,100,route).value;
});
  
</script>