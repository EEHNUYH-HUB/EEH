import { GetRect,Guid } from "@/zenc/js/Common"

export default class MNDrawIcon{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Type = "ICON"
        this.IsDown = false;
        this.DrawItem = "";
        this.StartPoint = null;
        this.EndPoint = null;
        this.Ps = null;
        this.ColorObj = colorobj;
    }
    
    
    DrawIcon(type,x,y){
        var obj = new Object;
        obj.IconType = type;
        obj.Type = this.Type;
        obj.ID= Guid();
        obj.StrokeColor = this.ColorObj .Stroke;
        obj.FillColor = this.ColorObj.Fill;
        obj.Rect = GetRect(x-8, y-8, x+8, y+8);
        console.log(obj.Rect)
        this.ObjList.push(obj)
        this.Eventer.AddedMethod(obj);
        console.log(obj)
    }
    
   
}


