<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb :Buttons="Buttons"></Breadcrumb>
            <n-grid cols="1 s:1 m:1 l:1 xl:2 2xl:2" :x-gap="12" :y-gap="8" responsive="screen">
                <n-gi v-for="(item, index) in memberItems" :key="index">
                    <playerCard :Item="item" ></playerCard>
                </n-gi>
            </n-grid>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>

    <createPlayer v-model:Show="IsShowCreatePanel" @changed="Load"></createPlayer>
</template>
  
<script setup>
import { ref, onMounted } from 'vue';
import { useStore } from "vuex";


import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"
import playerCard from "@/views/settings/player/component/playerCard.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"
import createPlayer from '@/views/settings/player/component/createPlayer.vue'
const store = useStore()
const memberItems = ref(null)
const AnchorItems = ref(new Array);
const Buttons = ref(new Array);
const IsShowCreatePanel = ref(false);

var btn = new Object;
btn.Name = "Create Player";
btn.OnClick = () => {    
    IsShowCreatePanel.value = true;
}
Buttons.value.push(btn);

onMounted(async () => {
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

</script>
