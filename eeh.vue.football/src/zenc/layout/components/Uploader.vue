<template>
    <div>
        <n-button ghost @click="OnShow"> {{ Props.Text }} </n-button>
        <input class="btnFileLoad d-none" type="file" multiple ref="oFileHandler" v-show="false"
            @change="FileChangeHandler" />
        <n-drawer v-model:show="IsShow" :default-width="420" :placement="Placement" resizable>
            <n-drawer-content>
                <n-space vertical>
                    <div class="upload_div" draggable="true" @dragenter=OnDragEnter @drop="OnFileDrop"
                        @click="ShowFileDig">



                        <div style="margin-bottom: 12px">
                            <n-icon size="48" :depth="3">
                                <archive-icon />
                            </n-icon>
                        </div>
                        <n-p depth="3" style="margin: 8px 0 0 0">
                            영역을 클릭 하거나 파일을 드래그 하세요.
                        </n-p>
                    </div>

                    <div v-for="(item, index) in FileList" :key="index" @click="OnClickProgress(item)">
                        <div style="display:flex;"> 
                            {{ item.Name }} 
                        </div>
                        <n-progress type="line"  :percentage="item.Percent" :status="item.Status" >
                            
                        </n-progress>
                    </div>
                </n-space>
                <template #footer>
                    <n-button @click="OnAllRunUpload">
                        {{ Props.Text }}
                    </n-button>
                </template>
            </n-drawer-content>
        </n-drawer>
    </div>
</template>
  
<script setup>
import { ref ,defineEmits} from 'vue'
import { ArchiveOutline as ArchiveIcon,Close } from '@vicons/ionicons5'
import {  useMessage,useDialog   } from 'naive-ui'
import FileHandler from '@/zenc/js/FileHandler'
import {DownloadLink,ImageLink} from '@/zenc/js/Common'

const oFileHandler = ref();
const IsShow = ref(false);
const FileList = ref(new Array);
const Message = useMessage();
const Dialog = useDialog();
const Props = defineProps({
    Text: { type: String, default: 'Upload' },
    Placement: { type: String, default: 'right' }
})
const Emits = defineEmits(['completed','error'])
const OnShow = () => {
    IsShow.value = !IsShow.value;
    if (FileList.value) {
        var len = FileList.value.length;
        for (var i = len-1 ;i >= 0 ;i --) {
            var item = FileList.value[i];

            if(item.Status == "success"){
                FileList.value.splice(i,1);
            }
        }
    }

}
const OnCloseFile = (item)=>{
    for(var i in  FileList.value){
        var  citem = FileList.value[i];
        if(item == citem){
            FileList.value.splice(i,1);
            break;
        }
    }
}
const OnFileDrop = () => {
    alert("준비중")
}
const OnDragEnter = (e) => {
    if (e.preventDefault) {
        e.preventDefault();
    }
}
const FileChangeHandler = async (e) => {

    for (var i = 0; i < e.target.files.length; i++) {
        var obj = e.target.files[i];
        FileList.value.push({ File: obj, Percent: 0, Name: obj.name ,Status:"info",Msg:''});
    }
}
const ShowFileDig = () => {
    oFileHandler.value.click();
}

const OnAllRunUpload = () => {
    for (var i in FileList.value) {
        OnRunUpload(FileList.value[i])
    }
}
const OnClickProgress = (item) =>{

    var p = {
          title: '',
          content: '항목에서 제거 하시겠습니까?',
          positiveText: 'Yes',
          negativeText: 'No',
          onPositiveClick: () => {
            OnCloseFile(item);
          },
          onNegativeClick: () => {
            
          }
        }
    if(item.Status == "success"){
        p.title = "전송이 완료 되었습니다.";
        Dialog.success(p)

    }
    else if(item.Status == "info"){
        p.title = "전송중 입니다.";
        Dialog.info(p)}    
    else if(item.Status == "error"){
        p.title = "전송이 실패 되었습니다.";
        Dialog.error(p)}
    
    if(item.Msg){

        Message.error(item.Msg);
    }
        
}
const OnRunUpload = (item) => {

    if(item.Status == "success") return;

    item.Status = "info";
    item.Msg = '';
    var uploader = new FileHandler();
    uploader.CompletedEvent = (staticID) => {
        item.Status = "success";
        item.StaticID = staticID;
        item.Url = DownloadLink(staticID);
        item.ImageUrl =ImageLink(staticID);
        Emits('completed',item);
    }
    uploader.ProgressEvent = (pecent) => {
        item.Percent = pecent;
        
    }

    uploader.ErrorEvent = (msg) => {
        
        item.Status = "error";
        item.Msg = msg;
        Emits('error',item);
    }

    uploader.Upload(item.File, null);
}

</script> 
<style scoped>
.upload_div {
    cursor: pointer;
    box-sizing: border-box;
    width: 100%;
    text-align: center;
    border-radius: 3px;
    padding: 24px;
    opacity: 1;
    transition: opacity .3s cubic-bezier(0.4, 0, 0.2, 1), border-color .3s cubic-bezier(0.4, 0, 0.2, 1), background-color .3s cubic-bezier(0.4, 0, 0.2, 1);
    border: 1px dotted rgb(224, 224, 230);
}
</style>