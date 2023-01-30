<template>
  <div :class="store.state.useIsMobile ? 'div_root_mobile' : 'div_root_default'">
    <div :class="store.state.useIsMobile ? 'div_mobile' : 'div_default'" class=" document-scroll-container">
      <Breadcrumb></Breadcrumb>
      <n-grid cols="1 s:1 m:2 l:2 xl:5 2xl:5" :x-gap="12" :y-gap="8" responsive="screen">
        <n-gi v-for="(item, index) in data" :key="index">
          <n-card :title="item.title" :id="'id' + item.type">
            <n-data-table :columns="item.columns" :data="item.data" :bordered="true" :single-line="false" single-column
              size="small" />
          </n-card>
        </n-gi>

      </n-grid>
    </div>
    <Anchor :Items="AnchorItems" DivID=".document-scroll-container"></Anchor>
  </div>


</template>
  
<script setup>
import { onMounted, ref } from 'vue'
import { useStore } from "vuex";

import Breadcrumb from "@/zenc/layout/components/Breadcrumb.vue"
import { CashOutline as CashIcon } from '@vicons/ionicons5'
import Anchor from "@/zenc/layout/components/Anchor.vue"

const store = useStore()
const data = ref(null)
const AnchorItems = ref(new Array);

onMounted(async () => {
  var dt = await store.state.apiClient.ExecDataTable('SQL', 'SELRANKING', null);

  data.value = new Array;


  var dt2 = await store.state.apiClient.ExecDataTable('SQL', 'SELMEMBER', null);
  for (var i in dt2) {
    var row = dt2[i];
    var totalCnt = row.wincnt + row.losscnt + row.tiecnt;
    if (totalCnt > 0) {
      row.rate= (row.wincnt * 100.0 / totalCnt).toFixed(0);
    }
    else {
      row.rate = 0;
    }
  }

  data.value.push(convertTo('rate', dt2));
  data.value.push(convertTo('score', dt));
  data.value.push(convertTo('goal', dt));
  data.value.push(convertTo('assist', dt));
  data.value.push(convertTo('save', dt));
  
  
  AnchorItems.value = new Array;
  AnchorItems.value.push({ Title: '승률 순위', ID: '#idrate' });
  AnchorItems.value.push({ Title: '종합 순위', ID: '#idscore' });
  AnchorItems.value.push({ Title: '골 순위', ID: '#idgoal' });
  AnchorItems.value.push({ Title: '도움 순위', ID: '#idassist' });
  AnchorItems.value.push({ Title: '수비 순위', ID: '#idsave' });
  
  

})


const convertTo = (type, data) => {
  var rtn = new Object;
  rtn.type = type;
  rtn.columns = new Array;
  rtn.columns.push({ title: '순위', key: 'ranking', width: 80, align: 'center' })
  rtn.columns.push({ title: '이름', key: 'col_name' })
  rtn.data = new Array;

  var sortData = null;
  if (type == 'goal') {
    rtn.title = "골 순위"
    rtn.columns.push({ title: '골', key: 'goalcnt', width: 80, align: 'center' })

    sortData = data.sort(compareGoal);
  }
  else if (type == 'assist') {
    rtn.title = "도움 순위"
    rtn.columns.push({ title: '도움', key: 'assistcnt', width: 80, align: 'center' })
    sortData = data.sort(compareAssist);
  }
  else if (type == 'save') {
    rtn.title = "수비 순위"
    rtn.columns.push({ title: '수비', key: 'savecnt', width: 80, align: 'center' })
    sortData = data.sort(compareSave);
  }
  else if (type == 'score') {
    rtn.title = "종합 순위"
    rtn.columns.push({ title: '점수', key: 'score', width: 80, align: 'center' })
    sortData = data.sort(compareScore);
  }
  else if (type == 'rate') {
    rtn.title = "승률 순위"
    rtn.columns.push({ title: '승률(%)', key: 'rate', width: 80, align: 'center' })
    sortData = data.sort(compareRate);
  }
  var beforeValue = 0;
  var rank = 0;
  for (var i in sortData) {

    
    var obj = new Object;
    var tmp = sortData[i];
    obj.goalcnt = tmp.goalcnt;
    obj.assistcnt = tmp.assistcnt;
    obj.savecnt = tmp.savecnt;
    obj.score = tmp.score;
    obj.col_name = tmp.col_name;
    obj.rate = tmp.rate;
    var chkValue;
    if (type == 'goal') {
      chkValue  = obj.goalcnt;
    }
    else if (type == 'assist') {
      chkValue  = obj.assistcnt;
    }
    else if (type == 'save') {
      chkValue  = obj.savecnt;
    }
    else if (type == 'score') {
      chkValue  = obj.score;
    }
    else if (type == 'rate') {
      chkValue  = obj.rate;
    }

    if(beforeValue ==0){
      rank +=1;
    }
    else{
      if(chkValue != beforeValue){
        rank =i*1+1;
      }
    }

    beforeValue = chkValue;
    obj.ranking = rank + '위';
    
    if (rank > 10 || chkValue == 0) break;
    rtn.data.push(obj)
    
  }
  return rtn;

}

function compareGoal(a, b) {
  if (a.goalcnt > b.goalcnt) {
    return -1;
  }
  if (a.goalcnt < b.goalcnt) {
    return 1;
  }
  // a must be equal to b
  return 0;
}

function compareAssist(a, b) {
  if (a.assistcnt > b.assistcnt) {
    return -1;
  }
  if (a.assistcnt < b.assistcnt) {
    return 1;
  }
  // a must be equal to b
  return 0;
}

function compareSave(a, b) {
  if (a.savecnt > b.savecnt) {
    return -1;
  }
  if (a.savecnt < b.savecnt) {
    return 1;
  }
  // a must be equal to b
  return 0;
}

function compareScore(a, b) {
  if (a.score > b.score) {
    return -1;
  }
  if (a.score < b.score) {
    return 1;
  }
  // a must be equal to b
  return 0;
}

function compareRate(a, b) {
  if (a.rate > b.rate) {
    return -1;
  }
  if (a.rate < b.rate) {
    return 1;
  }
  // a must be equal to b
  return 0;
}
</script>
