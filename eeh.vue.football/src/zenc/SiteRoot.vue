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

import {onMounted} from 'vue'
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

onMounted(async ()=>{
var params = [{key:'UserName',value:'JMFC'},{key:'UserID',value:'JMFC'},{key:'Email',value:'jmfc@naver.com'}];
var key = await store.state.apiClient.GenerateKey(params);

})
</script>