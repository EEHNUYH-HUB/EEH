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
    {
        id:"4"
        ,name:"시각화 셋팅"
        ,path:"mind"
        ,desc: ""
        ,component: () => import("@/zenc/layout/LY0001.vue")
        ,children:[
            {
                id:"4-0"
                ,name:"MindMap"
                ,path:"maindmap"       
                ,desc: "Mind Map"         
                ,component : () => import("@/views/document/mindmap/M0001.vue")                      
            },
        ]
    }
]


    