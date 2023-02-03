<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            
            <svgPanel @selectedItem="OnSelectedItem" ></svgPanel>
            
        </div>
    </div>
    
    <taskFactory v-model:show="IsShowTaskDtl" :Item = "SelectedItem"></taskFactory>
    
</template>
<script setup>
import { useStore } from "vuex";
import {ref} from 'vue'
import svgPanel from "@/zenc/svg/component/svgPanel.vue"
import taskFactory from "@/zenc/svg/component/taskFactory.vue"
const store = useStore()
const SelectedItem = ref(null)
const IsShowTaskDtl = ref(false)

const OnSelectedItem= (item)=>{
    SelectedItem.value = item
    IsShowTaskDtl.value = true
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