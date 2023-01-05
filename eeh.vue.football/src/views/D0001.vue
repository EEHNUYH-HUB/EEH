<template>
<div :class="store.state.useIsMobile?'div_root_mobile':'div_root_default'" >
    <div :class="store.state.useIsMobile?'div_mobile':'div_default'" class=" document-scroll-container">    
        <Breadcrumb></Breadcrumb>
        
      </div>

</div>
</template>
  
<script setup>
import { ref } from 'vue';
import { useStore } from "vuex";
import  Breadcrumb  from "@/zenc/layout/components/Breadcrumb.vue"
const store = useStore()

</script>

<style scoped>

.n-button {
    
    float: right;
}
.n-grid {
    margin-top: 24px;    
}

.div_root_default {
    display: flex; flex-wrap: nowrap; padding: 32px 24px 56px 56px;
}
.div_root_mobile {
    display: flex; flex-wrap: nowrap; padding: 24px;
}
.div_default {
    width: calc(100% - 228px); margin-right: 36px;
}
.div_mobile {
    width: calc(100%); 
}
</style>