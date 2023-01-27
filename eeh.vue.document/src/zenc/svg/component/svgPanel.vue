<template>
    <div class="m-0 radios card" id="mainSvg">

        <nav class="card bg-light  top-0 navbar navbar-expand-lg  z-index-3 end-1  position-absolute m-2">
            <button class="shadow-none navbar-toggler " type="button" data-bs-toggle="collapse"
                data-bs-target="#navigation2" aria-controls="navigation2" aria-expanded="false"
                aria-label="Toggle navigation2">
                <span class="mt-2 navbar-toggler-icon">
                    <span class="navbar-toggler-bar bar1"></span>
                    <span class="navbar-toggler-bar bar2"></span>
                    <span class="navbar-toggler-bar bar3"></span>
                </span>
            </button>
            <ul id="navigation2" class="collapse navbar-collapse navbar-nav navbar-nav-hover ">
                <li class="col mb-0 btn  m-1 " @click="SetMode('PEN')"
                    :class="Mode.Name == 'PEN' ? BtnActiveBackgroundColor : BtnBackgroundColor ">
                    <svg width=" 16" height="16" :fill="Mode.Name == 'PEN' ? 'white' : 'currentColor'" viewBox="0 0 16 16">
                        <path
                            d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                    </svg>

                </li>
                <li class="col mb-0 btn  m-1" @click="SetMode('LINE')"
                    :class="Mode.Name == 'LINE' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'LINE' ? 'white' : 'currentColor'" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M2 8a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11A.5.5 0 0 1 2 8Z" />
                    </svg>
                </li>
                <li class="col mb-0 btn  m-1" @click="SetMode('RECT')"
                    :class="Mode.Name == 'RECT' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'RECT' ? 'white' : 'currentColor'" viewBox="0 0 16 16">
                        <path
                            d="M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z" />
                    </svg>
                </li>
                <li class="col mb-0 btn   m-1" @click="SetMode('CIRCLE')"
                    :class="Mode.Name == 'CIRCLE' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'CIRCLE' ? 'white' : 'currentColor'"
                        viewBox="0 0 16 16">
                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                    </svg>
                </li>
                <li class="col mb-0 btn  m-1" @click="SetMode('TRIANGLE')"
                    :class="Mode.Name == 'TRIANGLE' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'TRIANGLE' ? 'white' : 'currentColor'"
                        viewBox="0 0 16 16">
                        <path
                            d="M7.938 2.016A.13.13 0 0 1 8.002 2a.13.13 0 0 1 .063.016.146.146 0 0 1 .054.057l6.857 11.667c.036.06.035.124.002.183a.163.163 0 0 1-.054.06.116.116 0 0 1-.066.017H1.146a.115.115 0 0 1-.066-.017.163.163 0 0 1-.054-.06.176.176 0 0 1 .002-.183L7.884 2.073a.147.147 0 0 1 .054-.057zm1.044-.45a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566z" />
                    </svg>
                </li>
                <li class="col mb-0 btn  m-1" @click="SetMode('POLYGON')"
                    :class="Mode.Name == 'POLYGON' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'POLYGON' ? 'white' : 'currentColor'"
                        viewBox="0 0 16 16">
                        <path
                            d="M7.779.052a.5.5 0 0 1 .442 0l6.015 2.97a.5.5 0 0 1 .267.34l1.485 6.676a.5.5 0 0 1-.093.415l-4.162 5.354a.5.5 0 0 1-.395.193H4.662a.5.5 0 0 1-.395-.193L.105 10.453a.5.5 0 0 1-.093-.415l1.485-6.676a.5.5 0 0 1 .267-.34L7.779.053zM2.422 3.813l-1.383 6.212L4.907 15h6.186l3.868-4.975-1.383-6.212L8 1.058 2.422 3.813z" />
                    </svg>
                </li>
                <li class="col mb-0 btn  m-1" @click="SetMode('TEXT')"
                    :class="Mode.Name == 'TEXT' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'TEXT' ? 'white' : 'currentColor'" viewBox="0 0 16 16">
                        <path
                            d="M12.258 3h-8.51l-.083 2.46h.479c.26-1.544.758-1.783 2.693-1.845l.424-.013v7.827c0 .663-.144.82-1.3.923v.52h4.082v-.52c-1.162-.103-1.306-.26-1.306-.923V3.602l.431.013c1.934.062 2.434.301 2.693 1.846h.479L12.258 3z" />
                    </svg>
                </li>
                <li class="col mb-0 btn  m-1" @click="SetMode('PICK')"
                    :class="Mode.Name == 'PICK' ? BtnActiveBackgroundColor : BtnBackgroundColor">
                    <svg width="16" height="16" :fill="Mode.Name == 'PICK' ? 'white' : 'currentColor'" viewBox="0 0 16 16">
                        <path fill-rule="evenodd"
                            d="M7.646.146a.5.5 0 0 1 .708 0l2 2a.5.5 0 0 1-.708.708L8.5 1.707V5.5a.5.5 0 0 1-1 0V1.707L6.354 2.854a.5.5 0 1 1-.708-.708l2-2zM8 10a.5.5 0 0 1 .5.5v3.793l1.146-1.147a.5.5 0 0 1 .708.708l-2 2a.5.5 0 0 1-.708 0l-2-2a.5.5 0 0 1 .708-.708L7.5 14.293V10.5A.5.5 0 0 1 8 10zM.146 8.354a.5.5 0 0 1 0-.708l2-2a.5.5 0 1 1 .708.708L1.707 7.5H5.5a.5.5 0 0 1 0 1H1.707l1.147 1.146a.5.5 0 0 1-.708.708l-2-2zM10 8a.5.5 0 0 1 .5-.5h3.793l-1.147-1.146a.5.5 0 0 1 .708-.708l2 2a.5.5 0 0 1 0 .708l-2 2a.5.5 0 0 1-.708-.708L14.293 8.5H10.5A.5.5 0 0 1 10 8z" />
                    </svg>
                </li>
                <li>
                    <div class="m-4"></div>
                </li>
            </ul>


        </nav>


        <svg class=" mainsvg " @mousemove="OnMouseMove" @mouseup="OnMouseUp" @mouseout="OnMouseOut"
            @contextmenu="OnContextMenu" @mousedown="OnMouseDown">

            <g v-for="item, index in SvgList" :key="index" @mouseenter="OnMouseEnter(item)"
                @mouseleave="OnMouseLeave(item)"
                :transform="'translate(' + item.Rect.X + ',' + item.Rect.Y + ') scale(' + item.Rect.ScaleX + ',' + item.Rect.ScaleY + ') rotate(' + item.Rect.Rotate + ',' + item.Rect.Width / 2 + ',' + item.Rect.Height / 2 + ')'">
                <rect v-if="item.Type == 'RECT'" :width="item.Rect.Width" :height="item.Rect.Height"
                    :fill="item.FillColor" :stroke="item.StrokeColor">
                </rect>
                <ellipse v-else-if="item.Type == 'CIRCLE'" :cx="item.Rect.Width / 2" :cy="item.Rect.Height / 2"
                    :rx="item.Rect.Width / 2" :ry="item.Rect.Height / 2" :fill="item.FillColor"
                    :stroke="item.StrokeColor"></ellipse>
                <polygon v-else-if="item.Type == 'POLYGON' || item.Type == 'TRIANGLE'" :points="item.Ps"
                    :fill="item.FillColor" :stroke="item.StrokeColor"></polygon>
                <path v-else-if="item.Type == 'PEN'" :d="item.Path" :fill="item.FillColor" :stroke="item.StrokeColor" />
                <line v-else-if="item.Type == 'LINE'" :x1="item.X1" :y1="item.Y1" :x2="item.X2" :y2="item.Y2"
                    :fill="item.FillColor" :stroke="item.StrokeColor"></line>

                <text v-else-if="item.Type == 'TEXT'" x="12" y="26" :fill="item.FillColor"
                    :stroke="item.StrokeColor">{{ item.Text }}</text>
                <g v-if="Mode.Name == 'PICK' && item.IsSelected">
                    <rect :width="item.Rect.Width" opacity="0.5" :height="item.Rect.Height" stroke="#96C6DE"
                        fill="none"></rect>

                    <g v-for="sub, i in  item.Rect.SubObjs" :key="i">
                        <circle :cx="sub.cX" :cy="sub.cY" :r="sub.R" :stroke="sub.S" stroke-width="0.5" cur
                            @mousedown="sub.IsDown = true" :fill="sub.IsDown ? '#186083' : sub.F">
                        </circle>
                    </g>
                </g>

            </g>


            <path v-if="Mode.Name != 'PICK'" :d="Mode.DrawItem" :fill="DefaultFill" :stroke="DefaultStroke"
                opacity="0.5" />
            <path v-else :d="Mode.DrawItem" :fill="Mode.IsDrawJoin ? 'none' : '#2391C9'" stroke="#38474E" opacity="0.3" />

            <path v-for="item, index in JoinList" :key="index" :d="item.Path" :fill="item.FillColor" stroke-width="0.5"
                marker-end="url(#Triangle)" marker-start="url(#Circle)" :stroke="item.StrokeColor" />

            <defs>
                <marker id="Triangle" viewBox="0 0 10 10" refX="5" refY="5" markerUnits="strokeWidth" markerWidth="12"
                    markerHeight="12" orient="auto">
                    <path d="M 0 0 L 10 5 L 0 10 z" />
                </marker>
                <marker id="Circle" viewBox="0 0 10 10" refX="5" refY="5" markerUnits="strokeWidth" markerWidth="12"
                    markerHeight="12" orient="auto">
                    <circle cx = "6" cy = "6" r = "5"/>
                </marker>
            </defs>
        </svg>

        <input type="text" v-if="Mode.Name == 'TEXT' && Mode.IsDown" @keydown="OnKeyUp"
            class="md-textarea form-control textarea js-autoresize" v-model="Mode.Text" style="position:absolute;"
            :style="'left:' + Mode.X + 'px;top:' + Mode.Y + 'px;height:' + Mode.H + 'px;width:' + Mode.W + 'px'" />

    </div>

</template>
<script setup>

import { ref, defineProps, onMounted, defineExpose, watch } from 'vue'
import MNDrawPen from '@/zenc/svg/js/MNDrawPen'
import MNDrawLine from '@/zenc/svg/js/MNDrawLine'
import MNDrawRect from '@/zenc/svg/js/MNDrawRect'
import MNDrawCircle from '@/zenc/svg/js/MNDrawCircle'
import MNDrawTriangle from '@/zenc/svg/js/MNDrawTriangle'
import MNDrawPolygon from '@/zenc/svg/js/MNDrawPolygon'
import MNDrawText from '@/zenc/svg/js/MNDrawText'
import MNDrawPicker from '@/zenc/svg/js/MNDrawPicker'

import { GetOffsetPoint } from "@/zenc/js/Common"


const COLOROBJ = ref(Object);

var obj = new Object;
obj.Stroke = "#000000FF";
obj.Fill = "#FFFFFF00";
COLOROBJ.value = obj;

const DefaultStroke = ref('#000000FF');
const DefaultFill = ref('#FFFFFF00');
const SvgList = ref(null);
const JoinList = ref(new Array);
const BoforeMode = ref("");
const Mode = ref(new MNDrawPen);

const IsDown = ref(false);
const BtnActiveBackgroundColor = ref("bg-primary");
const BtnBackgroundColor = ref("bg-white");
watch([DefaultStroke, DefaultFill], (newValue, oldValue) => {
    // do Something
    COLOROBJ.value.Stroke = newValue[0];
    COLOROBJ.value.Fill = newValue[1];

    if (Mode.value && Mode.value.ChangedColor) {
        Mode.value.ChangedColor(COLOROBJ.value);
    }
})
onMounted(() => {

    window.onkeypress = (e) => {

        // if (Mode.value.Name == "TEXT")
        //     Mode.value.MouseDown(e)
        //console.log(e.key)
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
    if (Mode.value && Mode.value.MouseUp)
        Mode.value.MouseUp(GetOffsetPoint(e));
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
SetMode("PEN");

defineExpose({
    RemoteAddObj, RemoteRemoveObj, RemoteSelectObj, RemoteChangeObj, RemoteUnSelectObj
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
    height: 600px;
    background-color: gainsboro;
}
</style>