<template>
    <g v-if="props.Item && props.Item.IsSelected">
        <rect :width="props.Item.Rect.Width" opacity="0.5" :height="props.Item.Rect.Height" stroke="#96C6DE" fill="none">
        </rect>
        <g v-for="sub, i in  props.Item.Rect.SubObjs" :key="i">
            <circle :cx="sub.cX" :cy="sub.cY" :r="sub.R" :stroke="sub.S" stroke-width="0.5" cur
                @mousedown="sub.IsDown = true" :fill="sub.IsDown ? '#186083' : sub.F">
            </circle>
        </g>
    </g>
</template>

<script setup>
import {ref,defineProps} from 'vue'
const props = defineProps({Item:{type:Object}})
</script>