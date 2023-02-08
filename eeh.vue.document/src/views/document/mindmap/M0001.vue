<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb :Buttons="Buttons"></Breadcrumb>
            
            <n-grid cols="1 s:1 m:1 l:1 xl:2 2xl:2" :x-gap="12" :y-gap="8" responsive="screen">
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem"  Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem" Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem" Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem" Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem" Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem" Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
                <n-gi >
                    <n-card title="DataBase Query model">
                        <svgPanel @selectedItem="OnSelectedItem" Height="500px" SvgHeight="1000px" ></svgPanel>
                    </n-card>
                </n-gi>
            </n-grid>
        </div>
    </div>
    
    <taskFactory v-model:show="IsShowTaskDtl" :Item = "SelectedItem"></taskFactory>
    
</template>
<script setup>
import Header from "@/zenc/layout/components/Header.vue"
import { useStore } from "vuex";
import {ref} from 'vue'
import svgPanel from "@/zenc/svg/component/svgPanel.vue"
import taskFactory from "@/zenc/svg/component/taskFactory.vue"
import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"



const Buttons = ref(new Array);
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