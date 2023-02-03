<template>
    <n-card title="쿼리 정보" size="small">
        <svgPanel ref="svgPanelCtrl"></svgPanel>
    </n-card>
</template>
<script setup>
import {ref,defineProps,onMounted} from "vue"
import { useStore } from "vuex";
import { GetDatabaseInfo ,GetIcon } from "@/zenc/svg/js/Common"
import svgPanel from  "@/zenc/svg/component/svgPanel.vue"

const store = useStore()
const svgPanelCtrl = ref();
const props = defineProps({ Item: { type: Object } })


const SvgList = ref(null);
const JoinList = ref(null);const 
dbObj = ref(null)

onMounted(async()=>{

    SvgList.value = svgPanelCtrl.value.SvgList;
    JoinList.value = svgPanelCtrl.value.JoinList;
    
    dbObj.value = GetDatabaseInfo(props.Item);

    
    var joins = props.Item.JoinObjs;
    if (joins) {
        for (var i in joins) {
            var beforeItem = joins[i].StartObj;
            if (beforeItem.IconType == 'table') {
                var param = new Object;
                param.info = dbObj.value;
                param.tableName =   beforeItem.TableName;
                
                var cols =  await store.state.apiClient.Run('EEH.DB', 'DbHandler', 'GetColumns', param);
                var colorObj = new Object;
                colorObj.Stroke = beforeItem.StrokeColor;
                colorObj.Fill  = beforeItem.FillColor;

                var obj =GetIcon('table',150,30*(i+1),200,colorObj);
                obj.Type = "ITEM";
                obj.TableName = beforeItem.TableName;
                obj.Columns = cols;

                SvgList.value.push(obj);
             
            }
        }
    }



})
</script>