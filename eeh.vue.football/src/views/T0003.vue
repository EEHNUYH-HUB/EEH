<template>
    <div :class="store.state.useIsMobile?'div_root_mobile':'div_root_default'" >
        <div :class="store.state.useIsMobile?'div_mobile':'div_default'" class=" document-scroll-container">    
            <Breadcrumb></Breadcrumb>
            
            <n-grid cols="1 s:1 m:1 l:2 xl:2 2xl:2" 
            :x-gap="12" :y-gap="8" 
            responsive="screen">
                <n-grid-item >
                    <n-card  id="id1" title="ExecuteDataSet" hoverable>
                        <n-button ghost @click="OnExecuteDataSet" > ExecuteDataSet</n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                    <n-card  id="id2" title="ExecuteDataTable" hoverable>
                        <n-button ghost @click="OnExecuteDataTable" > ExecuteDataTable </n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                    <n-card  id="id3" title="ExecuteScalar" hoverable>
                        <n-button ghost @click="OnExecuteScalar" > ExecuteScalar </n-button>
                    </n-card>
                </n-grid-item>
                <n-grid-item >
                    <n-card  id="id4" title="ExecuteNonQuery" hoverable>
                        <n-button ghost @click="OnExecuteNonQuery" > ExecuteNonQuery </n-button>
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
    import APIClient from '@/zenc/js/RestApiClient'
    import {ConvertJsonToKeyValueObject,ConvertKeyValueObjectToObject} from '@/zenc/js/Common'
    import  Breadcrumb  from "@/zenc/layout/components/Breadcrumb.vue"
    import Anchor from "@/zenc/layout/components/Anchor.vue"
    const store = useStore()
    
    const OnExecuteDataSet = async function(){        
    }
    const ExecuteDataTable = async function(){
    }
    const ExecuteScalar = async function(){        
    }
    const ExecuteNonQuery = async function(){
    }
    const AnchorItems = ref(new Array);
    AnchorItems.value.push({Title:'ExecuteDataSet',ID:'#id1'});
    AnchorItems.value.push({Title:'ExecuteDataTable',ID:'#id2'});
    AnchorItems.value.push({Title:'ExecuteScalar',ID:'#id3'});
    AnchorItems.value.push({Title:'ExecuteNonQuery',ID:'#id4'});
    
    </script>
    