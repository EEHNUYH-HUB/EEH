<template>
    <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
        <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
            <Breadcrumb></Breadcrumb>

            <n-grid cols="1 s:1 m:1 l:2 xl:2 2xl:2" :x-gap="12" :y-gap="8" responsive="screen">
                <n-grid-item>
                    <n-card id="id1" title="File Upload" hoverable>

                        <Uploader class="n-button"></Uploader>
                    </n-card>
                </n-grid-item>
                <n-grid-item>
                    <n-card id="id2" title="ExecuteScalar Code" hoverable>
                        <n-code word-wrap code="
                    <Uploader Placement='right' Text='업로드'></Uploader>                    
import Uploader from '@/zenc/layout/components/Uploader.vue'

" language="js" />
                    </n-card>
                </n-grid-item>
            </n-grid>
        </div>
        <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
    </div>
</template>
  
<script setup>
import { ref } from 'vue';
import { useStore } from "vuex";
import Uploader from "@/zenc/layout/components/Uploader.vue"
import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"
import Anchor from "@/zenc/layout/components/Anchor.vue"
const store = useStore()
const IsRunUpload = ref(false)
const AnchorItems = ref(new Array);
AnchorItems.value.push({ Title: 'File Upload', ID: '#id1' });
AnchorItems.value.push({ Title: 'File Upload Code', ID: '#id2' });


</script>
<style scoped>
.n-button {
    float: right;
}

.n-grid {
    margin-top: 24px;
}

.div_root_default {
    display: flex;
    flex-wrap: nowrap;
    padding: 32px 24px 56px 56px;
}

.div_root_mobile {
    display: flex;
    flex-wrap: nowrap;
    padding: 24px;
}

.div_default {
    width: calc(100% - 228px);
    margin-right: 36px;
}

.div_mobile {
    width: calc(100%);
}
</style>