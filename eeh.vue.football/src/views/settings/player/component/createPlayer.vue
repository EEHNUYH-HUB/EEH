<template>
    <n-drawer v-model:show="Show" :default-width="420" placement="right" resizable>
        <n-drawer-content title="Create Player" closable>
            <n-space vertical>
                <n-input v-model:value="newItem.name" placeholder="이름을 입력해 주세요" />
                <n-date-picker v-model:value="newItem.birtyDay" placeholder="생년월일을 입력해 주세요" type="date" />
                <n-input v-model:value="newItem.phone" :allow-input="OnlyAllowNumber" placeholder="전화번호를 입력해 주세요" />
            </n-space>
            <template #footer>
                <n-button @click="OnCretePlayer">
                    저장
                </n-button>
            </template>
        </n-drawer-content>
    </n-drawer>
</template>
<script setup>
import {ref,onMounted,computed,defineEmits} from 'vue'
import {  OnlyAllowNumber ,ConvertDateToYYYYMMDD} from '@/zenc/js/Common'

import { useStore } from "vuex";
const newItem = ref(null)
const Props = defineProps({Show:{type:Boolean},Item:{type:Object}})
const store = useStore()
onMounted(()=>{
    InitNewItem(Props.Item);
})
const InitNewItem = (item) => {
    newItem.value = new Object;
    if (item) {
        newItem.value.name = item.col_name;
        var y = parseInt(item.col_birthday.substring(0,4));
        var m = parseInt(item.col_birthday.substring(4,6))-1;
        var d = parseInt(item.col_birthday.substring(6,8));

        
        var newDate = new Date( y, m, d);
        newItem.value.birtyDay = newDate.getTime();
        newItem.value.phone = item.col_phone;
        newItem.value.pkid= item.pk_id;
    }
    else {
        newItem.value.name = "";
        newItem.value.birtyDay = null;
        newItem.value.phone = "";
        newItem.value.pkid = -1;;
    }

}

const OnCretePlayer = async () => {
    
    var birthday = ConvertDateToYYYYMMDD(new Date(newItem.value.birtyDay));
    var params = [
        { key: 'name', value: newItem.value.name },
        { key: 'phone', value: newItem.value.phone },
        { key: 'birthday', value: birthday},
        { key: 'pkid', value: newItem.value.pkid}
    ];

    await store.state.apiClient.Run('EEH.FOOTBALL.BIZ', 'FootballBiz', 'UpsertMember',params);

    Show.value = false;

    if(Props.Item){
        Props.Item.col_phone = newItem.value.phone;
        Props.Item.birtyDay = birthday;
        Props.Item.col_name = newItem.value.name;
    }
    emit('changed',newItem.value);

}
const emit= defineEmits(['update:Show','changed'])
const Show = computed(({
    get() {
      return Props.Show;
    },
    set(val) {
        emit('update:Show',val)
    }
  }))
</script>