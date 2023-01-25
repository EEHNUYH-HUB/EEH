<template>
    <div style="display:flex;position:relative;width:100%; overflow:hidden;" 
    :style="{ height: ContainerHeight +'px'}" >
        
        <svg @mousemove="OnMouseMove" @mouseup="OnMouseUp"
                 @mouseout="OnMouseOut2" background-color="red" stroke="red" stroke-width="1" 
                 @mousedown="OnMouseDown"
                 zoomAndPan="magnify" :width="MainWidth" :height="MainHeight"
                 preserveAspectRatio="none" alignment-baseline="alphabetic"
                 :transform="MainPosition">
                 
            <path :d="Path" stroke="#eb6e4c" fill="none" stroke-width="1"></path>

            <path v-for="item,index in PathList" :key="index" :d="item" stroke="#eb6e4c" fill="none" stroke-width="2"></path>

            
            
            <text x="30" y="90" stroke-width="0" fill="blue" font-size="21" > evening </text>
        </svg>
        

        
    </div>
</template>
<script setup>
import {ref} from 'vue'

const ContainerHeight = ref(800);
const MainWidth =ref(1500)
const MainHeight = ref(800)
const MainPosition = ref("translate(0, 0)")

const PathList = ref(new Array)
const IsMouseDown =ref (false)
const StartPoint = ref(null);
const Path = ref("M10 80 C 40 10, 65 10, 95 80 S 150 150, 180 80");

const GetRect = (startPoint,endPoint)=>{
    var rect = new Object;
    rect.minX = -1;
    rect.minY = -1;
    rect.maxX = -1;
    rect.maxY = -1;
    rect.w = -1;
    rect.h = -1;
    rect.cX = -1;
    rect.cY = -1;
    rect.sX = startPoint.X;
    rect.sY = startPoint.Y;
    rect.eX = endPoint.X;
    rect.eY = endPoint.Y;
    rect.lineFlow = "";
    if (startPoint.X == endPoint.X) {
        rect.minX = rect.maxX = startPoint.X;
        if(startPoint.Y == endPoint.Y){
            rect.minY = rect.maxY  =startPoint.Y;
        }
        else if (startPoint.Y > endPoint.Y) {
            rect.lineFlow +="BT";
            rect.minY = endPoint.Y;
            rect.maxY = startPoint.Y;
        }
        else {
            rect.lineFlow +="TB";
            rect.minY = startPoint.Y;
            rect.maxY = endPoint.Y;
        }
    }
    else if (startPoint.X < endPoint.X) {
        
        rect.minX = startPoint.X;
        rect.maxX = endPoint.X;
        rect.lineFlow = "LR";
        if(startPoint.Y == endPoint.Y){
            rect.minY = rect.maxY  =startPoint.Y;
        }
        else if (startPoint.Y > endPoint.Y) {
            rect.lineFlow +="BT";
            rect.minY = endPoint.Y;
            rect.maxY = startPoint.Y;
        }
        else {
            rect.lineFlow +="TB";
            rect.minY = startPoint.Y;
            rect.maxY = endPoint.Y;
        }
    }
    else {
        
        rect.minX = endPoint.X;
        rect.maxX = startPoint.X;
        rect.lineFlow = "RL";
        if(startPoint.Y == endPoint.Y){
            rect.minY = rect.maxY  =startPoint.Y;
        }
        else if (startPoint.Y > endPoint.Y) {
            rect.lineFlow +="BT";
            rect.minY = endPoint.Y;
            rect.maxY = startPoint.Y;
        }
        else {
            rect.lineFlow +="TB";
            rect.minY = startPoint.Y;
            rect.maxY = endPoint.Y;
        }
    }

    rect.w = rect.maxX - rect.minX;
    rect.h = rect.maxY - rect.minY;
    rect.cX = rect.minX + rect.w /2;
    rect.cY = rect.minY + rect.h /2;
    return rect;
}
const DrawPath = (startPoint,endPoint) =>{
    var rect  = GetRect(startPoint,endPoint);
    var sub = 2
    var w = rect.w /sub;
    var h = rect.h /sub;
    var arry = new Array;
    arry.push({tag:'M',x:rect.sX,y:rect.sY });
    if(rect.lineFlow == 'LRTB'){
        arry.push({tag:'C',x:rect.sX + w ,y:rect.sY});
        arry.push({tag:',',x:rect.eX  - w  ,y:rect.eY  });
        
    } else if(rect.lineFlow == 'RLTB'){
        arry.push({tag:'C',x:rect.sX - w ,y:rect.sY});
        arry.push({tag:',',x:rect.eX + w  ,y:rect.eY  });
    } else if(rect.lineFlow == 'LRBT'){
        arry.push({tag:'C',x:rect.sX + w ,y:rect.sY});
        arry.push({tag:',',x:rect.eX - w  ,y:rect.eY  });
    } else if(rect.lineFlow == 'RLBT'){
        arry.push({tag:'C',x:rect.sX - w ,y:rect.sY});
        arry.push({tag:',',x:rect.eX + w  ,y:rect.eY  });
    }
    arry.push({tag:',',x:rect.eX,y:rect.eY });

    var str = "";
    for(var i in arry){
        var t = arry[i];
        str+=" "+t.tag+" "+t.x+" "+t.y;
    }

    console.log(Path.value)
    
    Path.value = str;
}
const OnMouseDown =(e) =>{
    IsMouseDown.value = true;
    StartPoint.value = new Object;
    StartPoint.value.X = e.offsetX;
    StartPoint.value.Y = e.offsetY;

}

const OnMouseMove =(e) =>{
    if(IsMouseDown.value){
        var endPoint = new Object;
        endPoint.X = e.offsetX;
        endPoint.Y = e.offsetY;

        DrawPath(StartPoint.value,endPoint);
    }

}
const OnMouseUp =(e) =>{
    IsMouseDown.value = false
    StartPoint.value = null;
    PathList.value.push(Path.value)
    
}
const OnMouseOut2 =(e) =>{
    
}
</script>