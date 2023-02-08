<template>
    <n-card title="데이터베이스 정보" size="small">
        <n-space vertical>
            <n-select v-model:value="props.Item.DBType" filterable placeholder="데이터베이스 타입을 선택해주세요." :options="options" />
            <n-input just v-model:value="props.Item.Server" type="text" placeholder="서버 정보를 입력해주세요." />
            <n-input-number v-model:value="props.Item.Port" type="text" placeholder="포트 번호를 입력해주세요." />
            <n-input v-model:value="props.Item.DatabaseName" type="text" placeholder="데이터베이스명을 입력해주세요." />
            <n-input v-model:value="props.Item.UserID" type="text" placeholder="아이디 정보를 입력해주세요." />
            <n-input  v-model:value="props.Item.Password" type="password" placeholder="비밀번호 정보를 입력해주세요." />
            <n-button @click="OnConnectionTest">Save</n-button>
        </n-space>
    </n-card>
</template>
<script setup>
import {ref,defineProps,onMounted} from "vue"
import { useStore } from "vuex";
import { GetDatabaseInfo } from "@/zenc/svg/js/Common"
const store = useStore()

const props = defineProps({ Item: { type: Object } })
const options = ref([{label:"MS-SQL",value:"0"},{label:"PostgreSQL",value:"1"},{label:"Oracle",value:"2"}]) 
const OnConnectionTest =async () =>{

    var param= new Object;
    param.info = GetDatabaseInfo(props.Item);

    var rtn = await store.state.apiClient.Run('EEH.DB', 'DbHandler', 'ConnectionTest', param);           
    if(rtn){
        

        if(!props.Item.DisplayName){
            props.Item.DisplayName = props.Item.DatabaseName+"("+GetOptionLabel(props.Item.DBType)+")";

        }
    }else{
        props.Item.Server ="";
        props.Item.Port ="";
        props.Item.DatabaseName ="";
        props.Item.UserID ="";
        props.Item.Password ="";
        props.Item.DBType =null;
    }

};
const GetOptionLabel = (value) => {
    
     for ( var i in options.value){
        var item = options.value[i];
        
        if(item.value.toString() == value){
            return item.label;
        }
     }

}

onMounted(()=>{
        props.Item.Server ="jmfc.eehnuyh.com";
        props.Item.Port ="5432";
        props.Item.DatabaseName ="JMFC";
        props.Item.UserID ="postgres";
        props.Item.Password ="HyunHee1228!@#";
        props.Item.DBType ="1";

})
</script>