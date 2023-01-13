<template>

    <n-alert v-if="Team" :show-icon="false" :type="Team.teamType" :title="Team.teamName" closable @close="OnClose">

        <n-space vertical >
           
            <n-grid cols="2 s:4 m:4 l:4 xl:4 2xl:"  responsive="screen" >
                <n-grid-item>
                    <n-space  align="center" Horizontal justify="center" item-style="display: flex;">
                        Name
                    </n-space>
                </n-grid-item>
                <n-grid-item>
                    <n-space  align="center" Horizontal justify="center" item-style="display: flex;">
                        Goal
                    </n-space>
                        
                </n-grid-item>
                <n-grid-item>
                    <n-space  align="center" Horizontal justify="center" item-style="display: flex;">
                        Assist
                    </n-space>
                        
                </n-grid-item>
                <n-grid-item>
                    <n-space  align="center" Horizontal justify="center" item-style="display: flex;">
                        Save
                    </n-space>
                        
                </n-grid-item>
            </n-grid>
            <n-grid cols="2 s:4 m:4 l:4 xl:4 2xl:" responsive="screen" v-for="item, index in Team.players" :key="index">
                <n-grid-item>
                    <n-space  align="center" Horizontal justify="center" item-style="display: flex;">
                        {{ item.name }}
                    </n-space>
                        
                </n-grid-item>
                <n-grid-item>
                    <scoreButton BadgeColor="red" v-model:Score="item.goal" @changed="OnChanged"></scoreButton>
                </n-grid-item>
                <n-grid-item>
                    <scoreButton BadgeColor="blue" v-model:Score="item.assist" > </scoreButton>
                </n-grid-item>
                <n-grid-item>
                    <scoreButton BadgeColor="gray" v-model:Score="item.save" ></scoreButton>
                </n-grid-item>
            </n-grid>
            

        </n-space>

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
import scoreButton from '@/views/settings/league/component/scoreButton.vue'


import { onMounted, ref } from 'vue'
const Props = defineProps({
    Game: { type: Object }
    , Teams: { type: Array }
    , IsLeft: null
    , Score: { type: Number }
})
const Team = ref(null);
onMounted(() => {


})

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
                player.aave = 0;
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
const OnChanged =(val) =>{
    Team.value.score += val;

    if(Team.value.score < 0)
    Team.value.score = 0;
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