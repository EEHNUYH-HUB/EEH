<template >
    <n-drawer v-model:show="show" :default-width="420" placement="right" resizable>
        <n-drawer-content :title="props.Item.DisplayName">
            <n-space vertical>
                <n-card title="기본 정보" size="small">
                    <n-space vertical>
                        <n-input v-model:value="props.Item.DisplayName" type="text" placeholder="이름을 입력해주세요." />
                        <n-color-picker v-model:value="props.Item.StrokeColor" :show-alpha="false" />
                        <n-color-picker v-model:value="props.Item.FillColor" :show-alpha="false" />
                    </n-space>
                </n-card>
                <databaseTask v-if="props.Item.IconType == 'database'" :Item="props.Item"></databaseTask>
                <databaseTableTask v-else-if="props.Item.IconType == 'table'" :Item="props.Item"></databaseTableTask>
                <databaseSqlTask v-else-if="props.Item.IconType == 'sql'" :Item="props.Item"></databaseSqlTask>
                <div v-else>
                    준비중
                </div>
            </n-space>
        </n-drawer-content>
    </n-drawer>
</template>
<script setup>
import databaseTask from "@/zenc/svg/component/task/databaseTask.vue"
import databaseTableTask from "@/zenc/svg/component/task/databaseTableTask.vue"
import databaseSqlTask from "@/zenc/svg/component/task/databaseSqlTask.vue"
import { ref, defineProps, defineEmits, computed } from "vue"
import { useStore } from "vuex";
const store = useStore()


const props = defineProps({ Item: { type: Object }, show: { type: Boolean } })
const emits = defineEmits(['update:show'])
const show = computed(({
    get() {
        return props.show;
    },
    set(val) {
        emits('update:show', val)
    }
}))


</script>
<style scoped>
div {
    -ms-user-select: none;
    -moz-user-select: -moz-none;
    -khtml-user-select: none;
    -webkit-user-select: none;
    user-select: none;
}

</style>