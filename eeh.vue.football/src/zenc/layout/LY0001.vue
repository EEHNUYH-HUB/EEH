<template>
    <Header></Header>
    <n-layout content-style="max-height: calc(100vh - 64px); " 
    :has-sider ="store.state.showSider" >
        <Menu  v-if="store.state.showSider"></Menu>
        <n-layout   :native-scrollbar="false">    
            <n-layout   content-style="min-height: calc(100vh - 127px); display: flex; flex-direction: column; "  :native-scrollbar="false">   
                <router-view  />
            </n-layout>
            <Footer />         
        </n-layout>
    </n-layout>
     
</template>
  
<script setup>
import Header from "@/zenc/layout/components/Header.vue"
import Footer from "@/zenc/layout/components/Footer.vue"
import Menu from "@/zenc/layout/components/Menu.vue"
// import { useLoadingBar,useMessage,useNotification  } from 'naive-ui'
 import { useStore } from "vuex";
 const store = useStore();
// store.state.apiClient.LodingBar = useLoadingBar();
// store.state.apiClient.Message = useMessage();
// store.state.apiClient.Notification = useNotification(); 

// store.state.apiClient.Store = store;
</script>