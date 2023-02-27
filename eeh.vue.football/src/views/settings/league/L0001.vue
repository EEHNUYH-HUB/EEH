<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb :Buttons="Buttons"></Breadcrumb>
            
            <createLeague v-for="item,index in NewLeagues"  :key="index" :NewLeague ="item"  @completed="Load"></createLeague>
            <n-grid cols="1 s:1 m:1 l:1 xl:2 2xl:2" :x-gap="12" :y-gap="8" responsive="screen">
                <n-gi  v-for="item,index in ResultLeague" :key="index">
                    <resultLeagueCard :Item="item" @ondeleted="Load"></resultLeagueCard>
                </n-gi>
            </n-grid>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>
</template>
<script setup>
import { ref, onMounted } from 'vue';
import { useStore } from "vuex";
import createLeague from '@/views/settings/league/component/createLeague.vue'
import resultLeagueCard from '@/views/settings/league/component/resultLeagueCard.vue'
import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"
import { ConvertYYYYMMDDToStringDate } from '@/zenc/js/Common'
const store = useStore();
const Buttons = ref(new Array);

const AnchorItems = ref(new Array);
const NewLeagues = ref(new Array);
const ResultLeague = ref(null);
var btn = new Object;  
btn.Name = "Create League";
btn.OnClick = async () => {
    
    await store.state.apiClient.ExecScalar('SQL', 'INSERTLEAGUE', null);
    await Load();

}
Buttons.value.push(btn);

onMounted(async()=>{

    await Load();
})

const Load = async()=>{
    
    NewLeagues.value = await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'GetRunningLeague',);
console.log(NewLeagues.value)
    ResultLeague.value = await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'GetLeagueHistory', null);           
    
    
    AnchorItems.value = new Array;
    
    for (var i in ResultLeague.value) {
        var item = ResultLeague.value[i];
        AnchorItems.value.push({ Title: ConvertYYYYMMDDToStringDate(item.strDate), ID: '#rlc' + item.leagueID });

    }
    
}
</script>
