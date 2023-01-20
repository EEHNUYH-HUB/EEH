<template>
    <n-card :id="'id'+Props.Item.pk_id" :title="Props.Item.col_name" hoverable>
        <n-p>
            {{ Props.Item.col_address }}
        </n-p>
        <n-p v-if="Props.Item.col_address2">
            {{ Props.Item.col_address2 }}
        </n-p>
        <div :id="Props.Item.pk_id" style="width:calc(100%);height:200px;margin:20px 0px">
            
        </div>
    </n-card>
</template>
<script setup>
import { onMounted } from "vue"

const Props = defineProps({
    Item: { type: Object }
})

onMounted(()=>{

    var position = new naver.maps.LatLng(Props.Item.col_latitude,Props.Item.col_longitude);
    var map = new naver.maps.Map(''+Props.Item.pk_id, {
        center: position,
        zoom: 17
    });

    
    var marker = new naver.maps.Marker({ position:position, map:map});
})
</script>