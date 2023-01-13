<template>
    
    <n-config-provider :locale="koKR" :date-locale="dateKoKR"  :theme="store.state.themeObj" :hljs="hljs">
        
                        <router-view  />
    </n-config-provider>
</template>
<script setup>
import { useStore } from "vuex";
import hljs from 'highlight.js/lib/core'
import javascript from 'highlight.js/lib/languages/javascript'
import cpp from 'highlight.js/lib/languages/cpp'
import csharp from 'highlight.js/lib/languages/csharp'

import {onMounted,onBeforeMount} from 'vue'
import { koKR, dateKoKR } from "naive-ui";
import { useLoadingBar,useMessage,useNotification  } from 'naive-ui'


hljs.registerLanguage('javascript', javascript)
hljs.registerLanguage('cpp', cpp)
hljs.registerLanguage('csharp', csharp)


const store = useStore()
store.commit('init'); 
store.state.apiClient.LodingBar = useLoadingBar();
store.state.apiClient.Message = useMessage();
//store.state.apiClient.Notification = useNotification(); 
store.state.apiClient.Store = store;

onBeforeMount(async ()=>{
// var params = [{key:'UserName',value:'JMFC'},{key:'UserID',value:'JMFC'},{key:'Email',value:'jmfc@naver.com'}];
// var key = await store.state.apiClient.GenerateKey(params);
// console.log(key)
var key = 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lSWRlbnRpZmllciI6IjhiMjQ1MDZjLWYxZjMtNGQxYS1hZGRkLTJmZjRkN2IyNTY3MCIsIlVzZXJOYW1lIjoiSk1GQyIsIlVzZXJJRCI6IkpNRkMiLCJFbWFpbCI6ImptZmNAbmF2ZXIuY29tIiwibmJmIjoxNjczMzQxMTQ1LCJleHAiOjE2NzM0Mjc1NDUsImlzcyI6Imh0dHBzOi8vY28ua3IiLCJhdWQiOiJodHRwczovL2NvLmtyIn0.rB1ZRcayQ9HxoDuXndxYUeJsm6hcOizwo4UVrm-TUag';
sessionStorage.setItem("apikey",key);
await store.state.apiClient .CertificationInfo(key);
})
</script>

<style >
.n-button {

    float: right;
}

.n-grid {
    margin-top: 24px;
}

.div_root_default {
    display: flex;
    flex-wrap: nowrap;
    padding: 32px 24px 56px 56px;
}

.div_root_mobile {
    display: flex;
    flex-wrap: nowrap;
    padding: 24px;
}

.div_default {
    width: calc(100% - 228px);
    margin-right: 36px;
}

.div_mobile {
    width: calc(100%);
}
</style>