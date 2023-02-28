<template>
    <n-popover :show="IsShow" :x="X" :y="Y" trigger="manual" :show-arrow="true">
        <n-space vertical>
            <template  v-for="item, index in Icons" :key="index">
            {{item.DisplayName}}
            <n-space >
                <n-button  v-for="item, index in item.Items" :key="index" @click="OnClickIcon(item)">
                    <iconFactory :Item="item"></iconFactory>
                </n-button>
            </n-space>
        </template>
        </n-space>
    </n-popover>
</template>

<script setup>

import { ref, defineProps, defineExpose,defineEmits } from 'vue'
import { GetIcons } from '@/zenc/svg/js/Common'
import iconFactory from "@/zenc/svg/component/iconFactory.vue"

const props = defineProps({ColorObj:{type:Object}})
const IsShow = ref(false)
const Icons = ref(new Array)
const X = ref(0)
const Y = ref(0)

const Show = (isShow, x, y, item) => {
    
    if (isShow) {
        X.value = x;
        Y.value = y;

        var beforeIconType = 'none';
        if(item){
            beforeIconType = item.IconType;
        }
        Icons.value = GetIcons(props.ColorObj,beforeIconType,item);
     
    }
    IsShow.value = isShow;
}
const OnClickIcon = (item) => {
    
    emits("OnSelectedIcon",item)
    Show(false,0,0,null);
}
const emits = defineEmits(["OnSelectedIcon"])
defineExpose({ Show })
</script>
