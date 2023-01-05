<template>
    <div :class="store.state.useIsMobile?'div_root_mobile':'div_root_default'" >
        <div :class="store.state.useIsMobile?'div_mobile':'div_default'" class=" document-scroll-container">    
            <Breadcrumb></Breadcrumb>
            
            <n-grid cols="1 s:1 m:1 l:2 xl:2 2xl:2" 
            :x-gap="12" :y-gap="8" 
            responsive="screen">
                
                <n-grid-item >
                    <n-card  id="id1" title="ExecuteDataTable" hoverable>
                        <n-form-item path="scope" label="Scope">
                            <n-input v-model:value="ApiParamDT.scope"  placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="statementId" label="StatementId">
                            <n-input v-model:value="ApiParamDT.statementId"  placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="parameters" label="Parameters">
                            <n-dynamic-input v-model:value="ApiParamDT.parameters" preset="pair" key-placeholder="Please input the key" value-placeholder="Please input the value" />
                        </n-form-item>

                        <n-data-table :columns="dtColumns" :data="dtData"  :bordered="false" />

                        <n-button ghost @click="OnExecuteDataTable" style="margin-top:24px"> ExecuteDataTable </n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                 <n-card  id="id2" title="ExecuteDataTable Code" hoverable>
                    <n-code word-wrap code="import { useStore } from 'vuex';
import { ref } from 'vue';

const store = useStore();

const ApiParamDT= ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });

var result = 
await store.state.apiClient.ExecDataTable(ApiParamDT.value.scope,ApiParamDT.value.statementId,ApiParamDT.value.parameters);

//DataGrid Column 생성
const dtColumns = ref(new Array);
const dtData = ref(new Array);

var columns = new Array;
var cols = GetColumnsforDataTable(result);
for(var i in cols){
    columns.push( {key:cols[i],title:cols[i]} );
}
dtColumns.value = columns;
dtData.value = result;
" language="js" />

                </n-card>
            </n-grid-item>
            <n-grid-item >
                    <n-card  id="id3" title="ExecuteNonQuery" hoverable>
                        <n-form-item path="scope" label="Scope">
                            <n-input v-model:value="ApiParamN.scope"  placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="statementId" label="StatementId">
                            <n-input v-model:value="ApiParamN.statementId"  placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="parameters" label="Parameters">
                            <n-dynamic-input v-model:value="ApiParamN.parameters" preset="pair" key-placeholder="Please input the key" value-placeholder="Please input the value" />
                    
                        </n-form-item>
                        <n-button ghost @click="OnExecuteNonQuery" > ExecuteNonQuery </n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                 <n-card  id="id4" title="ExecuteNonQuery Code" hoverable>
                    <n-code word-wrap code="import { useStore } from 'vuex';

const store = useStore();

const ApiParamN= ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });


var result = 
await store.state.apiClient.ExecNonQuery(ApiParamN.value.scope,ApiParamN.value.statementId,ApiParamN.value.parameters);
" language="js" />
                </n-card>
            </n-grid-item>
            <n-grid-item >
                    <n-card  id="id5" title="ExecuteDataSet" hoverable>
                        <n-form-item path="scope" label="Scope">
                            <n-input v-model:value="ApiParamDS.scope" placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="statementId" label="StatementId">
                            <n-input v-model:value="ApiParamDS.statementId"  placeholder="Please input the value"  />
                        </n-form-item>
                        <n-form-item path="parameters" label="Parameters">
                            <n-dynamic-input v-model:value="ApiParamDS.parameters" preset="pair" 
                            key-placeholder="Please input the key" value-placeholder="Please input the value" />
                    
                        </n-form-item>
                        <n-button ghost @click="OnExecuteDataSet" > ExecuteDataSet </n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                 <n-card  id="id6" title="ExecuteDataSet Code" hoverable>
                    
                    <n-code word-wrap code="import { useStore } from 'vuex';

const store = useStore();

const ApiParamDS= ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });

var result = 
await store.state.apiClient.ExecDataSet(ApiParamDS.value.scope,ApiParamDS.value.statementId,ApiParamDS.value.parameters);  
" language="js" />
                </n-card>
            </n-grid-item>
                <n-grid-item >
                    <n-card  id="id7" title="ExecuteScalar" hoverable>
                        <n-form-item path="scope" label="Scope">
                            <n-input v-model:value="ApiParamS.scope"  placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="statementId" label="StatementId">
                            <n-input v-model:value="ApiParamS.statementId"  placeholder="Please input the value" />
                        </n-form-item>
                        <n-form-item path="parameters" label="Parameters">
                            <n-dynamic-input v-model:value="ApiParamS.parameters" preset="pair" key-placeholder="Please input the key" value-placeholder="Please input the value" />
                    
                        </n-form-item>
                        <n-button ghost @click="OnExecuteScalar" > ExecuteScalar </n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                 <n-card  id="id8" title="ExecuteScalar Code" hoverable>
                    <n-code word-wrap code="import {GetColumnsforDataTable} from '@/zenc/js/Common'
import { useStore } from 'vuex';

const store = useStore();

const ApiParamS= ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });

var result = 
await store.state.apiClient.ExecScalar(ApiParamS.value.scope,ApiParamS.value.statementId,ApiParamS.value.parameters);
" language="js" />
                </n-card>
            </n-grid-item>
                
            </n-grid>
        </div>
        
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    
    </div>
    </template>
      
    <script setup>
    import { ref } from 'vue';
    import  Breadcrumb  from "@/zenc/layout/components/Breadcrumb.vue"
    import Anchor from "@/zenc/layout/components/Anchor.vue"
    import {GetColumnsforDataTable} from '@/zenc/js/Common'
    import { useStore } from "vuex";

    const store = useStore();
    
    const ApiParamDT = ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });
    const ApiParamDS= ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });
    const ApiParamS = ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });
    const ApiParamN= ref({
        scope: '',
        statementId : '',
        parameters: [{key:'',value:''}]
    });
    const OnExecuteDataSet = async function(){   
        var ds = await store.state.apiClient.ExecDataSet(ApiParamDS.value.scope,ApiParamDS.value.statementId,ApiParamDS.value.parameters);  
        console.log(ds)
    }

    const dtColumns = ref(new Array);
    const dtData = ref(new Array);

    const OnExecuteDataTable = async function(){
       var dt = await store.state.apiClient.ExecDataTable(ApiParamDT.value.scope,ApiParamDT.value.statementId,ApiParamDT.value.parameters);
       
       var columns = new Array;
       var cols = GetColumnsforDataTable(dt);
       for(var i in cols){
            columns.push( {key:cols[i],title:cols[i]} );
       }
       dtColumns.value = columns;
       dtData.value = dt;
    }
    const OnExecuteScalar = async function(){        
        var result = await store.state.apiClient.ExecScalar(ApiParamS.value.scope,ApiParamS.value.statementId,ApiParamS.value.parameters);
        console.log(result)
    }
    const OnExecuteNonQuery = async function(){
        var result = await store.state.apiClient.ExecNonQuery(ApiParamN.value.scope,ApiParamN.value.statementId,ApiParamN.value.parameters);
        console.log(result)
    }
    const AnchorItems = ref(new Array);
    AnchorItems.value.push({Title:'ExecuteDataTable',ID:'#id1'});
    AnchorItems.value.push({Title:'ExecuteDataTable Code',ID:'#id2'});
    AnchorItems.value.push({Title:'ExecuteNonQuery',ID:'#id3'});
    AnchorItems.value.push({Title:'ExecuteNonQuery Code',ID:'#id4'});
    AnchorItems.value.push({Title:'ExecuteDataSet',ID:'#id5'});
    AnchorItems.value.push({Title:'ExecuteDataSet Code',ID:'#id6'});
    AnchorItems.value.push({Title:'ExecuteScalar',ID:'#id7'});
    AnchorItems.value.push({Title:'ExecuteScalar Code',ID:'#id8'});
    
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