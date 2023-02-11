<template>
    <n-modal v-model:show="ShowObj.IsShow" :style="bodyStyle" preset="card">
        <template #header>
            상세설정
        </template>

        <n-tabs :default-value="ShowObj.val" type="line" animated >
            <n-tab-pane v-if="Props.Game.leftTeam" name="left" :tab="Props.Game.leftTeam.teamName">
                <modalItemScore :Team="Props.Game.leftTeam"></modalItemScore>
            </n-tab-pane>
            <n-tab-pane v-if="Props.Game.rightTeam" name="right" :tab="Props.Game.rightTeam.teamName">
                <modalItemScore :Team="Props.Game.rightTeam"></modalItemScore>
            </n-tab-pane>

        </n-tabs>

    </n-modal>
</template>

<script setup>

import modalItemScore from "@/views/settings/league/component/modalItemScore.vue"

import {  ref ,computed,defineEmits} from 'vue'
const Props = defineProps({
    Game: { type: Object }
    ,ShowObj :{type:Object}
   
})

const bodyStyle = ref({ width: "350px" })

const emit = defineEmits(["update:ShowObj"])
const ShowObj = computed(({
    get() {
      return Props.ShowObj;
    },
    set(val) {
        emit('update:ShowObj',val)
    }
  }))

</script>