<template>
<div :class="store.state.useIsMobile?'div_root_mobile':'div_root_default'" >
    <div :class="store.state.useIsMobile?'div_mobile':'div_default'" class=" document-scroll-container">    
        <Breadcrumb></Breadcrumb>
        
        <n-grid cols="1 s:1 m:1 l:2 xl:2 2xl:2" 
        :x-gap="12" :y-gap="8" 
        responsive="screen">
            <n-grid-item >
                <n-card  id="id1" title="Generate Key" hoverable>
                    
                    <n-dynamic-input v-model:value="Params" preset="pair" key-placeholder="Please input the key" value-placeholder="Please input the value" />
                    
                    <n-p>
                         {{GenerateKeyResult}}
                    </n-p>
                    <n-button ghost @click="OnGenerateKey" > GenerateKey </n-button>
                </n-card>
            </n-grid-item>
            <n-grid-item >
                 <n-card  id="id2" title="Generate Key Code" hoverable>
                    
                    <n-code word-wrap code="import {ConvertJsonToKeyValueObject} from '@/zenc/js/Common'
import { useStore } from 'vuex';

const store = useStore();

const GenerateKeyResult = ref('');

const Params = ref([{key:'UserName',value:''},{key:'UserID',value:''},{key:'Email',value:''}]);
GenerateKeyResult.value = await store.state.apiClient.GenerateKey(Params.value);
" language="js" />



  

                </n-card>
            </n-grid-item>
            <n-grid-item >
                <n-card  id="id3" title="Certification Info" hoverable>
                    <n-input type="textarea" :value="GenerateKeyResult" placeholder="Generate Key" :autosize="{minRows: 1}"></n-input>
                    <n-p></n-p>
                    <n-p v-for="(item,index) in CertificationInfoResult" :key="index">
                        {{item}}
                    </n-p>
                    <n-button ghost @click="OnCertificationInfo" > CertificationInfo </n-button>
                </n-card>
            </n-grid-item>
            <n-grid-item >
                 <n-card  id="id4" title="Certification Info Code" hoverable>
                    
                    <n-code word-wrap code="import {ConvertJsonToKeyValueObject} from '@/zenc/js/Common'
import { useStore } from 'vuex';

const store = useStore();
const CertificationInfoResult = ref(null);
CertificationInfoResult.value = ConvertJsonToKeyValueObject(await store.state.apiClient.CertificationInfo(GenerateKeyResult.value));
" language="javascript" />
                    
                    
                </n-card>
            </n-grid-item>
        </n-grid>
    </div>
    
    <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>

</div>
</template>
  
<script setup>
import { ref } from 'vue';
import { useStore } from "vuex";
import  Breadcrumb  from "@/zenc/layout/components/Breadcrumb.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"

import {ConvertJsonToKeyValueObject} from '@/zenc/js/Common'

const store = useStore()
const Params = ref([{key:'UserName',value:''},{key:'UserID',value:''},{key:'Email',value:''}]);
const GenerateKeyResult = ref('');
const CertificationInfoResult = ref(null);
const OnGenerateKey = async function(){
    GenerateKeyResult.value = await store.state.apiClient.GenerateKey(Params.value);
}
const OnCertificationInfo = async function(){
    CertificationInfoResult.value = ConvertJsonToKeyValueObject(await store.state.apiClient.CertificationInfo(GenerateKeyResult.value));
}

const AnchorItems = ref(new Array);
AnchorItems.value.push({Title:'Generate Key',ID:'#id1'});
AnchorItems.value.push({Title:'Generate Key Code',ID:'#id2'});
AnchorItems.value.push({Title:'Certification Info',ID:'#id3'});
AnchorItems.value.push({Title:'Certification Info Code',ID:'#id4'});

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