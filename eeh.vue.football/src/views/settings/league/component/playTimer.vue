<template>
    <n-space>
    <span style="font-variant-numeric: tabular-nums">
      <n-countdown ref="countdown" :duration="600000" :active="active" />
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
const active = ref(false)
const countdown= ref();

const handlePlay  = () =>{

    active.value = !active.value;

}
const handleReset = () =>{countdown.value.reset();}
</script>