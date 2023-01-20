<template>
  <n-loading-bar-provider>
    <n-message-provider>
      <n-notification-provider>
        <n-dialog-provider>
          <SiteRoot></SiteRoot>
        </n-dialog-provider>
      </n-notification-provider>
    </n-message-provider>
  </n-loading-bar-provider>
</template>

<script steup>

import SiteRoot from '@/zenc/SiteRoot.vue'

export default {
  name: 'App',
  components: {
    SiteRoot
  }
}
</script>

