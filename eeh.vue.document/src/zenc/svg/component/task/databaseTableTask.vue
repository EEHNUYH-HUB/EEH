<template>
    <n-card title="테이블 정보" size="small" v-if="tables">
        <n-space vertical>
            <n-select
            @update:value="handleUpdateValue"
            v-model:value="props.Item.TableName" filterable placeholder="데이터베이스 타입을 선택해주세요."
                :options="tables" />
        </n-space>
    </n-card>
    <n-card v-else>
        <n-p>테이블 정보를 가지고 올수 없습니다.</n-p>
    </n-card>
</template>
<script setup>
import { ref, defineProps, onMounted ,watch} from "vue"
import { useStore } from "vuex"
import { GetDatabaseInfo } from "@/zenc/svg/js/Common"

const store = useStore()
const props = defineProps({ Item: { type: Object } })

const tables = ref(null)
const dbObj = ref(null)

onMounted(async () => {
    dbObj.value = GetDatabaseInfo(props.Item);
    
    if(dbObj.value && dbObj.value.DBType && dbObj.value.Server && dbObj.value.DatabaseName){
        tables.value = new Array;
        var param = new Object;
        param.info = dbObj.value;
        var strTables = await store.state.apiClient.Run('EEH.DB', 'DbHandler', 'GetTables', param);
        if (strTables) {
            for (var j in strTables) {
                var tableName = strTables[j];
                tables.value.push({ label: tableName, value: tableName });

            }
        }
    }
    else{
        tables.value = null;
    }

})
const handleUpdateValue = (value)=>{

    props.Item.DisplayName = value;
}
</script>