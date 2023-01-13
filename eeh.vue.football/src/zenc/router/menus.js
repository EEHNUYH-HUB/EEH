import {h,ref} from 'vue'
import { RouterLink  } from 'vue-router'

function ConvertToMenuItem(currentDept,allowDept,parent,menuJson,pagePath,findActiveIdHandler){
    
    var obj = new Object;
    obj.key = menuJson.id;
    obj.desc= menuJson.desc;
    var strPath = parent+menuJson.path;
    var iof = pagePath.indexOf(strPath);
    if(iof > -1){
        //console.log(iof,strPath,pagePath,menuJson.id)
        findActiveIdHandler(menuJson.id);
    }
    if(menuJson.children && menuJson.children.length > 0){
        
        if(currentDept < allowDept){
            obj.children = new Array;
            for(var i in menuJson.children){    
                obj.children.push(ConvertToMenuItem(currentDept+1,allowDept,strPath+"/",menuJson.children[i],pagePath,findActiveIdHandler));
            }
        }
    }
    if(menuJson.component)
        obj.label = () =>h( RouterLink, { to: { name: menuJson.path } }, { default: () => menuJson.name });
    else{
        obj.type = "group";
        obj.label = () => menuJson.name ;
    }

    return obj;
}

export function GetBreadcrumbObjRef(route){
    
    var pagePath = route.fullPath;
    var paths = pagePath.split('/');
    var rtn = new Array;
    var len = paths.length;
    var targetList =  menuJsonList;
    var beforePath = "/";
    for(var i =1;i<len;i++){
        var currentPath = paths[i];
        for(var j in targetList){
            var tmp = targetList[j];
            if(tmp.path == currentPath){
                rtn.push({ name : tmp.name ,path:tmp.path ,desc:tmp.desc});
                targetList = tmp.children;
                break;
            }
        }
    }
    
    return ref(rtn);
}

export function GetMenuObjRef(startIndex,allowDept,route) {
    var rtn = new Object;
    rtn.Menus = new Array;
    rtn.ActiveKey =""

    if(allowDept > 0){
        var pagePath = route.fullPath;
        var paths = pagePath.split('/');
        
        
        var len = paths.length-1;
        var targetList = null;
        var beforePath = "/";
        var currentIndex = 0;
        if(len > startIndex){
            
            targetList =  menuJsonList;
            if(startIndex != currentIndex){
                currentIndex+=1;
                for(var i =1;i<len;i++){
                    var currentPath = paths[i];
                    var isOK = false;
                    
                    for(var j in targetList){
                        var tmp = targetList[j];
                        if(tmp.path == currentPath){
                            targetList = tmp.children;
                            beforePath += tmp.path+"/";
                            isOK = true;
                            break;
                        }
                    }

                    if(isOK){
                        isOK = false;
                        if(startIndex == currentIndex){
                            break;
                        }
                        currentIndex +=1;
                    }else{
                        targetList = null;
                        break;
                    }
                
                }
            }
        }

        if(targetList){
            for(var i in targetList){    
                rtn.Menus.push(ConvertToMenuItem(1,allowDept,beforePath,targetList[i],pagePath,(id)=>{
                    rtn.ActiveKey = id
                }));
            }

            //rtn.ActiveKey = FindActiveKey(pagePath.Menus);
        
        }
    }

    return ref(rtn);
}

// function FindActiveKey(path,menus){
//     for(var i in menus){
//         if()
//     }
// }
export function GetRoutes () {
    
    var result =  ConvertToRoutes(menuJsonList,true,"");

    return result;
}

function ConvertToRoutes(lst,isFirst,beforePath){
    var rtn = new Array;

    for(var i in lst){
        var item = lst[i];
    
        
        var strPath = (isFirst?'/':'')+item.path; 
        if(isFirst){
            rtn.push({
                
                path: "/",
                redirect :strPath
                
            });
        }
        if(item.children && item.children.length >0){
           
            var redirectPath = beforePath? beforePath+'/'+strPath:strPath;
            var cItems =ConvertToRoutes(item.children,false,redirectPath);
            rtn.push({
                
                path: strPath,
                name :item.path,
                component: item.component,
                redirect :redirectPath+'/'+ cItems[0].path,
                children : cItems
            });
        }
        else{
            rtn.push({
                path: strPath,
                name :item.path,
                component: item.component
            });
        }
    }

    return rtn;
}


const menuJsonList =
[
    // {
    //     id:"0"
    //     ,name:"Developer Advanced Setting"
    //     ,path:"dasapi"
    //     ,desc: "개발 및 프로젝트 초기 설정"
    //     ,component: () => import("@/zenc/layout/LY0001.vue")
    //     ,children:[
    //         {
    //             id:"0-1"
    //             ,name:".Net 6.0 환경 설정"
    //             ,path:"be"       
    //             ,desc: ".Net 6.0 관련 소스 위치 및 설정 방법"                   
    //             ,component : () => import("@/views/D0001.vue")                      
    //         }
    //         , {
    //             id:"0-2"
    //             ,name:"Vue3.0 환경 설정"
    //             ,path:"fe"       
    //             ,desc: "Vue3.0 관련 소스 위치 및 설정 방법"         
    //             ,component : () => import("@/views/D0001.vue")                      
    //         }
    //     ]
    // },
    // {
    //     id:"1"
    //     ,name:"Rest API Sample"
    //     ,path:"rstapi"
    //     ,desc: "Rest API Sample를 설정하는 그룹"
    //     ,component: () => import("@/zenc/layout/LY0001.vue")
    //     ,children:[
    //         {
    //             id:"1-1"
    //             ,name:"Auth API 호출"
    //             ,path:"authapi"       
    //             ,desc: "Auth API 사용 설명"         
    //             ,children:[
    //                 {
    //                     id:"1-1-1"
    //                     ,name:"Certification"
    //                     ,path:"gcmethod"
    //                     ,desc: "인증 관련"         
    //                     ,component : () => import("@/views/T0001.vue")                      
                                   
    //                 }      
    //             ]                
                           
    //         },
    //         {
    //             id:"1-2"
    //             ,name:"DataBase API 호출"
    //             ,path:"dbapi"                
    //             ,children:[
    //                 {
    //                     id:"1-2-1"
    //                     ,name:"SmartSQL"
    //                     ,path:"smsql"
    //                     ,desc: "MyBatis in C# + .NET Core + Cache ( Memory|Redis )"         
    //                     ,component : () => import("@/views/T0002.vue")                      
                                   
    //                 }      
    //                 ,{
    //                     id:"1-2-2"
    //                     ,name:"Dynamic Procedure"
    //                     ,path:"dp"
    //                     ,desc: "Stored procedure 호출"         
    //                     ,component : () => import("@/views/T0003.vue")                      
                                   
    //                 }    
    //             ]                
                           
    //         },
    //         {
    //             id:"1-3"
    //             ,name:"Dynamic API 호출 "
    //             ,path:"dyapi"
    //             ,desc: "동적 DLL 메서드 호출 방법"         
    //             ,children:[
    //                 {
    //                     id:"1-3-1"
    //                     ,name:"LoadAssembly"
    //                     ,path:"lamethod"
    //                     ,desc: "동적 DLL 메서드 호출 방법"         
    //                     ,component : () => import("@/views/T0001.vue")                 
    //                 }       
                    
    //             ]                    
                           
    //         }
    //         ,
    //         {
    //             id:"1-4"
    //             ,name:"File API 호출 "
    //             ,path:"fileapi"
    //             ,desc: "파일 업로드 방법"         
    //             ,children:[
    //                 {
    //                     id:"1-4-1"
    //                     ,name:"Upload"
    //                     ,path:"upmethod"
    //                     ,desc: "파일 업로드 방법"         
    //                     ,component : () => import("@/views/F0001.vue")                 
    //                 }       
    //                 ,  {
    //                     id:"1-4-2"
    //                     ,name:"Download"
    //                     ,path:"dwmethod"
    //                     ,desc: "파일 다운로드 방법"         
    //                     ,component : () => import("@/views/T0001.vue")                 
    //                 }    
    //             ]                
                           
    //         }
    //     ]
    // }
    // ,
    // {
    //     id:"2"
    //     ,name:"Naive UI Sample"
    //     ,path:"Comp"
    //     ,desc: "UI 샘플 리스트"         
    //     ,component: () => import("@/zenc/layout/LY0001.vue")
    // }
    // ,
    // {
    //     id:"3"
    //     ,name:"Azure API Sample"
    //     ,path:"azapi"
    //     ,desc: "Azure API Sample를 설정하는 그룹"
    //     ,component: () => import("@/zenc/layout/LY0001.vue")
    // }
    // ,
    {
        id:"4"
        ,name:"JMFC"
        ,path:"setting"
        ,desc: "JMFC"
        ,component: () => import("@/zenc/layout/LY0001.vue")
        ,children:[
             {
                id:"4-1"
                ,name:"League Setting"
                ,path:"league"       
                ,desc: "리그를 설정 합니다."         
                ,component : () => import("@/views/settings/league/L0001.vue")                      
            }
            ,{
                id:"4-2"
                ,name:"Player Setting"
                ,path:"player"       
                ,desc: "승점 = 승리 * 3 + 무승부 * 1 + 패배 * 0 , 능력치 = 골 * 5 + 어시스트 * 2 + 세이브 * 1"                   
                ,component : () => import("@/views/settings/player/P0001.vue")                        
            }
            , {
                id:"4-3"
                ,name:"Location Setting"
                ,path:"location"       
                ,desc: "장소 정보를 설정 합니다."         
                ,component : () => import("@/views/settings/location/L0001.vue")                      
            }
            
        ]
    }
]


    