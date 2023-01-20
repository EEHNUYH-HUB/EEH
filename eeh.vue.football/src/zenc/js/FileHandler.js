import APIClient from '@/zenc/js/RestApiClient';
import {Guid} from '@/zenc/js/Common'
export default class MNFileHandler {

    constructor() {
        this.ProgressEvent = null;
        this.CompletedEvent = null;
        this.ErrorEvent = null;
        this.BufferSize = (1024 * 1024) * 0.5; // 2MB씩 split
        this.apiClient = new APIClient();


    }
    Upload(file, param) {
        var freader = new FileReader();
        var _self = this;
        var totalSize = file.size;
        var fileName = file.name;
        var loadSize = 0;
        var endPosition = 0;
        var staticID = Guid();

        freader.onload = async function (e) {
            
            var result = await _self.apiClient.FileUpload({
                StaticID: staticID,
                FileName: fileName,
                FileSize: totalSize,
                Base64String: _self.ArrayBufferToBase64(e.target.result),
                Parameters: param
            });

            loadSize = endPosition;
            if (_self.ProgressEvent)
                _self.ProgressEvent(_self.GetProgressPercent(loadSize, totalSize));


            if (result.resultType == 0) { // COMPLETE

                if (_self.CompletedEvent)
                    _self.CompletedEvent(staticID);
            }
            else if(result.resultType == 2){
                if (_self.ErrorEvent)
                    _self.ErrorEvent(result.errorMsg);
            }
            else {
                if (loadSize >= totalSize) {
                    if (_self.CompletedEvent)
                        _self.CompletedEvent(staticID);
                }
                else {

                    endPosition = _self.GetPostion(loadSize, totalSize);
                    freader.readAsArrayBuffer(file.slice(loadSize, endPosition));
                }
            }


        };
        endPosition = _self.GetPostion(loadSize, totalSize);
        freader.readAsArrayBuffer(file.slice(loadSize, endPosition));
    }
    GetPostion(loadSize, totalSize) {
        if (loadSize + this.BufferSize > totalSize) {

            return totalSize;
        }
        else {
            ;
            return loadSize + this.BufferSize;
        }
    }
    GetProgressPercent(loadSize, totalSize) {
        return Math.round(loadSize * 100 / totalSize)
    }
    ArrayBufferToBase64(buffer) {
        let binary = '';
        let bytes = new Uint8Array(buffer);
        let len = bytes.byteLength;
        for (let i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }
   
    Dispose() {
        //this.WebRTC.CloseAllConnection(this.ConnectionID);
    }
}