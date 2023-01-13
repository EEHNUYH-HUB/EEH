<template>
    <n-page-header :subtitle="CurrentItem.desc" >
        <template #title>
            {{CurrentItem.name}}
        </template>
        <template #header>
            <n-breadcrumb v-if="!store.state.useIsMobile">
                <n-breadcrumb-item v-for="(item,index) in Items" :key="index">{{item.name}}
                </n-breadcrumb-item>
            </n-breadcrumb>
        </template>
        <template #extra>
                <n-space v-for="item,index in Props.Buttons" :key="index">
                    <NButton @click="item.OnClick" >{{ item.Name }}</NButton>
                </n-space>
            </template>
    </n-page-header>
</template>

<script setup>
import { watch,ref } from 'vue'
import { GetBreadcrumbObjRef } from '@/zenc/router/menus'
import { useStore } from 'vuex'
import { useRoute } from 'vue-router'
import { NButton } from 'naive-ui';
const route = useRoute();
const store = useStore();
const Items = GetBreadcrumbObjRef(route);
const Title = ref("");
const SubTitle = ref("");
const CurrentItem = ref(null);
const Props = defineProps({Buttons:{type:Array}})
CurrentItem.value = Items.value[Items.value.length-1];




watch(()=>route.fullPath, () => {
    Items.value = GetBreadcrumbObjRef(route).value;
    CurrentItem.value= Items.value[Items.value.length-1];
});
</script>
