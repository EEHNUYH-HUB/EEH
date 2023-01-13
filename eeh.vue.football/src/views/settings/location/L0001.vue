<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb :Buttons="Buttons" ></Breadcrumb>
            <n-grid cols="1 s:1 m:1 l:2 xl:2 2xl:2" :x-gap="12" :y-gap="8" responsive="screen">
            <n-gi v-for="(item,index) in locationItems" :key="index" >
            <LocatonCard :Item="item" @click="OnSelectLocation(item)"></LocatonCard>
        </n-gi >
        </n-grid>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>

    <n-drawer v-model:show="IsShowCreatePanel" :default-width="420" placement="right" resizable>
        <n-drawer-content title="Create Player" closable>
            <n-space vertical>


                <n-input v-model:value="newItem.col_name" placeholder="이름을 입력해 주세요" />
                <n-input v-model:value="newItem.col_address" placeholder="도로명 주소를 입력해 주세요" />
                <n-input v-model:value="newItem.col_address2" placeholder="지번 주소를 입력해 주세요" />
                <n-input v-model:value="newItem.col_latitude" :allow-input="OnlyAllowDouble" placeholder="위도를 입력해 주세요" />
                <n-input v-model:value="newItem.col_longitude" :allow-input="OnlyAllowDouble" placeholder="경도를 입력해 주세요" />

            </n-space>
            <template #footer>
                <n-button @click="OnCreteLocation">
                    저장
                </n-button>
            </template>
        </n-drawer-content>
    </n-drawer>
</template>
  
<script setup>
import { ref,onMounted } from 'vue';
import { useStore } from "vuex";

import {OnlyAllowDouble} from '@/zenc/js/Common'
import  Breadcrumb  from "@/zenc/layout/components/Breadcrumb.vue"
import LocatonCard from "@/views/settings/location/component/locationCard.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"
const store = useStore()
const locationItems = ref(null)
const AnchorItems = ref(new Array);
const Buttons = ref(new Array);
const IsShowCreatePanel = ref(false);
const newItem = ref(null);


const OnCreteLocation = async () => {

    var param= new Object;
    param.row = newItem.value;
    await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'Upsertlocation',param);

    await Load();
    IsShowCreatePanel.value = false;

}


const InitNewItem = (item) => {
    
    if (item) {
        newItem.value = item;
    }
    else {
        
        newItem.value = new Object;
        newItem.value.col_name = "";
        newItem.value.col_address = "";
        newItem.value.col_address2 = "";
        newItem.value.col_latitude = 0;
        newItem.value.col_longitude = 0;
        newItem.value.col_longitude = -1;
    }

}

var btn = new Object;
btn.Name = "Create Location";
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
    locationItems.value = await store.state.apiClient.ExecDataTable('SQL', 'SELLOCATION', null);

    AnchorItems.value = new Array;
    for (var i in locationItems.value) {
        var item = locationItems.value[i];
        AnchorItems.value.push({ Title: item.col_name, ID: '#id' + item.pk_id });

    }
}


const OnSelectLocation = (item) =>{
    InitNewItem(item);
    IsShowCreatePanel.value = true;

}
</script>
