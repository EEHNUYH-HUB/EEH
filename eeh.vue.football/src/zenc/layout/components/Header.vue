<template>
    <n-layout-header class="nav" bordered style="height: 64px; " :style="style">
        <n-text tag="div" class="ui-logo" :depth="1" >
      <img src="@/assets/main_logo.png">
      <span v-if="!store.state.useIsMobile">Developer</span>
    </n-text>
    <div :style="!store.state.useIsMobile ? 'display: flex; align-items: center;' : ''">
      <div v-if="!(store.state.useIsMobile || store.state.useIsTablet)" class="nav-menu">
        <n-menu     mode="horizontal" 
        v-model:value="headerObj.ActiveKey" :options="headerObj.Menus" />
      </div>
      <n-auto-complete :style="!store.state.useIsMobile ? 'width: 216px; margin-left: 24px' : undefined" placeholder="Search" clear-after-select blur-after-select />
    </div>
    <n-popover
      v-if="(store.state.useIsMobile || store.state.useIsTablet)"
      ref="mobilePopoverRef"
      style="padding: 0; width: 288px"
      placement="bottom-end"
      display-directive="show"
      trigger="click"
    >
      <template #trigger>
        <n-icon size="20" style="margin-left: 12px">
          <menu-outline />
        </n-icon>
      </template>
      <div style="overflow: auto; max-height: 79vh">
        <n-menu v-model:value="allMenuObj.ActiveKey" :options="allMenuObj.Menus" :indent="18" />
      </div>
    </n-popover>
    <div v-else  class="nav-end">
      <n-button size="small" quaternary class="nav-picker" @click="store.commit('setTheme')" > {{ store.state.themeName }} </n-button>
      <n-text class="nav-picker padded" v-if="store.state.userInfo"> {{ store.state.userInfo.UserName }}({{store.state.userInfo.Email}}) </n-text>
      <n-space align="flex-end">  
        <n-badge value="9">
          <n-avatar round size="small" src="https://07akioni.oss-cn-beijing.aliyuncs.com/07akioni.jpeg"></n-avatar>
        </n-badge>
      </n-space>
    </div>
    </n-layout-header>
</template>
<script setup>
import { computed} from 'vue'
import { GetMenuObjRef } from "@/zenc/router/menus";
import { MenuOutline } from '@vicons/ionicons5'
import { useStore } from 'vuex'
import { useRoute } from "vue-router";
const store = useStore();
const route = useRoute();
const headerObj = GetMenuObjRef(0,1,route);
const allMenuObj = GetMenuObjRef(0,100,route);

const style = computed(()=>{
    return store.state.useIsMobile ? 
    { '--side-padding': '16px', 'grid-template-columns': 'auto 1fr auto' } : 
    { '--side-padding': '32px', 'grid-template-columns': 'calc(272px - var(--side-padding)) 1fr auto' }
})



</script>



<style scoped>
.nav {
  display: grid;
  grid-template-rows: calc(var(--header-height) - 1px);
  align-items: center;
  padding: 0 var(--side-padding);
}

.ui-logo {
  cursor: pointer;
  display: flex;
  align-items: center;
  font-size: 18px;
}
.ui-logo > img {
  margin-right: 12px;
  
}
.nav-menu {
  padding-left: 36px;
}
.nav-picker {
  margin-right: 4px;
}
.nav-picker.padded {
  padding: 0 10px;
}
.nav-picker:last-child {
  margin-right: 0;
}
.nav-end {
  display: flex;
  align-items: center;
}
</style>

<style>
.nav-menu .n-menu-item {
  height: calc(var(--header-height) - 1px) !important;
}
</style>