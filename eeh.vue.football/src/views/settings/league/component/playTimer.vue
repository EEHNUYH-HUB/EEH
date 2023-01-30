<template>
  <n-space align="center">
    <span style="font-variant-numeric: tabular-nums;font-size:30px">
      <n-countdown ref="countdown" :duration="props.timeMin * 60000" :active="active" />
    </span>
    <n-button size="tiny" @click="handlePlay">
      {{active?'STOP':'PLAY'}} 
    </n-button>
    <n-button size="tiny" @click="handleReset">
      Reset
    </n-button>
  </n-space>
</template>
<script setup>
import {  ref } from 'vue'
import { CountdownInst  } from 'naive-ui'
const props = defineProps({timeMin:{type:Number}})
const active = ref(false)
const countdown= ref();

const handlePlay  = () =>{

    active.value = !active.value;

}
const handleReset = () =>{countdown.value.reset();}
</script>