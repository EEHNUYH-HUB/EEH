<template>
    <tableItemSvg :Item="props.Item"></tableItemSvg>
</template>
<script setup>
import tableItemSvg from '@/zenc/svg/component/item/tableItemSvg.vue'
import {ref,defineProps} from 'vue'

const props = defineProps({ Item:{type:Object}})
</script>
<style scoped>
div {
    -ms-user-select: none;
    -moz-user-select: -moz-none;
    -khtml-user-select: none;
    -webkit-user-select: none;
    user-select: none;
}

</style>