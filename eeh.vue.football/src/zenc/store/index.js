import { createStore } from "vuex";
import { darkTheme } from "naive-ui";
import { useMemo } from 'vooks'
import { useIsMobile, useIsTablet } from '@/utils/composables'
import APIClient from '@/zenc/js/RestApiClient';


export default createStore({
  state: {
    themeObj: null,
    themeName : null,
    userInfo : null,
    showSider :null,
    useIsMobile : null,
    useIsTablet : null,
    apiClient: null,
    
  },
  mutations: {
    init(state){
      state.apiClient =new APIClient();
      var strUserInfo = sessionStorage.getItem("userInfo");
      if(strUserInfo){
        state.userInfo = JSON.parse(strUserInfo);
      }

      
       
      state.themeName = localStorage.getItem("themeName");
      if(state.themeName == "Dark"){
        state.themeObj = null;
      }
      else{
        state.themeObj = darkTheme;
      }
      

      state.useIsMobile  =  useMemo(() => { 
        return useIsMobile().value;
      });
      state.useIsTablet  =  useMemo(() => { 
        return useIsTablet().value;
      });

      state.showSider  =  useMemo(() => { 
          return !state.useIsMobile && !state.useIsTablet
        });
    },
    setTheme(state){
      
        if(state.themeName == "Dark"){
            state.themeObj = darkTheme;
        }
        else{
            state.themeObj = null;
        }

        if(state.themeObj){
          state.themeName  = "Light";
        }
        else{
          state.themeName  = "Dark";
        }
        
        
        localStorage.setItem("themeName", state.themeName); 
    }
    ,setUserInfo(state,userInfo){
      state.userInfo = userInfo;
      sessionStorage.setItem("userInfo",JSON.stringify(userInfo));
    }
  },
  actions: {
    empty({ commit }, payload) {
      //commit("sidebarType", payload);
    }
  },
  getters: {
   
  },
});
