<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb></Breadcrumb>
            <playerCard v-for="(item,index) in locationItems" :key="index" style="margin:20px 2px" :Item="item"></playerCard>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>
</template>
  
<script setup>
import { ref,onMounted } from 'vue';
import { useStore } from "vuex";

import {ConvertKeyValueObjectToObject} from '@/zenc/js/Common'
import  Breadcrumb  from "@/zenc/layout/components/Breadcrumb.vue"
import playerCard from "@/views/settings/player/component/playerCard.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"
const store = useStore()
const locationItems = ref(null)
const AnchorItems = ref(new Array);

onMounted (async ()=>{

    const ApiParamDT= ref({
        scope: 'SQL',
        statementId : 'SELLOCATION',
        parameters: null
    });
    locationItems.value =  await store.state.apiClient.ExecDataTable(ApiParamDT.value.scope,ApiParamDT.value.statementId,ConvertKeyValueObjectToObject(ApiParamDT.value.parameters));
    for(var i in locationItems.value){
        var item = locationItems.value[i];
        AnchorItems.value.push({ Title: item.col_name, ID: '#id'+item.pk_id });
    
    }
})
</script>

<style scoped>

.n-button {
    
    float: right;
}
.n-grid {
    margin-top: 24px;    
}

.div_root_default {
    display: flex; flex-wrap: nowrap; padding: 32px 24px 56px 56px;
}
.div_root_mobile {
    display: flex; flex-wrap: nowrap; padding: 24px;
}
.div_default {
    width: calc(100% - 228px); margin-right: 36px;
}
.div_mobile {
    width: calc(100%); 
}
</style>