<template>
      <path :d="props.Item.Path" :stroke="props.Item.StrokeColor"  :fill="props.Item.FillColor" stroke-width="0.5"
            :marker-end="props.Item.EndType"  :marker-start="props.Item.StartType" />

        
</template>
<script setup>
import {ref,defineProps} from 'vue'
const props = defineProps({Item:{type:Object}})

</script>