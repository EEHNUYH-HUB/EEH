<template>
    <svg :width="props.Width" :height="props.Height" fill="currentColor" viewBox="0 0 16 16">
        <path
            d="M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z" />
    </svg>
</template>
<script setup>
import {ref} from 'vue'
const props = defineProps({ IconType:{type:String},Width:{type:Number},Height:{type:Number}})
</script>