import axios from "axios";
import {ConvertKeyValueObjectToObject} from '@/zenc/js/Common'
// function setInterceptors(instance) {
//     instance.interceptors.request.use(
//       function(config) {
//         var apikey = sessionStorage.getItem("apikey");
//         config.headers.Authorization = apikey ? apikey : '';


//         if(APIClient && APIClient.LodingBar){
//           APIClient.LodingBar.start();
//         }
//         return config;
//       },
//       ErrorMethod,
//     );

//     // Add a response interceptor
//     instance.interceptors.response.use(
//       function(response) {
//         if(APIClient && APIClient.LodingBar){
//           APIClient.LodingBar.finish();
//         }
//         APIClient.Message.success('성공하였습니다.')
//         APIClient.Notification.success({content:"성공하였습니다.",
//         meta:JSON.stringify(response.data),duration: 2500,keepAliveOnHover: true})

//         return response;
//       },
//       ErrorMethod,
//     );

//     return instance;
//   }
//   function ErrorMethod(error){

//     if(APIClient){
//       if(APIClient.LodingBar)
//         APIClient.LodingBar.error();
//       if(APIClient.Message)
//       {
//         var msg = '';
//         if(error.response.data.message)
//         {
//           msg =error.response.data.message;
//         }
//         else{
//           msg =error.response.data.title;
//         }

//         APIClient.Message.error('['+error.response.status+'] '+ msg)
//       }
//     }
//     if (error && error.response && error.response.status == 401){

//         APIClient.Notification.error({content:"인증실패",
//         meta:`인증 정보가 없습니다. Certification 페이지 에서 Generate 하십시오`,duration: 2500,keepAliveOnHover: true})
//         //alert(error)
//         //store.dispatch('logout');
//     }
//     return Promise.reject(error);

//   }


// function createInstance(){
//   const instance = axios.create({
//     baseURL: process.env.VUE_APP_API_URL
//   });

//   return setInterceptors(instance);
// }

// const axiosService = createInstance();

// const APIClient =  new Object;
// APIClient.LodingBar = null;
// APIClient.Message = null;
// APIClient.Notification = null;

// APIClient.Store = null;

// APIClient.GenerateKey = async function(parameter){

//   const  rtn = await axiosService.post('/api/Auth/GenerateKey', parameter);
//   if(rtn.data){
//     var key = rtn.data;
//     var userInfo =await this.CertificationInfo(key);

//     if(userInfo){
//       sessionStorage.setItem("apikey", key); 

//       this.Store.commit('setUserInfo',userInfo); 
//     }
//   }

//   return rtn.data;
// }
// APIClient.CertificationInfo = async function(token){

//   const  rtn = await axiosService.get('/api/Auth/CertificationInfo?key='+token);
//   return rtn.data;
// }



// APIClient.Run = async function(assemblyName,className,methodName,parameter){
//   const param ={
//     assemblyName: assemblyName,
//     className: className,
//     methodName: methodName,
//     parameter: parameter
//   }


//   const rtn = await axiosService.post('/api/SmartSql/LoadAssembly', param);


//   return rtn.data;
// }

// APIClient.ExecDataSet=async function(scope, statementId, parameter){
//   const param = {
//     scope: scope,
//     statementId : statementId,
//     parameters: parameter
//   }


//   const  rtn = await  axiosService.post('/api/SmartSql/ExecuteDataSet',param);


//   return rtn.data;
// }


// APIClient.ExecDataTable= async function(scope, statementId, parameter){
//   const param = {
//     scope: scope,
//     statementId : statementId,
//     parameters: parameter
//   }

//   const  rtn = await  axiosService.post('/api/SmartSql/ExecuteDataTable', param);

//   return rtn.data;
// }

// APIClient.ExecScalar=async function(scope, statementId, parameter){
//   const param = {
//     scope: scope,
//     statementId : statementId,
//     parameters: parameter
//   }

//   const  rtn = await axiosService.post('/api/SmartSql/ExecuteScalar', param);


//   return rtn.data;
// }

// APIClient.ExecNonQuery =async function(scope, statementId, parameter){
//   const param = {
//     scope: scope,
//     statementId : statementId,
//     parameters: parameter
//   }


//   const rtn = await axiosService.post('/api/SmartSql/ExecuteNonQuery', param);


//   return rtn.data;
// }


// APIClient.FileUpload =async function(parameter){

//   const rtn = await axiosService.post('/api/File/Upload', parameter);

//   return rtn.data;
// }



// export default APIClient;



export default class APIClient {
  constructor() {
    this.LodingBar = null;
    this.Message = null;
    this.Notification = null;
    this.Store = null;
    this.axiosService = this._createInstance();
  }
  _createInstance() {
    const instance = axios.create({
      baseURL: process.env.VUE_APP_API_URL
    });

    return this._setInterceptors(instance);
  }
  _setInterceptors(instance) {
    var _self = this;
    instance.interceptors.request.use(
      function (config) {
        var apikey = sessionStorage.getItem("apikey");
        config.headers.Authorization = apikey ? apikey : 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lSWRlbnRpZmllciI6IjhiMjQ1MDZjLWYxZjMtNGQxYS1hZGRkLTJmZjRkN2IyNTY3MCIsIlVzZXJOYW1lIjoiSk1GQyIsIlVzZXJJRCI6IkpNRkMiLCJFbWFpbCI6ImptZmNAbmF2ZXIuY29tIiwibmJmIjoxNjczMzQxMTQ1LCJleHAiOjE2NzM0Mjc1NDUsImlzcyI6Imh0dHBzOi8vY28ua3IiLCJhdWQiOiJodHRwczovL2NvLmtyIn0.rB1ZRcayQ9HxoDuXndxYUeJsm6hcOizwo4UVrm-TUag';


        if (_self.LodingBar) {
          _self.LodingBar.start();
        }
        return config;
      },
      _self._errorMethod,
    );

    // Add a response interceptor
    instance.interceptors.response.use(
      function (response) {
        if (_self.LodingBar)
          _self.LodingBar.finish();
        // if (_self.Message)
        //   _self.Message.success('성공하였습니다.')
        if (_self.Notification)
          _self.Notification.success({
            content: "성공하였습니다.",
            meta: JSON.stringify(response.data), duration: 2500, keepAliveOnHover: true
          })

        return response;
      },
      _self._errorMethod,
    );

    return instance;
  }
  _errorMethod(error) {

    if (this && this.LodingBar)
      this.LodingBar.error();

    if (this && this.Message) {
      var msg = '';
      if (error.response.data.message) {
        msg = error.response.data.message;
      }
      else {
        msg = error.response.data.title;
      }

      this.Message.error('[' + error.response.status + '] ' + msg)
    }

    if (error && error.response && error.response.status == 401) {

      if (this && this.Notification) {
        this.Notification.error({
          content: "인증실패",
          meta: `인증 정보가 없습니다. Certification 페이지 에서 Generate 하십시오`, duration: 2500, keepAliveOnHover: true
        })
      }

    }
    return Promise.reject(error);

  }




  async GenerateKey(parameter) {

    const rtn = await this.axiosService.post('/api/Auth/GenerateKey', ConvertKeyValueObjectToObject(parameter));
    if (rtn.data) {
      var key = rtn.data;
      var userInfo = await this.CertificationInfo(key);

      if (userInfo) {
        sessionStorage.setItem("apikey", key);
      }
    }

    return rtn.data;
  }
  async CertificationInfo(token) {

    const rtn = await this.axiosService.get('/api/Auth/CertificationInfo?key=' + token);
    var userInfo = rtn.data;
    if (userInfo) {
      this.Store.commit('setUserInfo', userInfo);
    }
    return userInfo;
  }



  async Run(assemblyName, className, methodName, parameter) {
    const param = {
      assemblyName: assemblyName,
      className: className,
      methodName: methodName,
      parameter: ConvertKeyValueObjectToObject(parameter)
    }


    const rtn = await this.axiosService.post('/api/SmartSql/LoadAssembly', param);


    return rtn.data;
  }

  async ExecDataSet(scope, statementId, parameter) {
    const param = {
      scope: scope,
      statementId: statementId,
      parameters: ConvertKeyValueObjectToObject(parameter)
    }


    const rtn = await this.axiosService.post('/api/SmartSql/ExecuteDataSet', param);


    return rtn.data;
  }


  async ExecDataTable(scope, statementId, parameter) {
    const param = {
      scope: scope,
      statementId: statementId,
      parameters: ConvertKeyValueObjectToObject(parameter)
    }

    const rtn = await this.axiosService.post('/api/SmartSql/ExecuteDataTable', param);

    return rtn.data;
  }

  async ExecScalar(scope, statementId, parameter) {
    const param = {
      scope: scope,
      statementId: statementId,
      parameters: ConvertKeyValueObjectToObject(parameter)
    }

    const rtn = await this.axiosService.post('/api/SmartSql/ExecuteScalar', param);


    return rtn.data;
  }

  async ExecNonQuery(scope, statementId, parameter) {
    const param = {
      scope: scope,
      statementId: statementId,
      parameters: ConvertKeyValueObjectToObject(parameter)
    }


    const rtn = await this.axiosService.post('/api/SmartSql/ExecuteNonQuery', param);


    return rtn.data;
  }


  async FileUpload(parameter) {

    const rtn = await this.axiosService.post('/api/File/Upload', parameter);

    return rtn.data;
  }


}