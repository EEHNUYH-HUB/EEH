<template>
    <n-drawer v-model:show="Show" :default-width="420" placement="right" resizable>
        <n-drawer-content :title="ConvertYYYYMMDDToStringDate(Props.Item.strDate) " closable>

            <n-space vertical>
                <n-steps vertical current="4">
                    <n-step title="League Date And Location">
                        <div class="n-step-description">
                            <p>{{ ConvertYYYYMMDDToStringDate(Props.Item.strDate) }}</p>
                            <p>{{ Props.Item.locationName }}</p>
                        </div>
                    </n-step>
                    <n-step title="Best Player">
                        <div class="n-step-description">
                            <n-space>

                                <n-tag round :bordered="false" v-for="item, index in Props.Item.bestPlayer"
                                    :key="index">
                                    {{ item.name }}
                                </n-tag>

                            </n-space>
                        </div>
                    </n-step>
                    <n-step title="Team Ranking">
                        <div class="n-step-description">

                            <template v-for="team, teamIndex in Props.Item.teams" :key="teamIndex">
                                <n-space vertical style="margin:8px 0px">
                                    <n-tag :type="team.teamType">
                                        {{ teamIndex+ 1}}등 {{ team.teamName }} {{ team.winCnt }} 승 {{ team.tieCnt }} 무
                                        {{
                                            team.lossCnt
                                        }} 패




                                    </n-tag>
                                    <n-data-table :columns="columns" :data="team.players" :bordered="true"  :single-line="false" single-column size="small" />
                                </n-space>
                            </template>
                        </div>
                    </n-step>

                </n-steps>
            </n-space>
            <template #footer>
                <n-button type="info" size="small" @click="OnDelete">삭제</n-button>
            </template>
        </n-drawer-content>
    </n-drawer>
</template>

<script setup>
import { useStore } from "vuex";
import { ConvertYYYYMMDDToStringDate } from '@/zenc/js/Common'
import { computed, ref, defineEmits } from 'vue'
import teamCard from '@/views/settings/league/component/teamCard.vue'
const store = useStore();
const columns = ref(new Array);
columns.value.push({ title: '이름', key: 'name' })
columns.value.push({ title: '골', key: 'goal' ,width: 50,align:'center'})
columns.value.push({ title: '도움', key: 'assist' ,width: 50,align:'center'})
columns.value.push({ title: '수비', key: 'save' ,width: 50,align:'center'})


const Props = defineProps({ Item: { type: Object } ,Show:{type:Boolean}})

const OnDelete = async () => {
    if (confirm("정말 삭제 하시겠습니까?")) {
        await store.state.apiClient.ExecNonQuery('SQL', 'DELETELEAGUE', { id: Props.Item.leagueID });
        Show.value = false;
        emit('ondeleted');
        
    }
}

const emit= defineEmits(['update:Show','ondeleted'])
const Show = computed(({
    get() {
      return Props.Show;
    },
    set(val) {
        emit('update:Show',val)
    }
  }))
</script>