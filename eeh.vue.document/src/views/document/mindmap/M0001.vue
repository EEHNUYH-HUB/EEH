<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb ></Breadcrumb>
            <n-grid cols="1">
                <n-gi>
                <svgPanel></svgPanel>
                </n-gi>
            </n-grid>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>
</template>
<script setup>
import { useStore } from "vuex";
import {ref} from 'vue'
import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"

import svgPanel from "@/zenc/svg/component/svgPanel.vue"
const store = useStore()

const AnchorItems = ref(new Array);
</script>