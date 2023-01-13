<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb :Buttons="Buttons"></Breadcrumb>
            <n-grid cols="1 s:1 m:1 l:1 xl:2 2xl:2" :x-gap="12" :y-gap="8" responsive="screen">
                <n-gi v-for="(item, index) in memberItems" :key="index">
                    <playerCard :Item="item" @click="OnSelectPlayer(item)"></playerCard>
                </n-gi>
            </n-grid>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>

    <n-drawer v-model:show="IsShowCreatePanel" :default-width="420" placement="right" resizable>
        <n-drawer-content title="Create Player" closable>
            <n-space vertical>
                <n-input v-model:value="newItem.name" placeholder="이름을 입력해 주세요" />
                <n-date-picker v-model:value="newItem.birtyDay" placeholder="생년월일을 입력해 주세요" type="date" />
                <n-input v-model:value="newItem.phone" :allow-input="OnlyAllowNumber" placeholder="전화번호를 입력해 주세요" />
            </n-space>
            <template #footer>
                <n-button @click="OnCretePlayer">
                    저장
                </n-button>
            </template>
        </n-drawer-content>
    </n-drawer>
</template>
  
<script setup>
import { ref, onMounted } from 'vue';
import { useStore } from "vuex";

import {  OnlyAllowNumber ,ConvertDateToYYYYMMDD} from '@/zenc/js/Common'
import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"
import playerCard from "@/views/settings/player/component/playerCard.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"
const store = useStore()
const memberItems = ref(null)
const AnchorItems = ref(new Array);
const Buttons = ref(new Array);
const IsShowCreatePanel = ref(false);
const newItem = ref(null);

const OnCretePlayer = async () => {
    
    var params = [
        { key: 'name', value: newItem.value.name },
        { key: 'phone', value: newItem.value.phone },
        { key: 'birthday', value: ConvertDateToYYYYMMDD(new Date(newItem.value.birtyDay))},
        { key: 'pkid', value: newItem.value.pkid}
    ];

    await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'UpsertMember',params);

    await Load();
    IsShowCreatePanel.value = false;

}

const InitNewItem = (item) => {
    newItem.value = new Object;
    if (item) {
        newItem.value.name = item.col_name;
        var y = parseInt(item.col_birthday.substring(0,4));
        var m = parseInt(item.col_birthday.substring(4,6))-1;
        var d = parseInt(item.col_birthday.substring(6,8));

        
        var newDate = new Date( y, m, d);
        newItem.value.birtyDay = newDate.getTime();
        newItem.value.phone = item.col_phone;
        newItem.value.pkid= item.pk_id;
    }
    else {
        newItem.value.name = "";
        newItem.value.birtyDay = null;
        newItem.value.phone = "";
        newItem.value.pkid = -1;;
    }

}
var btn = new Object;
btn.Name = "Create Player";
btn.OnClick = () => {
    InitNewItem();
    IsShowCreatePanel.value = true;
}
Buttons.value.push(btn);

onMounted(async () => {
    InitNewItem();
    await Load();
})

const Load = async () => {
    memberItems.value = await store.state.apiClient.ExecDataTable('SQL', 'SELMEMBER', null);

    AnchorItems.value = new Array;
    for (var i in memberItems.value) {
        var item = memberItems.value[i];
        AnchorItems.value.push({ Title: item.col_name, ID: '#id' + item.pk_id });

    }

}

const OnSelectPlayer = (item) =>{
    InitNewItem(item);
    IsShowCreatePanel.value = true;

}
</script>
