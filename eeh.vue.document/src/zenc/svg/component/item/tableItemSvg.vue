<template>
    
  <foreignObject width="1610" height="1620">
    <div>{{props.Item.TableName}}</div>
      <div v-for="item,index in props.Item.Columns" :key="index" >{{item}}</div>
  </foreignObject>  

</template>

<script setup>
import {ref,defineProps,onMounted}from 'vue'
const props = defineProps({Item:{type:Object}})
onMounted(()=>{
  
})
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