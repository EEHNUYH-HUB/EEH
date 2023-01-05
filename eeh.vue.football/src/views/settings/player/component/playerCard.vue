<template>
    <n-card hoverable>
        
        <n-space   align="center">
            <n-avatar  round :size="48" src="https://07akioni.oss-cn-beijing.aliyuncs.com/07akioni.jpeg" />
            
            <div style="margin:0px 20px">
                <n-p>이현희</n-p>
                <n-p>1983년 12월 28일</n-p>
                <n-p>12승/12무/12패 (50%)</n-p>
                <n-p>12골 12어시스트 1save</n-p>
            </div>
            
        </n-space>
         <n-space  justify ="end" align="center">
            <Uploader Text="사진등록" ></Uploader>
        </n-space>
        
    </n-card>
</template>
<script setup>
import { onMounted } from "vue"
import Uploader from "@/zenc/layout/components/Uploader.vue"

const Props = defineProps({
    Item: { type: Object }
})

onMounted(()=>{
    
})
</script>