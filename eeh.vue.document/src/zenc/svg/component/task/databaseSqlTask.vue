<template>
    <n-grid cols="1" :x-gap="12" :y-gap="8" responsive="screen">
        <n-gi>
            <n-card title="다이어그램" size="small">
                <svgPanel ref="svgPanelCtrl" Height="500px" SvgHeight="1000px"></svgPanel>
            </n-card>
        </n-gi>
        <n-gi>
            <n-card title="쿼리" size="small">
                <template #header-extra>
                    <n-button @click="OnCreateQuery" size="small">쿼리생성</n-button>
                </template>
                <n-code word-wrap :code="Query" language="sql" />

            </n-card>
        </n-gi>
        <n-gi>
            <n-card title="결과" size="small">
                <template #header-extra>
                    <n-button @click="OnCreateQuery" size="small">쿼리생성</n-button>
                </template>
            </n-card>
        </n-gi>
    </n-grid>
</template>
<script setup>
import { ref, defineProps, onMounted } from "vue"
import { useStore } from "vuex";
import { GetDatabaseInfo, GetIcon ,ExcelCellName} from "@/zenc/svg/js/Common"
import svgPanel from "@/zenc/svg/component/svgPanel.vue"
import DrawPicker from '@/zenc/svg/js/DrawPicker'
import DrawSqlPicker from '@/zenc/svg/js/DrawSqlPicker'
import { Alert } from "@vicons/ionicons5";
const store = useStore()
const svgPanelCtrl = ref();
const props = defineProps({ Item: { type: Object } })
const dbObj = ref(null);
const Query = ref("");
const AliasArray = ref(['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'])
onMounted(async () => {
    

    // SvgList.value = svgPanelCtrl.value.SvgList;
    // JoinList.value = svgPanelCtrl.value.JoinList;

    svgPanelCtrl.value.SetPicker("sql");
    var picker = svgPanelCtrl.value.Picker;
    dbObj.value = GetDatabaseInfo(props.Item);


    var joins = props.Item.JoinObjs;
    if (joins) {
        for (var i in joins) {
            var beforeItem = joins[i].StartObj;
            if (beforeItem.IconType == 'table') {
                var param = new Object;
                param.info = dbObj.value;
                param.tableName = beforeItem.TableName;
                var cols = await store.state.apiClient.Run('EEH.DB', 'DbHandler', 'GetColumns', param);
                var colorObj = new Object;
                colorObj.Stroke = beforeItem.StrokeColor;
                colorObj.Fill = beforeItem.FillColor;
                var obj = GetIcon('table', 100, 80 + (80 * i), 30, colorObj);
                obj.IsShowDisplayName = true;
                obj.DisplayName = obj.TableName = beforeItem.TableName;
                obj.SubName = obj.Alias = ExcelCellName(i);
                obj.Columns = new Array;
                for (var i in cols) {
                    obj.Columns.push({ label: cols[i], value: cols[i] });
                }


                picker.ObjList.push(obj);

            }
        }
    }
    OnCreateQuery();
})

const OnCreateQuery = () => {
    var picker = svgPanelCtrl.value.Picker;
    var joins = picker.JoinList;
    
    
    
    var nonJoin = new Array;
    var query = "SELECT             *  \r\nFROM               ";
    var writeTable = new Array;
    for (var i in joins) {
        var join = joins[i];
if(join.StartJoinColumn && join.EndJoinColumn){

        var startTable = join.StartObj;
        var isStartWrite = !UseTable(startTable,writeTable);
        var endTable = join.EndObj;
        var isEndWrite = !UseTable(endTable,writeTable);

        var startTableQuery ="";
        var isLeftStart= isStartWrite && isEndWrite?true:(!isStartWrite)
        if(isStartWrite){
            startTableQuery =  startTable.TableName.toUpperCase() +' AS '+startTable.Alias.toUpperCase();
        }
        var endTableQuery ="";
        if(isEndWrite){
            endTableQuery =  endTable.TableName.toUpperCase() +' AS '+endTable.Alias.toUpperCase()
        }
        
        var joinQuery  = "";
        if(join.JoinArrow=="START"){
            if(isLeftStart)
                joinQuery = '\r\nLEFT OUTER JOIN   '
            else
                joinQuery = '\r\nRIGHT OUTER JOIN  '
        }
        else if(join.JoinArrow=="END"){
            if(isLeftStart)
                joinQuery = '\r\nRIGHT OUTER JOIN  '
            else
                joinQuery = '\r\nLEFT OUTER JOIN   '
        }
        else{
            joinQuery = '\r\nINNER JOIN        '
        }
        if(isStartWrite && isEndWrite){
            query += startTableQuery +' '+joinQuery+' ' +endTableQuery;
        }
        else if(isStartWrite){
            query +=  ' '+joinQuery+' ' +startTableQuery;
        }
        else if(isEndWrite){
            query +=  ' '+joinQuery+' ' +endTableQuery;
        }

        query += ' ON '+ join.StartObj.Alias+"."+ join.StartJoinColumn.toUpperCase() +' = '+join.EndObj.Alias+"."+ join.EndJoinColumn.toUpperCase()
    }
    }
    function UseTable(table,writeTable){
        for(var i in writeTable){
            var as = writeTable[i].Alias;
            if(table.Alias == as){
                return true;
            }
        }
        writeTable.push(table);
        return false;
    }
   
    Query.value = query;
}
</script>