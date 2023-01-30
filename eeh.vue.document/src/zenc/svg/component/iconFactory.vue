<template>
    <chatSvg v-if="props.IconType=='chat'" :Widht="props.Width" :Height="props.Height" ></chatSvg>
    <circleSvg v-else-if="props.IconType=='circle'" :Widht="props.Width" :Height="props.Height" ></circleSvg>
    <databaseSvg v-else-if="props.IconType=='database'" :Widht="props.Width" :Height="props.Height" ></databaseSvg>
    <diagramSvg v-else-if="props.IconType=='diagram'" :Widht="props.Width" :Height="props.Height" ></diagramSvg>
    <diamondSvg v-else-if="props.IconType=='diamond'" :Widht="props.Width" :Height="props.Height" ></diamondSvg>
    <emailSvg v-else-if="props.IconType=='email'" :Widht="props.Width" :Height="props.Height" ></emailSvg>
    <fontsSvg v-else-if="props.IconType=='fonts'" :Widht="props.Width" :Height="props.Height" ></fontsSvg>
    <listTaskSvg v-else-if="props.IconType=='listTask'" :Widht="props.Width" :Height="props.Height" ></listTaskSvg>
    <pcSvg v-else-if="props.IconType=='pc'" :Widht="props.Width" :Height="props.Height" ></pcSvg>
    <serverSvg v-else-if="props.IconType=='server'" :Widht="props.Width" :Height="props.Height" ></serverSvg>
    <squareSvg v-else-if="props.IconType=='square'" :Widht="props.Width" :Height="props.Height" ></squareSvg>
    <triangleSvg v-else-if="props.IconType=='triangle'" :Widht="props.Width" :Height="props.Height" ></triangleSvg>
    <tableSvg v-else-if="props.IconType=='table'" :Widht="props.Width" :Height="props.Height" ></tableSvg>
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

const props = defineProps({ IconType:{type:String},Width:{type:Number},Height:{type:Number}})
</script>