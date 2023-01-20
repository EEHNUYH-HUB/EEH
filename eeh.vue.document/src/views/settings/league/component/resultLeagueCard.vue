<template>
    <n-card title="" hoverable :id="'rlc' + Props.Item.leagueID"  @click="IsShowDtl= true">

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
                        <n-space >
                            
                            <n-tag round :bordered="false" v-for="item, index in Props.Item.bestPlayer" :key="index" >
                                {{ item.name }}
                            </n-tag>
   
                        </n-space>
                    </div>
                </n-step>
                <n-step title="Team Ranking">
                    <div class="n-step-description">

                        <template v-for="team, teamIndex in Props.Item.teams" :key="teamIndex" >
                        <n-space vertical style="margin:8px 0px">
                            <n-tag :type="team.teamType">
                                {{ teamIndex+ 1}}등 {{ team.teamName }} {{ team.winCnt }} 승 {{ team.tieCnt }} 무 {{
                                    team.lossCnt
                                }} 패

                                


                            </n-tag>
                            
                        </n-space>
                    </template>
                    </div>
                </n-step>

            </n-steps>
        </n-space>
    </n-card>

    <resultDetailCard v-model:Show="IsShowDtl" :Item="Props.Item" @ondeleted="Emits('ondeleted')"></resultDetailCard>
</template>
<script setup>
import { useStore } from "vuex";
import { ConvertYYYYMMDDToStringDate } from '@/zenc/js/Common'
import { onMounted, ref, defineEmits } from 'vue'
import resultDetailCard from '@/views/settings/league/component/resultDetailCard.vue'

const store = useStore();

const IsShowDtl = ref(false);

const Props = defineProps({ Item: { type: Object } })
const Emits = defineEmits(['ondeleted'])

</script>