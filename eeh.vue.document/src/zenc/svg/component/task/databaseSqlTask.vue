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
                    <n-button @click="OnBuild" size="small">빌드</n-button>
                </template>
                <n-data-table :columns="Result.Columns" :pagination="PageSize" :data="Result.Data" :bordered="true" :single-line="false" single-column size="small" />
            </n-card>
        </n-gi>
    </n-grid>
</template>
<script setup>
import { ref, defineProps, onMounted } from "vue"
import { useStore } from "vuex";
import { GetDatabaseInfo, GetIcon, ExcelCellName } from "@/zenc/svg/js/Common"
import {DataTableToGridData} from "@/zenc/js/Common"


import svgPanel from "@/zenc/svg/component/svgPanel.vue"

const store = useStore()
const svgPanelCtrl = ref();
const props = defineProps({ Item: { type: Object } })
const dbObj = ref(null);
const Query = ref("");

const Result = ref(new Object);
const PageSize = ref(10);

onMounted(async () => {

    svgPanelCtrl.value.SetPicker("sql");
    var picker = svgPanelCtrl.value.Picker;
    dbObj.value = GetDatabaseInfo(props.Item);
    picker.OnBeforeDrawIcon =async (item)=>{
        
        if(item.IconType == 'table'){
            await BindingColumn(item,ExcelCellName(picker.JoinList.length+1));
        }
    }
    picker.OnAfeterDrawIcon = (item) =>{
        console.log(item)
        console.log(svgPanelCtrl.value.Picker.ObjList)
        if (item.JoinObjs) {
            for (var i in item.JoinObjs)
            {
                var joinObj = item.JoinObjs[i];
                var startObj = joinObj.StartObj;
                for(var j in startObj.Columns){
                    var col = startObj.Columns[j];
                    
                    if(col.obj.foreign_table_name == item.TableName){
                    var isNext = false;
                        if(startObj.JoinObjs){
                            for(var k in startObj.JoinObjs){
                               var sJoin = startObj.JoinObjs[k];
                               if(sJoin.StartJoinColumn == col.obj.column_name || sJoin.EndJoinColumn == col.obj.column_name){
                                isNext = true;
                                break;
                               }
                            }
                        }

                        if(!isNext){
                            joinObj.StartJoinColumn = col.obj.column_name;
                            joinObj.EndJoinColumn = col.obj.foreign_column_name;
                            return;
                        }
                    }
                    
                }

              
            }
        }
        
    }
    var joins = props.Item.JoinObjs;
    if (joins) {
        for (var i in joins) {
            var beforeItem = joins[i].StartObj;
            if (beforeItem.IconType == 'table') {

                var colorObj = new Object;
                colorObj.Stroke = beforeItem.StrokeColor;
                colorObj.Fill = beforeItem.FillColor;
                var obj = GetIcon('table', 100, 80 + (80 * i), 30, colorObj);
                obj.IsShowDisplayName = true;
                obj.DisplayName = obj.TableName = beforeItem.TableName;

                await BindingColumn(obj,ExcelCellName(i));
                

                picker.ObjList.push(obj);

            }
        }

        AutoJoin();
    }

    OnCreateQuery();
})

const BindingColumn = async (obj,alias) =>{
    var param = new Object;
    param.info = dbObj.value;
    param.tableName = obj.TableName;
    var cols = await store.state.apiClient.Run('EEH.DB', 'DbHandler', 'GetColumns', param);
    obj.SubName = obj.Alias = alias;
    obj.Columns = new Array;
    for (var i in cols) {
        obj.Columns.push({ label: cols[i].column_name, value: cols[i].column_name, obj: cols[i] });
    }

}
const AutoJoin = () => {
    var picker = svgPanelCtrl.value.Picker;
    var writeTable = new Array;
    for (var i in picker.ObjList) {
        var table = picker.ObjList[i];
        
        for (var j in table.Columns) {
            var col = table.Columns[j];
            if (col.obj && col.obj.foreign_table_name && col.obj.foreign_column_name) {
                var joinTables = FindTables(col.obj.foreign_table_name);
                
                if (joinTables && joinTables.length > 0) {
                    for (var k in joinTables) {
                        var joinTable = joinTables[k];
                        
                        var isStartUse =UseTable(table,writeTable);
                        var isEndUse =UseTable(joinTable,writeTable);
                        if(isStartUse && isEndUse){
                            continue;
                        }
                        var joinObj = picker.DrawJoin(table, joinTable, null);
                        if (joinObj) {
                            joinObj.StartJoinColumn = col.obj.column_name;
                            joinObj.EndJoinColumn = col.obj.foreign_column_name;
                            break;
                        }
                    }
                }
            }

        }
    }

    function FindTables(tableName) {

        var rtn = new Array;
        for (var i in picker.ObjList) {
            var tbl = picker.ObjList[i];
            
            if (tbl.TableName.toUpperCase() == tableName.toUpperCase()) {
                rtn.push(tbl);
            }
        }
        
        return rtn;
    }
}
const OnBuild =async () =>{
    var param = new Object;
    param.info = dbObj.value;
    param.query = Query.value;
    
    var dt = await store.state.apiClient.Run('EEH.DB', 'DbHandler', 'Build', param);
    Result.value =  DataTableToGridData(dt);
    
}
const OnCreateQuery = () => {
    var picker = svgPanelCtrl.value.Picker;
    var joins = picker.JoinList;

    var query = "SELECT             *  \r\nFROM               ";
    var writeTable = new Array;
    for (var i in joins) {
        var join = joins[i];
        if (join.StartJoinColumn && join.EndJoinColumn) {

            var startTable = join.StartObj;
            var isStartWrite = !UseTable(startTable, writeTable);
            var endTable = join.EndObj;
            var isEndWrite = !UseTable(endTable, writeTable);

            var startTableQuery = "";
            var isLeftStart = isStartWrite && isEndWrite ? true : (!isStartWrite)
            if (isStartWrite) {
                startTableQuery = startTable.TableName.toUpperCase() + ' AS ' + startTable.Alias.toUpperCase();
            }
            var endTableQuery = "";
            if (isEndWrite) {
                endTableQuery = endTable.TableName.toUpperCase() + ' AS ' + endTable.Alias.toUpperCase()
            }

            var joinQuery = "";
            if (join.JoinArrow == "START") {
                if (isLeftStart)
                    joinQuery = '\r\nLEFT OUTER JOIN   '
                else
                    joinQuery = '\r\nRIGHT OUTER JOIN  '
            }
            else if (join.JoinArrow == "END") {
                if (isLeftStart)
                    joinQuery = '\r\nRIGHT OUTER JOIN  '
                else
                    joinQuery = '\r\nLEFT OUTER JOIN   '
            }
            else {
                joinQuery = '\r\nINNER JOIN        '
            }
            if (isStartWrite && isEndWrite) {
                query += startTableQuery + ' ' + joinQuery + ' ' + endTableQuery;
            }
            else if (isStartWrite) {
                query += ' ' + joinQuery + ' ' + startTableQuery;
            }
            else if (isEndWrite) {
                query += ' ' + joinQuery + ' ' + endTableQuery;
            }

            query += ' ON ' + join.StartObj.Alias + "." + join.StartJoinColumn.toUpperCase() + ' = ' + join.EndObj.Alias + "." + join.EndJoinColumn.toUpperCase()
        }
    }
   

    Query.value = query;
}

const UseTable =(table, writeTable) =>{
        for (var i in writeTable) {
            var as = writeTable[i].Alias;
            if (table.Alias == as) {
                return true;
            }
        }
        writeTable.push(table);
        return false;
    }
</script>