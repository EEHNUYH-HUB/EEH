<template>
    <svg class="mainsvg" @mousemove="OnMouseMove" @mouseup="OnMouseUp" @mouseout="OnMouseOut"
        @contextmenu="OnContextMenu" @mousedown="OnMouseDown">
        <g v-for="item, index in SvgList" :key="index" @mouseenter="OnMouseEnter(item)" @mouseleave="OnMouseLeave(item)"
            :transform="'translate(' + item.Rect.X + ',' + item.Rect.Y + ') scale(' + item.Rect.ScaleX + ',' + item.Rect.ScaleY + ') rotate(' + item.Rect.Rotate + ',' + item.Rect.Width / 2 + ',' + item.Rect.Height / 2 + ')'">
      

            <rect v-if="item.Type == 'RECT'" :width="item.Rect.Width" :height="item.Rect.Height" :fill="item.FillColor"
                :stroke="item.StrokeColor"></rect>
            <ellipse v-else-if="item.Type == 'CIRCLE'" :cx="item.Rect.Width / 2" :cy="item.Rect.Height / 2"
                :rx="item.Rect.Width / 2" :ry="item.Rect.Height / 2" :fill="item.FillColor" :stroke="item.StrokeColor">
            </ellipse>
            <polygon v-else-if="item.Type == 'POLYGON' || item.Type == 'TRIANGLE'" :points="item.Ps"
                :fill="item.FillColor" :stroke="item.StrokeColor"></polygon>
            <path v-else-if="item.Type == 'PEN'" :d="item.Path" :fill="item.FillColor" :stroke="item.StrokeColor" />
            <line v-else-if="item.Type == 'LINE'" :x1="item.X1" :y1="item.Y1" :x2="item.X2" :y2="item.Y2"
                :fill="item.FillColor" :stroke="item.StrokeColor"></line>
            <text v-else-if="item.Type == 'TEXT'" x="12" y="26" :fill="item.FillColor" :stroke="item.StrokeColor">{{
                item.Text
            }}</text>
            <iconFactory v-else-if="item.Type == 'ICON'" :Item="item"></iconFactory>
            <itemFactory v-else-if="item.Type == 'ITEM'" :Item = item></itemFactory>
            <g v-if="Mode.Name == 'PICK' && item.IsSelected">
                <rect :width="item.Rect.Width" opacity="0.5" :height="item.Rect.Height" stroke="#96C6DE" fill="none">
                </rect>
                <g v-for="sub, i in  item.Rect.SubObjs" :key="i">
                    <circle :cx="sub.cX" :cy="sub.cY" :r="sub.R" :stroke="sub.S" stroke-width="0.5" cur
                        @mousedown="sub.IsDown = true" :fill="sub.IsDown ? '#186083' : sub.F">
                    </circle>
                </g>
            </g>
            
        </g>
        <path v-if="Mode.Name != 'PICK'" :d="Mode.DrawItem" :fill="DefaultFill" :stroke="DefaultStroke" opacity="0.5" />
        <path v-else :d="Mode.DrawItem" :fill="Mode.IsDrawJoin ? 'none' : '#2391C9'" stroke="#38474E" opacity="0.3" />

        <path v-for="item, index in JoinList" :key="index" :d="item.Path" :fill="item.FillColor" stroke-width="0.5"
            marker-end="url(#Triangle)"  marker-start="url(#Circle)" :stroke="item.StrokeColor" />

        <defs>
            <marker id="Triangle" viewBox="0 0 10 10" refX="6" refY="6" markerUnits="strokeWidth" markerWidth="12"
                markerHeight="12" orient="auto">
                <path d="M 0 0 L 10 5 L 0 10 z"  />
            </marker>
            <marker id="Circle" viewBox="0 0 10 10" refX="5" refY="5" markerUnits="strokeWidth" markerWidth="12"
                markerHeight="12" orient="auto">
                <circle cx="6" cy="6" r="2" />
            </marker>
        </defs>
        

       
    </svg>

    <input type="text" v-if="Mode.Name == 'TEXT' && Mode.IsDown" @keydown="OnKeyUp"
        class="md-textarea form-control textarea js-autoresize" v-model="Mode.Text" style="position:absolute;"
        :style="'left:' + Mode.X + 'px;top:' + Mode.Y + 'px;height:' + Mode.H + 'px;width:' + Mode.W + 'px'" />

    <n-popover :show="PopoverObj.IsShow" :x="PopoverObj.X" :y="PopoverObj.Y" trigger="manual" :show-arrow="true"  >

        <n-grid cols="5" :x-gap="12" :y-gap="8" responsive="screen">
            <n-gi v-for="item, index in Icons" :key="index">
                <n-button @click="OnClickIcon(item.IconType)">
                    <iconFactory :Item="item"></iconFactory>
                </n-button>
            </n-gi>


        </n-grid>
    </n-popover>
    
    <n-color-picker v-model:value="COLOROBJ.Stroke" />
    <n-color-picker v-model:value="COLOROBJ.Fill" />

</template>
<script setup>

import { ref, defineProps, onMounted, defineExpose, watch,defineEmits } from 'vue'
import MNDrawPen from '@/zenc/svg/js/MNDrawPen'
import MNDrawLine from '@/zenc/svg/js/MNDrawLine'
import MNDrawRect from '@/zenc/svg/js/MNDrawRect'
import MNDrawCircle from '@/zenc/svg/js/MNDrawCircle'
import MNDrawTriangle from '@/zenc/svg/js/MNDrawTriangle'
import MNDrawPolygon from '@/zenc/svg/js/MNDrawPolygon'
import MNDrawText from '@/zenc/svg/js/MNDrawText'
import MNDrawPicker from '@/zenc/svg/js/MNDrawPicker'

import { GetIcons,GetOffsetPoint } from '@/zenc/svg/js/Common'

import iconFactory from "@/zenc/svg/component/iconFactory.vue"
import itemFactory from "@/zenc/svg/component/itemFactory.vue"

const emits = defineEmits(["selectedItem"])


const SvgList = ref(new Array);
const JoinList = ref(new Array);
const PopoverObj = ref({ IsShow: false, X: 0, Y: 0 })
const COLOROBJ = ref(Object);

var obj = new Object;
obj.Stroke = "#000000FF";
obj.Fill = "#FFFFFF00";
COLOROBJ.value = obj;

const DefaultStroke = ref('#000000FF');
const DefaultFill = ref('#FFFFFF00');

const BoforeMode = ref("");
const Mode = ref(new MNDrawPen);

const IsDown = ref(false);
const BtnActiveBackgroundColor = ref("bg-primary");
const BtnBackgroundColor = ref("bg-white");

const Icons = ref(['chat', 'circle', 'database', 'diagram', 'diamond', 'email', 'fonts', 'listTask', 'pc', 'server', 'square', 'table', 'triangle'])


const ShowPopoverPoint = ref(null);
watch([DefaultStroke, DefaultFill], (newValue, oldValue) => {
    // do Something
    COLOROBJ.value.Stroke = newValue[0];
    COLOROBJ.value.Fill = newValue[1];

    if (Mode.value && Mode.value.ChangedColor) {
        Mode.value.ChangedColor(COLOROBJ.value);
    }
})
onMounted(() => {
    Icons.value = GetIcons(COLOROBJ.value);
    window.onkeypress = (e) => {

    }
    window.onkeyup = (e) => {

        if (e.key == "Control") {
            SetMode(BoforeMode.value);
        }
        else if (Mode.value && Mode.value.Name == "PICK") {
            if (e.key == "Escape") {
                if (Mode.value.AllUnSelected) {
                    Mode.value.AllUnSelected();
                }
            }
            else if (e.code == "Delete") {
                if (Mode.value.DeleteSvgObj) {
                    Mode.value.DeleteSvgObj();
                }
            }
        }

    }
    window.onkeydown = (e) => {
        if (e.key == "Control" && Mode.value && Mode.value.Name != "PICK") {
            BoforeMode.value = Mode.value.Name;
            SetMode("PICK")

        }
    }
})

const OnMouseMove = (e) => {

    if (Mode.value && Mode.value.MouseMove)
        Mode.value.MouseMove(GetOffsetPoint(e));

}
const OnMouseUp = (e) => {
    if (Mode.value && Mode.value.MouseUp) {
        
        PopoverObj.value.IsShow = false;
        Mode.value.MouseUp(GetOffsetPoint(e));
        if (Mode.value.Name == "PICK") {
            var isRight = false;
            if ("which" in e)
                isRight = e.which == 3;
            else if ("button" in e)
                isRight = e.button == 2;


            if (Mode.value.IsShowStandby) {
                OnShowDetail();
                
            }
            else {
                if (isRight || Mode.value.IsShowIcon) {
                    ShowPopoverIcon(e);
                }
            }

        }

    }

}

const OnClickIcon = (type) => {
    if (Mode.value && Mode.value.DrawIcon) {
        Mode.value.DrawIcon(type, ShowPopoverPoint.value, 32);
    }

    PopoverObj.value.IsShow = false;
}
const ShowPopoverIcon = (e) => {
    ShowPopoverPoint.value = GetOffsetPoint(e);
    PopoverObj.value.IsShow = !PopoverObj.value.IsShow;
    if (PopoverObj.value.IsShow) {
        PopoverObj.value.X = e.clientX;
        PopoverObj.value.Y = e.clientY;
    }

}

const OnMouseOut = (e) => {
}
const OnMouseDown = (e) => {

    if (Mode.value && Mode.value.MouseDown) {
        Mode.value.MouseDown(GetOffsetPoint(e));
    }
}
const OnKeyUp = (e) => {
    if (Mode.value && Mode.value.KeyUp)
        Mode.value.KeyUp(e);
}
const OnContextMenu = (e) => {
    e.preventDefault();
}
const OnMouseEnter = (item) => {
    if (Mode.value && Mode.value.Name == "PICK") {
        Mode.value.JoinObj(item);
    }
}
const OnMouseLeave = (item) => {
    if (Mode.value && Mode.value.Name == "PICK") {
        Mode.value.UnJoinObj(item);
    }
}

const RemoteAddObj = (obj) => {
    if (!SvgList.value)
        SvgList.value = new Array;

    SvgList.value.push(obj);
}
const RemoteRemoveObj = (obj) => {

    for (var i in SvgList.value) {
        if (SvgList.value[i].ID == obj.ID) {
            SvgList.value.splice(i, 1);

            break;
        }
    }
}
const RemoteSelectObj = (obj) => {
    for (var i in SvgList.value) {
        if (SvgList.value[i].ID == obj.ID) {
            SvgList.value[i].IsRemoteSelected = true;

            break;
        }
    }

}
const RemoteUnSelectObj = () => {
    for (var i in SvgList.value) {
        SvgList.value[i].IsRemoteSelected = false;
    }

}
const RemoteChangeObj = (obj) => {
    for (var i in SvgList.value) {
        if (SvgList.value[i].ID == obj.ID) {
            SvgList.value[i] = obj;

            break;
        }
    }
}
const Eventer = ref(null);
Eventer.value = new Object;
Eventer.value.AddedMethod = function (obj) {
    //Props.OnClassInstance.SendObj("ADD",[obj]);
}
Eventer.value.ChangedMethod = function (objs) {
    //Props.OnClassInstance.SendObj("CHANGE", objs);
}
Eventer.value.RemovedMethod = function (objs) {
    //Props.OnClassInstance.SendObj("REMOVE", objs);
}
Eventer.value.SelectedMethod = function (objs) {
    //Props.OnClassInstance.SendObj("SELECT", objs);
}
const OnShowDetail = () => {
    if (Mode.value && Mode.value.SelectedItems.length > 0) {
        var item = Mode.value.SelectedItems[0]

        emits("selectedItem", item)
        Mode.value.IsShowStandby = false;
    }
}
const SetMode = (type) => {
    if (!SvgList.value)
        SvgList.value = new Array;

    if (Mode.value && Mode.value.Name == "PICK") {
        Mode.value.AllUnSelected();
    }

    if (type == "PEN") {
        Mode.value = new MNDrawPen(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "LINE") {
        Mode.value = new MNDrawLine(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "RECT") {
        Mode.value = new MNDrawRect(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "CIRCLE") {
        Mode.value = new MNDrawCircle(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "TRIANGLE") {
        Mode.value = new MNDrawTriangle(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "POLYGON") {
        Mode.value = new MNDrawPolygon(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "TEXT") {
        Mode.value = new MNDrawText(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
    else if (type == "PICK") {
        Mode.value = new MNDrawPicker(SvgList.value, JoinList.value, COLOROBJ.value, Eventer.value);
    }
}
SetMode("PICK");

defineExpose({
    RemoteAddObj, RemoteRemoveObj, RemoteSelectObj, RemoteChangeObj, RemoteUnSelectObj,SvgList,JoinList
});


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
    height: calc(100vh - 180px);
    /* background-color: gainsboro; */
}
</style>