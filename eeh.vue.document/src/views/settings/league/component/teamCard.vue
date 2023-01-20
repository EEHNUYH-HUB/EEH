<template>
    <n-alert v-if="Props.Team" :show-icon="false" :type="Props.Team.teamType" :title="Props.Team.teamName">
        <n-space v-if="Props.IsVertical" vertical>
            <template v-for="item, index in Props.Team.players" :key="index">
                <n-p>{{ item.name }}</n-p>
            </template>
        </n-space>
        <n-space v-else >
            <template v-for="item, index in Props.Team.players" :key="index">
                <n-p>{{ item.name }}</n-p>
            </template>
        </n-space>
       
    </n-alert>
</template>
<script setup>
import {onMounted} from 'vue'
const Props = defineProps({
    Team: { type: Object },
    IsVertical:{type:Boolean}
})
onMounted (()=>{})
</script>