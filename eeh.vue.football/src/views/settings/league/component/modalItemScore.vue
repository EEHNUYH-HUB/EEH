<template>

    <n-grid cols="4">
        <n-grid-item>
            <n-space Horizontal justify="center" item-style="display: flex;">
                이름
            </n-space>
        </n-grid-item>
        <n-grid-item>
            <n-space Horizontal justify="center" item-style="display: flex;">
                골
            </n-space>

        </n-grid-item>
        <n-grid-item>
            <n-space Horizontal justify="center" item-style="display: flex;">
                도움
            </n-space>

        </n-grid-item>
        <n-grid-item>
            <n-space Horizontal justify="center" item-style="display: flex;">
                수비
            </n-space>

        </n-grid-item>
    </n-grid>
    <n-grid cols="4" v-for="item, index in Props.Team.players" :key="index">
        <n-grid-item>
            <n-space Horizontal justify="center" item-style="display: flex;">
                {{ item.name }}
            </n-space>

        </n-grid-item>
        <n-grid-item>
            <scoreButton BadgeColor="red" v-model:Score="item.goal" @changed="OnChanged"></scoreButton>
        </n-grid-item>
        <n-grid-item>
            <scoreButton BadgeColor="blue" v-model:Score="item.assist"> </scoreButton>
        </n-grid-item>
        <n-grid-item>
            <scoreButton BadgeColor="gray" v-model:Score="item.save"></scoreButton>
        </n-grid-item>
    </n-grid>
    <n-grid cols="4" responsive="screen">
        <n-grid-item>
            <n-space Horizontal justify="center" item-style="display: flex;">
                기타
            </n-space>

        </n-grid-item>
        <n-grid-item>
            <scoreButton BadgeColor="red" v-model:Score="etcScore" @changed="OnChanged"></scoreButton>
        </n-grid-item>
    </n-grid>

</template>

<script setup>

import scoreButton from '@/views/settings/league/component/scoreButton.vue'


import { ref } from 'vue'
const Props = defineProps({
    Team: { type: Object }
})

const etcScore = ref(0);

const columns = ref(new Array);
columns.value.push({ title: '이름', key: 'name' })
columns.value.push({ title: '골', key: 'goal', width: 50, align: 'center' })
columns.value.push({ title: '도움', key: 'assist', width: 50, align: 'center' })
columns.value.push({ title: '수비', key: 'save', width: 50, align: 'center' })

const OnChanged = (val) => {
    
    Props.Team.score += val;

    if (Props.Team.score < 0)
        Props.Team.score = 0;
}

</script>