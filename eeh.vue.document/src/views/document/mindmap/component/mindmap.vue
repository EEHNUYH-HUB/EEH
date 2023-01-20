<template>
    <div style="display:flex;position:relative;width:100%; overflow:hidden;" :style="{ height: ContainerHeight +'px'}" >
        <div :style="{ width: MainWidth +'px' ,height:MainHeight+'px'}">
        <svg @mousemove="OnMouseMove" @mouseup="OnMouseUp"
                 @mouseout="OnMouseOut2" color="gray" backgro
                 @mousedown="OnMouseDown(null)"
                 zoomAndPan="magnify" :width="MainWidth" :height="MainHeight"
                 preserveAspectRatio="none" alignment-baseline="alphabetic"
                 :transform="MainPosition">
                 
                 <svg  width="20" height="20" y="6"
                          class="bi bi-file-earmark" viewBox="0 0 16 16">
                        <path d="M4.879 4.515a.5.5 0 0 1 .606.364l1.036 4.144.997-3.655a.5.5 0 0 1 .964 0l.997 3.655 1.036-4.144a.5.5 0 0 1 .97.242l-1.5 6a.5.5 0 0 1-.967.01L8 7.402l-1.018 3.73a.5.5 0 0 1-.967-.01l-1.5-6a.5.5 0 0 1 .364-.606z" />
                        <path d="M4 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H4zm0 1h8a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1z" />

                    </svg>
        </svg>
        </div>
    </div>
</template>
<script setup>
import {ref} from 'vue'

const ContainerHeight = ref(2100);
const MainWidth =ref(1500)
const MainHeight = ref(1000)
const MainPosition = ref("translate(0, 0)")
const OnMouseMove =(e) =>{console.log(e)}
const OnMouseUp =(e) =>{console.log(e)}
const OnMouseOut2 =(e) =>{console.log(e)}
const OnMouseDown =(e) =>{
    console.log(e)

}
</script>