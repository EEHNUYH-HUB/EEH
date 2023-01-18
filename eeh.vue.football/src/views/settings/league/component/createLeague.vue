<template>
    <n-grid v-if="NewLeague" cols="1" responsive="screen">
        <n-gi>
            <n-card title="" hoverable>

                <n-space vertical>
                    <n-steps vertical :current="NewLeague.status">
                        <n-step title="League date and location">
                            <div class="n-step-description">
                                <n-space vertical v-if="NewLeague.status === 1">
                                    <n-date-picker v-model:value="NewLeague.starttimestamp"
                                        placeholder="리그 시작일을 입력해 주세요" type="date" />
                                    <n-select v-model:value="NewLeague.locationId" :options="Locations" />

                                    <n-button size="small" @click="nextButtonClick">
                                        Next
                                    </n-button>
                                </n-space>
                                <template v-else>
                                    <p>{{ NewLeague.strstartdate }}</p>
                                    <p>{{ NewLeague.locationName }}</p>
                                </template>
                            </div>
                        </n-step>
                        <n-step title="League player">
                            <div class="n-step-description">

                                <n-space vertical v-if="NewLeague.status === 2">
                                    <n-transfer v-model:value="SelectedPlayers" :options="Players" />

                                    <n-space horizontal justify="end" align="center">
                                        <n-input v-model:value="TxtPlayer" type="text" placeholder="카톡 내용을 복사해주세요." ></n-input>
                                        <n-button type="primary" size="small" @click="pastePlayer()">
                                            Text Load
                                        </n-button>
                                        <n-button type="info" size="small" @click="saveLeagueMember(false)">
                                            Save
                                        </n-button>
                                        <n-button size="small" @click="prevButtonClick">
                                            Prev
                                        </n-button>

                                        <n-button size="small" @click="nextButtonClick">
                                            Next
                                        </n-button>
                                    </n-space>
                                </n-space>
                                <n-space v-else-if="NewLeague.allPlayer.length > 0">
                                    <n-tag round :bordered="false" v-for="item, index in NewLeague.allPlayer"
                                        :key="index">
                                        {{ item.name }}
                                        <!-- <template #avatar>
                                            <n-avatar
                                                src="https://cdnimg103.lizhi.fm/user/2017/02/04/2583325032200238082_160x160.jpg" />
                                        </template> -->
                                    </n-tag>
                                </n-space>
                                <p v-else>리그에 참가할 선수를 설정 하세요.</p>
                            </div>
                        </n-step>
                        <n-step title="Team mapping">
                            <div class="n-step-description">
                                <n-space vertical v-if="NewLeague.status === 3">
                                    <n-grid cols="1" :x-gap="12" :y-gap="8" responsive="screen">
                                        <n-gi v-for="team, teamIndex in Teams" :key="teamIndex">
                                            <n-alert hoverable size="small" :title="team.teamName" :type="team.teamType"
                                                :show-icon="false">
                                                <n-space>
                                                    <template v-for="item, index in NewLeague.allPlayer" :key="index">
                                                        <n-checkbox v-model:checked="item.isChecked" :label="item.name"
                                                            v-if="item.teamId === 0 || item.teamId === team.teamId"
                                                            @click="OnCheck(item, team.teamId)" />
                                                    </template>
                                                </n-space>
                                            </n-alert>
                                        </n-gi>
                                    </n-grid>
                                    <n-space horizontal justify="end">
                                        <n-button type="info" size="small" @click="AutoMapping">
                                            Auto mapping
                                        </n-button>
                                        <n-button size="small" @click="prevButtonClick">
                                            Prev
                                        </n-button>
                                        <n-button size="small" @click="nextButtonClick">
                                            Next
                                        </n-button>
                                    </n-space>
                                </n-space>
                                <p v-else-if="NewLeague.status < 3">팀을 구성 하세요.</p>
                                <n-space vertical v-else>
                                    <n-grid cols="1" :x-gap="12" :y-gap="8" responsive="screen">
                                        <n-gi v-for="team, teamIndex in NewLeague.teams" :key="teamIndex">
                                            <teamCard :Team="team"></teamCard>
                                        </n-gi>
                                    </n-grid>
                                </n-space>

                            </div>
                        </n-step>
                        <n-step title="Game">
                            <div class="n-step-description">
                                <n-space vertical v-if="NewLeague.status === 4">
                                    <n-grid cols="1" :x-gap="12" :y-gap="8" responsive="screen">
                                        <n-gi v-for="game, gameIndex in NewLeague.games" :key="gameIndex">
                                            <n-card size="small" :title="(gameIndex + 1) + '경기'" v-if="!game.isEnd">

                                                <teamScore :Game="game" :IsLeft="true" :Teams="NewLeague.teams">
                                                </teamScore>

                                                <play-timer v-if="game.leftTeam && game.rightTeam" style="margin:20px 0px 20px 0px"></play-timer>
                                                <n-space Horizontal justify="center" style="margin:20px 0px 20px 0px">

                                                    <n-icon v-if="game.leftTeam" size="40">
                                                        {{ game.leftTeam.score }}
                                                    </n-icon>
                                                    <n-icon size="40">
                                                        vs
                                                    </n-icon>
                                                    <n-icon v-if="game.rightTeam" size="40">
                                                        {{ game.rightTeam.score }}
                                                    </n-icon>

                                                </n-space>

                                                <teamScore :Game="game" :IsLeft="false" :Teams="NewLeague.teams"
                                                    v-if="!game.isEnd">
                                                </teamScore>

                                            </n-card>
                                            <n-alert v-else @close="CloseGame(game)" closable :show-icon="false"
                                                :type="game.winTeamType">

                                                <n-space Horizontal justify="center">
                                                    <n-p>{{
                                                        game.leftTeam.teamName + ' ' + game.leftTeam.score +
                                                            ' vs ' + game.rightTeam.score + ' ' +
                                                            game.rightTeam.teamName
                                                    }}</n-p>
                                                </n-space>
                                            </n-alert>

                                        </n-gi>
                                    </n-grid>

                                    <n-space horizontal justify="end">

                                        <n-button size="small" @click="prevButtonClick"
                                            v-if="NewLeague.games.length < 2">
                                            Prev
                                        </n-button>
                                        <n-button size="small" @click="OnSaveGame(false)">
                                            Save Game
                                        </n-button>
                                        <n-button size="small" v-if="NewLeague.games.length > 1" type="info"
                                            @click="OnSaveGame(true)">
                                            End
                                        </n-button>
                                    </n-space>
                                </n-space>
                                <p v-else-if="NewLeague.status < 4">경기 내용을 기록 하세요.</p>
                            </div>
                        </n-step>
                    </n-steps>
                </n-space>
            </n-card>
        </n-gi>
    </n-grid>

</template>
<script setup>
import { defineExpose, defineEmits } from 'vue'
import { NAvatar, NCheckbox, TransferRenderTargetLabel } from 'naive-ui'
import teamCard from '@/views/settings/league/component/teamCard.vue'
import teamScore from '@/views/settings/league/component/teamScore.vue'
import playTimer from '@/views/settings/league/component/playTimer.vue'
import { ref, onMounted } from 'vue';
import { useStore } from "vuex";
import { ConvertDateToYYYYMMDD, ConvertYYYYMMDDToDate, ConvertYYYYMMDDToStringDate } from '@/zenc/js/Common'

const store = useStore();
const current = ref(true)
const Players = ref(null);
const Locations = ref(null);
const NewLeague = ref(null);
const SelectedPlayers = ref(new Array);
const Teams = ref(new Array);
const LeagueTeams = ref(new Array);

onMounted(async () => {
    await Init();
    await InitEditMode();
})
const Emits = defineEmits(['completed'])
const Init = async () => {
    Teams.value = await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'GetTeams', null);

    Locations.value = new Array;
    var lc = await store.state.apiClient.ExecDataTable('SQL', 'SELLOCATION', null);
    for (var i in lc) {
        Locations.value.push({ value: lc[i].pk_id, label: lc[i].col_name });
    }

    Players.value = new Array;
    var py = await store.state.apiClient.ExecDataTable('SQL', 'SELSIMPLEMEMBER', null);
    for (var i in py) {
        Players.value.push({ value: py[i].pk_id, label: py[i].col_name });
    }

    await InitEditMode();
}
const InitEditMode = async () => {

    var league = await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'GetRunningLeague',);

    if (league != null && league.leagueId) {

        SelectedPlayers.value = new Array;

        for (var i in league.allPlayer) {
            var player = league.allPlayer[i];
            SelectedPlayers.value.push(player.playerId)
        }
        NewLeague.value = league;
        if (NewLeague.value.locationId == 0) {
            NewLeague.value.locationId = null;
        }
        NewLeague.value.strstartdate = "";
        if (NewLeague.value.strDate) {
            NewLeague.value.starttimestamp = ConvertYYYYMMDDToDate(NewLeague.value.strDate);
            NewLeague.value.strstartdate = ConvertYYYYMMDDToStringDate(NewLeague.value.strDate);
        }
        else NewLeague.value.starttimestamp = null;


        NewLeague.value.games.push({ playId: null, leftTeam: null, rightTeam: null, isEnd: false, winTeamType: '' });


        return true;
    }



    return false;
}
const TxtPlayer = ref("");
const OnCheck = (item, id) => {
    item.teamId != id ? item.teamId = id : item.teamId = 0;
}
const pastePlayer = () => {
    var txt = TxtPlayer.value;

    SelectedPlayers.value = new Array;
    var splits = txt.split(",");
    var isEnd = false;
    if (splits) {
        for (var i in splits) {
            var str = splits[i];

            if (str) {

                var splits2 = str.split(" ");

                for (var k in splits2) {
                    var name = splits2[k];
                    if (name) {
                        
                        if (name.indexOf("예비") > -1) {
                            isEnd = true;
                        }
                        if (!isEnd) {
                            for (var k in Players.value) {
                                var p = Players.value[k];
                                if (p.label == name) {
                                    SelectedPlayers.value.push(p.value);
                                    continue;
                                }
                            }
                        }
                    }



                }
            }
        }

    }
}
const AutoMapping = () => {
    alert("승률 기준으로 자동으로 팀이 구성 됩니다.(데이터 부족으로 랜덤)");
    var total = Math.floor( NewLeague.value.allPlayer.length/3 );
    var sub = NewLeague.value.allPlayer.length%3;
    

    var rTeam = total;
    var bTeam = total;
    var yTeam = total;

    if(sub == 1){
        rTeam +=1;
    }
    else if(sub == 2){
        rTeam +=1;
        bTeam +=1;
    }
    var crTeam = 0;
    var cbTeam = 0;
    var cyTeam = 0;

    for (var i in NewLeague.value.allPlayer) {
        var player = NewLeague.value.allPlayer[i];
        if (player.teamId > 0) {

            if (player.teamId == 1) {
                crTeam +=1;
            } else if (player.teamId == 2) {
                cbTeam +=1;
            }
            else {
                cyTeam+=1;
            }
        }
    }

    for (var i in NewLeague.value.allPlayer) {
        var player = NewLeague.value.allPlayer[i];
        if (player.teamId == 0) {
            
            
            var v = Math.floor(Math.random() * 3)+1;
            for(var j =0 ;j<3;j++){
                if(v == 1 )
                {
                    if(rTeam > crTeam){
                        crTeam +=1;
                    }
                    else{
                        v +=1;
                        continue;
                    }
                }
                else if(v== 2){
                    if(bTeam > cbTeam){
                        cbTeam +=1;
                    }
                    else{
                        v +=1;
                        
                        continue;
                    }
                }
                else if(v== 3){
                    if(yTeam > cyTeam){
                        cyTeam +=1;
                    }
                    else{
                        v = 1;
                        
                        continue;
                    }
                }
                player.isChecked = true;
                OnCheck(player, v);
                break;
            }
        }
    }
}
const saveLeagueMember = async (isNext) => {

    await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'InsertLeagueMember', { leagueid: NewLeague.value.leagueId, ps: SelectedPlayers.value, isnext: isNext });

}
const nextButtonClick = async () => {
    if (NewLeague.value.status == 1) {
        if (NewLeague.value.starttimestamp && NewLeague.value.locationId > 0) {
            await store.state.apiClient.ExecNonQuery('SQL', 'UPDATELEAGUE',
                {
                    pk_id: NewLeague.value.leagueId
                    , fk_location_id: NewLeague.value.locationId
                    , col_status: 2
                    , col_date: ConvertDateToYYYYMMDD(new Date(NewLeague.value.starttimestamp))
                });
            await InitEditMode();
        }
    }
    else if (NewLeague.value.status == 2) {
        if (SelectedPlayers.value.length > 0) {
            await saveLeagueMember(true);
            await InitEditMode();
        }
    }
    else if (NewLeague.value.status == 3) {
        for (var i in NewLeague.value.allPlayer) {
            if (NewLeague.value.allPlayer[i].teamId == 0) {

                return;
            }
        }
        await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'TeamMapping', { leagueid: NewLeague.value.leagueId, players: NewLeague.value.allPlayer });
        await InitEditMode();
    }
    else if (NewLeague.value.status == 4) {
        await store.state.apiClient.ExecNonQuery('SQL', 'UPDATELEAGUESTATUS', { col_status: NewLeague.value.status + 1, pk_id: NewLeague.value.leagueId });
        NewLeague.value = null;
    }


}

const prevButtonClick = async () => {

    NewLeague.value.status = NewLeague.value.status - 1;
    await store.state.apiClient.ExecNonQuery('SQL', 'UPDATELEAGUESTATUS', { col_status: NewLeague.value.status, pk_id: NewLeague.value.leagueId });
    await InitEditMode();
}


const OnSaveGame = async (isEnd) => {
    if (isEnd) {
        if (!confirm("종료 하시겠습니까?")) {
            return;
        }


    }
    var games = NewLeague.value.games;
    var game = games[games.length - 1];
    if (game) {
        if (game.leftTeam && game.rightTeam) {
            game.isEnd = true;
            await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'UpsertGameInfo', { leagueid: NewLeague.value.leagueId, gameInfo: game });
            if (!isEnd)
                await InitEditMode();

        }
    }

    if (isEnd) {
        await nextButtonClick();
        Emits('completed');
    }

}

const CloseGame = async (item) => {
    await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'DeleteGameInfo', { gameid: item.playId });
    var games = NewLeague.value.games;
    for (var i in games) {
        var game = games[i];
        if (game == item) {
            games.splice(i, 1);
            break;
        }
    }
    return false;

}


const Create = async () => {

    if (await InitEditMode() == false) {
        await store.state.apiClient.ExecScalar('SQL', 'INSERTLEAGUE', null);
        await InitEditMode();
    }
}
defineExpose({ Create })

</script>