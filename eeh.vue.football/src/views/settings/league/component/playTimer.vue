<template>
  <n-space align="center">
    <span style="font-variant-numeric: tabular-nums;font-size:30px">
      <n-countdown ref="countdown" :duration="props.timeMin * 60000" :active="active"  @finish="Onfinish"/>
      <div style="display:none">
      <n-countdown ref="countdown2" :duration="(props.timeMin-3) * 60000" :active="active"  @finish="Onfinish" />
    </div>
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
import {  ref,watch,onMounted } from 'vue'

const props = defineProps({timeMin:{type:Number}})
const active = ref(false)
const countdown= ref();
const countdown2= ref();


const handlePlay  = () =>{
    active.value = !active.value;
}
const Onfinish =()=>{
 var audio = new Audio("1.mp3");
 audio.play();
}



const handleReset = () =>{countdown.value.reset();countdown2.value.reset();}
</script>