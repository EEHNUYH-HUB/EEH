<template>
    <joinLine v-if="props.Item.JoinType == 'line'" :Item="props.Item"  ></joinLine>
    <joinSql v-else-if="props.Item.JoinType == 'sql'" :Item="props.Item" :Picker = "props.Picker" ></joinSql>
</template>
<script setup>
import joinLine from "@/zenc/svg/component/join/joinLine.vue"
import joinSql from "@/zenc/svg/component/join/joinSql.vue"
import {ref,defineProps} from 'vue'
const props = defineProps({Item:{type:Object},Picker:{type:Object} })
</script>
<style scoped>
div {
    -ms-user-select: none;
    -moz-user-select: -moz-none;
    -khtml-user-select: none;
    -webkit-user-select: none;
    user-select: none;
}
svg text{
  text-anchor: middle;
  -ms-user-select: none;
    -moz-user-select: -moz-none;
    -khtml-user-select: none;
    -webkit-user-select: none;
    user-select: none;
}
</style>