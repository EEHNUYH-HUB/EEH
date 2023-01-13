<template>
    <n-card title="" hoverable :id="'rlc' + Props.Item.leagueID" closable @close="OnClose">

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
                            <n-tag round :bordered="false" v-for="item, index in Props.Item.bestPlayer" :key="index">
                                {{ item.name }}
                                <!-- <template #avatar>
                                    <n-avatar
                                        src="https://cdnimg103.lizhi.fm/user/2017/02/04/2583325032200238082_160x160.jpg" />
                                </template> -->
                            </n-tag>
                        </n-space>
                    </div>
                </n-step>
                <n-step title="Result">
                    <div class="n-step-description">

                        <n-space vertical>
                            <n-tag v-for="team, teamIndex in Props.Item.teams" :key="teamIndex" :type="team.teamType">
                                {{ teamIndex+ 1}}등 {{ team.teamName }} {{ team.winCnt }} 승 {{ team.tieCnt }} 무 {{
                                    team.lossCnt
                                }} 패
                            </n-tag>
                        </n-space>

                    </div>
                </n-step>

            </n-steps>
        </n-space>
    </n-card>
</template>
<script setup>
import { useStore } from "vuex";
import { ConvertYYYYMMDDToStringDate } from '@/zenc/js/Common'
import { onMounted, ref, defineEmits } from 'vue'
import teamCard from '@/views/settings/league/component/teamCard.vue'
const store = useStore();
const Props = defineProps({ Item: { type: Object } })
const Emits = defineEmits(['ondeleted'])
const OnClose = async () => {
    if (confirm("정말 삭제 하시겠습니까?")) {
        await store.state.apiClient.ExecNonQuery('SQL', 'DELETELEAGUE', { id: Props.Item.leagueID });

        Emits('ondeleted');
    }
}
</script>