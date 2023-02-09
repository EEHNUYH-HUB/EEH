<template>
    <n-grid cols="1" :x-gap="12" :y-gap="8" responsive="screen">
        <n-gi>
            <n-card title="다이어그램" size="small">
                <svgPanel ref="svgPanelCtrl" Height="500px" SvgHeight="1000px"></svgPanel>
            </n-card>
        </n-gi>
        <n-gi>
            <n-card title="쿼리" size="small">
                <n-code word-wrap code="
                SELECT * FROM
" language="sql" />
                    
            </n-card>
        </n-gi>
        <n-gi>
            <n-card title="결과" size="small">
            </n-card>
        </n-gi>
    </n-grid>
</template>
<script setup>
import { ref, defineProps, onMounted } from "vue"
import { useStore } from "vuex";
import { GetDatabaseInfo, GetIcon } from "@/zenc/svg/js/Common"
import svgPanel from "@/zenc/svg/component/svgPanel.vue"
import DrawPicker from '@/zenc/svg/js/DrawPicker'
import DrawSqlPicker from '@/zenc/svg/js/DrawSqlPicker'
const store = useStore()
const svgPanelCtrl = ref();
const props = defineProps({ Item: { type: Object } })
const dbObj = ref(null);

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

                obj.Columns = new Array;
                for (var i in cols) {
                    obj.Columns.push({ label: cols[i], value: cols[i] });
                }


                picker.ObjList.push(obj);

            }
        }
    }



})
</script>