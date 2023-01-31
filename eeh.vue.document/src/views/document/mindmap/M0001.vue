<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            
            <svgPanel @selectedItem="OnSelectedItem" ></svgPanel>
            
        </div>
    </div>
</template>
<script setup>
import { useStore } from "vuex";
import {ref} from 'vue'
import svgPanel from "@/zenc/svg/component/svgPanel.vue"
const store = useStore()

const OnSelectedItem= (item)=>{

    console.log(item);
}
</script>
<style scoped>
.div_default{
    width:calc(100%);
    margin-right:0px
}
.div_root_default{
padding:24px
}
</style>