<template>
     <div  v-if="!store.state.useIsMobile">
        <n-anchor affix :listen-to="DivID"  ignore-gap="true"  :trigger-top="24" :top="88" :bound="24" >
            <n-anchor-link v-for="(item,index) in Props.Items" :key="index" :title="item.Title" :href="item.ID" />
        </n-anchor>
    </div>
</template>
<script setup>
import { useStore } from "vuex";

const Props = defineProps({
    Items: { type: Array },
    DivID:{type:String}
})

const store = useStore()
</script>