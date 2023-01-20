<template>
    <n-space  align="center" Horizontal  justify="center" item-style="display: flex;">
        <n-badge :value="Score" show-zero :color="Props.BadgeColor"/>
        <n-button-group >
            <n-button @click="OnClick(1)"  >
                +
            </n-button>
            <n-button @click="OnClick(-1)" >
                -
            </n-button>
        </n-button-group>
    </n-space>
</template>
<script setup>

import {defineEmits,computed} from 'vue'

const Props = defineProps({
    BadgeColor: null,Score:null
})


const emit= defineEmits(['update:Score','changed'])
const Score = computed(({
    get() {
      return Props.Score;
    },
    set(val) {
        emit('update:Score',val)
    }
  }))

const OnClick = (index) =>{
    
    var v = Score.value + index;

    if(v < 0){
        v = 0;
    }

    Score.value = v;
    emit('changed',index)
}



</script>