<template>

    <n-alert v-if="Team" :show-icon="false" :type="Team.teamType" :title="Team.teamName" closable @close="OnClose">


        <n-data-table :columns="columns" :data="Team.players" @click="OnClick" :bordered="true"
            :single-line="false" single-column size="small" />
    </n-alert>

    <n-alert v-else :show-icon="false">
        <n-space Horizontal justify="center">
            <n-dropdown placement="bottom-start" trigger="click" size="small" :options="Props.Teams"
                @select="TeamSelect">
                <n-icon size="20">
                    <svg viewBox="0 0 512 512">
                        <path
                            d="M368.5 240H272v-96.5c0-8.8-7.2-16-16-16s-16 7.2-16 16V240h-96.5c-8.8 0-16 7.2-16 16 0 4.4 1.8 8.4 4.7 11.3 2.9 2.9 6.9 4.7 11.3 4.7H240v96.5c0 4.4 1.8 8.4 4.7 11.3 2.9 2.9 6.9 4.7 11.3 4.7 8.8 0 16-7.2 16-16V272h96.5c8.8 0 16-7.2 16-16s-7.2-16-16-16z" />
                    </svg>

                </n-icon>
            </n-dropdown></n-space>
    </n-alert>
</template>

<script setup>

import { defineEmits, computed,ref } from 'vue'
const Props = defineProps({
    Game: { type: Object }
    , Teams: { type: Array }
    , IsLeft: null
    , Score: { type: Number }
    ,ShowObj :{type:Boolean}
})
const emit = defineEmits(["update:ShowObj"])
const ShowObj = computed(({
    get() {
      return Props.ShowObj;
    },
    set(val) {
        emit('update:ShowObj',val)
    }
  }))

const Team = ref(null);

const columns = ref(new Array);
columns.value.push({ title: '이름', key: 'name' })
columns.value.push({ title: '골', key: 'goal', width: 50, align: 'center' })
columns.value.push({ title: '도움', key: 'assist', width: 50, align: 'center' })
columns.value.push({ title: '수비', key: 'save', width: 50, align: 'center' })
const OnClick = ()=>{
    Props.ShowObj.IsShow = true;
    Props.ShowObj.val = Props.IsLeft?'left':'right';
}
const TeamSelect = (index) => {

    for (var i in Props.Teams) {
        var team = Props.Teams[i];

        if (team.teamId == index) {

            Team.value = team;
            Team.value.score = 0;
            for (var j in Team.value.Players) {
                var player = Team.value.Players[j];
                player.goal = 0;
                player.assist = 0;
                player.save = 0;
            }

            if (Props.IsLeft) {
                Props.Game.leftTeam = Team.value;
            }
            else {
                Props.Game.rightTeam = Team.value;
            }
            break;
        }
    }


}

const OnClose = () => {
    Team.value = null;
    if (Props.IsLeft) {
        Props.Game.leftTeam = null;
    }
    else {
        Props.Game.rightTeam = null;
    }
    return true;

}
</script>