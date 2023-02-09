<template>
    <n-scrollbar :style="'max-height:'+ props.Height" trigger="none">
    <svg class="mainsvg" :height="props.SvgHeight" @mousemove="OnMouseMove" @mouseup="OnMouseUp" @mouseout="OnMouseOut"
        @contextmenu="OnContextMenu" @mousedown="OnMouseDown">
        <g v-for="item, index in Picker.ObjList" :key="index" @mouseenter="OnMouseEnter(item)" @mouseleave="OnMouseLeave(item)"
            :transform="'translate(' + item.Rect.X + ',' + item.Rect.Y + ') scale(' + item.Rect.ScaleX + ',' + item.Rect.ScaleY + ') rotate(' + item.Rect.Rotate + ',' + item.Rect.Width / 2 + ',' + item.Rect.Height / 2 + ')'">
            <iconFactory v-if="item.Type == 'ICON'" :Item="item"></iconFactory>            
            <pickerSvg :Item="item"></pickerSvg>

        </g>
        
        <path :d="Picker.DrawItem" fill="none" stroke="#38474E" opacity="0.3" />
        <joinFactory v-for="item, index in Picker.JoinList" :key="index" :Item="item"></joinFactory>
        <markerList></markerList>
    </svg>


    <iconsPop ref="iconSelecter" :ColorObj="Picker.ColorObj" @OnSelectedIcon="OnSelectedIcon"></iconsPop>
    
</n-scrollbar>
</template>
<script setup>

import { ref, defineProps, onMounted, defineExpose, defineEmits } from 'vue'

import DrawPicker from '@/zenc/svg/js/DrawPicker'
import DrawSqlPicker from '@/zenc/svg/js/DrawSqlPicker'

import { GetOffsetPoint } from '@/zenc/svg/js/Common'

import iconFactory from "@/zenc/svg/component/iconFactory.vue"
import joinFactory from "@/zenc/svg/component/joinFactory.vue"
import markerList from "@/zenc/svg/component/common/markerList.vue"
import pickerSvg from "@/zenc/svg/component/common/pickerSvg.vue"
import iconsPop from "@/zenc/svg/component/common/iconsPop.vue"

const emits = defineEmits(["selectedItem"])
const props = defineProps({ Height: {type:String,default:"calc(100vh - 180px)"},SvgHeight: {type:String,default:"calc(100vh - 180px)"}})
const iconSelecter = ref();
const DefaultStroke = ref('#000000FF');
const DefaultFill = ref('#FFFFFF00');

const Picker = ref(new DrawPicker());



onMounted(() => {

    window.onkeyup = (e) => {

        if (Picker.value ) {
            if (e.key == "Escape") {
                if (Picker.value.AllUnSelected) {
                    Picker.value.AllUnSelected();
                }
            }
            else if (e.code == "Delete") {
                if (Picker.value.DeleteSvgObj) {
                    Picker.value.DeleteSvgObj();
                }
            }
        }

    }
})

const OnMouseMove = (e) => {

    if (Picker.value && Picker.value.MouseMove)
        Picker.value.MouseMove(e);

}
const OnMouseUp = (e) => {
    if (Picker.value && Picker.value.MouseUp) {
        iconSelecter.value.Show(false, 0, 0, null);

        Picker.value.MouseUp(e);
       
        if (Picker.value.IsShowStandby) {
            OnShowDetail();
        }
        else  if ( Picker.value.IsShowIcon) {
            ShowPopoverIcon(e);
        }


    }

}

const OnSelectedIcon = (type) => {
    if (Picker.value && Picker.value.DrawIcon) {
        Picker.value.DrawIcon(type, 32);
    }
}
const ShowPopoverIcon = (e) => {
    iconSelecter.value.Show(true, e.clientX, e.clientY, Picker.value.ChangeObj);
}

const OnMouseOut = (e) => {
    if (Picker.value && Picker.value.MouseOut) {
        Picker.value.MouseOut(e);
    }
}
const OnMouseDown = (e) => {

    if (Picker.value && Picker.value.MouseDown) {
        Picker.value.MouseDown(e);
    }
}
const OnKeyUp = (e) => {
    if (Picker.value && Picker.value.KeyUp)
        Picker.value.KeyUp(e);
}
const OnContextMenu = (e) => {
    e.preventDefault();
}
const OnMouseEnter = (item) => {
    if (Picker.value ) {
        Picker.value.JoinObj(item);
    }
}
const OnMouseLeave = (item) => {
    if (Picker.value ) {
        Picker.value.UnJoinObj(item);
    }
}


const SetPicker = (type) => {
    if (type == "" || type == "Defaul")
        Picker.value = new DrawPicker();
    else if (type == "sql")
        Picker.value = new DrawSqlPicker();
}

const OnShowDetail = () => {
    if (Picker.value && Picker.value.SelectedItems.length > 0) {
        var item = Picker.value.SelectedItems[0]

        emits("selectedItem", item)
        Picker.value.IsShowStandby = false;
    }
}


defineExpose({ SetPicker ,Picker});


</script>
<style scoped>
div {
    -ms-user-select: none;
    -moz-user-select: -moz-none;
    -khtml-user-select: none;
    -webkit-user-select: none;
    user-select: none;
}

.mainsvg {
    width: 100%;
    /* height: calc(100vh - 180px); */
    /* background-color: gainsboro; */
}
</style>
