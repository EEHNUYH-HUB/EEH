<template>
    <n-card hoverable :id="'id' + Props.Item.pk_id">
        <n-page-header :title="Props.Item.col_name" 
        :subtitle="GetBirthDay(Props.Item.col_birthday) + ' ' + Postion">
            <n-grid :cols="5">
                <n-gi>
                    <n-space >
                        <n-statistic size="small" label="승률(%)" :value="WinRate" />
                    </n-space>
                </n-gi>
                <n-gi>
                    <n-space >
                        <n-statistic label="승패"
                            :value="Props.Item.wincnt + '/' + Props.Item.tiecnt + '/' + Props.Item.losscnt" />
                    </n-space>
                </n-gi>
                <n-gi>
                    <n-space >
                        <n-statistic label="득점"
                            :value="Props.Item.goal + '/' + Props.Item.assist + '/' + Props.Item.save" />
                    </n-space>
                </n-gi>
                <!-- <n-gi>
                    <n-space justify="center">
                    <n-statistic label="패배" :value="Props.Item.losscnt" />
                </n-space>
                </n-gi>  -->
                <n-gi>
                    <n-space >
                        <n-statistic label="승점" :value="Props.Item.teamscore" />
                    </n-space>
                </n-gi>

                <n-gi>

                    <n-space >
                        <n-statistic label="능력치" :value="Props.Item.memberscore" />
                    </n-space>
                </n-gi>

            </n-grid>

<!-- 
            <template #avatar>
                <n-avatar src="https://cdnimg103.lizhi.fm/user/2017/02/04/2583325032200238082_160x160.jpg" />
            </template> -->
            <template #extra>
                <n-space>
                    
                        <!-- <Uploader Text="사진등록" ></Uploader> -->
                </n-space>
            </template>

        </n-page-header>



    </n-card>
</template>
<script setup>
import { onMounted, ref, computed } from "vue"
import { GetBirthDay, GetPhoneNumber } from "@/zenc/js/Common"
import Uploader from "@/zenc/layout/components/Uploader.vue"

const Props = defineProps({
    Item: { type: Object }
})

const WinRate = computed(({
    get() {
        var totalCnt = Props.Item.wincnt + Props.Item.losscnt + Props.Item.tiecnt;
        if (totalCnt > 0) {
            return (Props.Item.wincnt * 100. / totalCnt).toFixed(0);
        }
        else '0%';
    }
}))


const Postion = computed(({
    get() {


        if (Props.Item.goal == Props.Item.assist && Props.Item.assist == Props.Item.save) {
            return "";
        }
        else if(Props.Item.goal > Props.Item.assist  &&  Props.Item.goal > Props.Item.save){
            return "공격수"
        }
        else if(Props.Item.assist > Props.Item.goal  &&  Props.Item.assist > Props.Item.save){
            return "미들필더"
        }
        else if(Props.Item.save > Props.Item.assist  &&  Props.Item.save > Props.Item.goal){
            return "수비수"
        }
        else if (Props.Item.goal == Props.Item.assist) {
            if (Props.Item.goal > Props.Item.save) {
                return "공격형 미들필더" 
            }
            else {
                return "수비형 미들필더"
            }
        }
        else if (Props.Item.goal == Props.Item.save) {
            if (Props.Item.goal > Props.Item.assist) {
                return "공격수"
            }
            else {
                return "미들필더"
            }
        }
        else if (Props.Item.assist == Props.Item.save) {
            if (Props.Item.assist > Props.Item.goal) {
                return "수비형 미들필더"
            }
            else {
                return "공격수"
            }
        }
        
    }
}))
</script>