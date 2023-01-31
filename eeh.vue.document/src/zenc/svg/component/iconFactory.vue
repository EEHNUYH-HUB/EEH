<template>
    <svg v-if="props.Item" :width="props.Item.Rect.Width" :height="Item.Rect.Height"  
    :fill="props.Item.StrokeColor"  viewBox="0 0 16 16">
        <chatSvg v-if="props.Item.IconType=='chat'" ></chatSvg>
        <circleSvg v-else-if="props.Item.IconType=='circle'"  ></circleSvg>
        <databaseSvg v-else-if="props.Item.IconType=='database'"  ></databaseSvg>
        <diagramSvg v-else-if="props.Item.IconType=='diagram'"  ></diagramSvg>
        <diamondSvg v-else-if="props.Item.IconType=='diamond'"  ></diamondSvg>
        <emailSvg v-else-if="props.Item.IconType=='email'"  ></emailSvg>
        <fontsSvg v-else-if="props.Item.IconType=='fonts'"  ></fontsSvg>
        <listTaskSvg v-else-if="props.Item.IconType=='listTask'"  ></listTaskSvg>
        <pcSvg v-else-if="props.Item.IconType=='pc'"  ></pcSvg>
        <serverSvg v-else-if="props.Item.IconType=='server'"  ></serverSvg>
        <squareSvg v-else-if="props.Item.IconType=='square'"  ></squareSvg>
        <triangleSvg v-else-if="props.Item.IconType=='triangle'"  ></triangleSvg>
        <tableSvg v-else-if="props.Item.IconType=='table'"  ></tableSvg>
    </svg>
    <text   :fill="props.Item.StrokeColor"  :x="props.Item.Rect.Width+30" :y="Item.Rect.Height/2+5" v-if="props.Item.IsShowDisplayName && props.Item.DisplayName">{{props.Item.DisplayName}}</text>
</template>
<script setup>
import chatSvg from '@/zenc/svg/component/icon/chatSvg.vue'
import circleSvg from '@/zenc/svg/component/icon/circleSvg.vue'
import databaseSvg from '@/zenc/svg/component/icon/databaseSvg.vue'
import diagramSvg from '@/zenc/svg/component/icon/diagramSvg.vue'
import diamondSvg from '@/zenc/svg/component/icon/diamondSvg.vue'
import emailSvg from '@/zenc/svg/component/icon/emailSvg.vue'
import fontsSvg from '@/zenc/svg/component/icon/fontsSvg.vue'
import listTaskSvg from '@/zenc/svg/component/icon/listTaskSvg.vue'
import pcSvg from '@/zenc/svg/component/icon/pcSvg.vue'
import serverSvg from '@/zenc/svg/component/icon/serverSvg.vue'
import squareSvg from '@/zenc/svg/component/icon/squareSvg.vue'
import triangleSvg from '@/zenc/svg/component/icon/triangleSvg.vue'
import tableSvg from '@/zenc/svg/component/icon/tableSvg.vue'
import {ref} from 'vue'

const props = defineProps({ Item:{type:Object}})
</script>
<style scoped>
svg text {
    -webkit-user-select: none;
       -moz-user-select: none;
        -ms-user-select: none;
            user-select: none;
}
svg text::selection {
    background: none;
}
</style>