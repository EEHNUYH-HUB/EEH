import { GetRect,Guid } from "@/zenc/js/Common"

export default class MNDrawLine{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;


        this.Name = "RECT";
        this.IsDown = false;
        this.DrawItem = "";
        this.StartPoint = null;
        this.EndPoint = null;
        this.ColorObj = colorobj;
    }
    MouseUp(e){
        this.DrawItem = "";
        this.IsDown = false;
        var obj = new Object;
        obj.Type = this.Name;
            obj.ID = Guid();
        var rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
        
        obj.StrokeColor = this.ColorObj .Stroke;
        obj.FillColor = this.ColorObj.Fill;
        obj.Rect = rect;
        //if(obj.Rect.Width>10 && obj.Rect.Height >10)
        this.ObjList.push(obj)
        this.Eventer.AddedMethod(obj);
        this.StartPoint = null;
        this.EndPoint = null;
    }
    MouseDown(e) {
        this.IsDown = true;
        
        this.StartPoint = new Object;
        this.EndPoint = new Object;
        this.StartPoint.X = e.offsetX;
        this.StartPoint.Y = e.offsetY;
        this.EndPoint.X = e.offsetX;
        this.EndPoint.Y = e.offsetY;

        this.DrawItem = "M" + this.StartPoint.X + " " + this.StartPoint.Y;
    }
    MouseMove(e) {
        if (this.IsDown) {
            this.EndPoint.X = e.offsetX;
            this.EndPoint.Y = e.offsetY;
            
            this.DrawItem
                = " M" + this.StartPoint.X + " " + this.StartPoint.Y
                + " L" + this.EndPoint.X + " " + this.StartPoint.Y
                + " L" + this.EndPoint.X + " " + this.EndPoint.Y
                + " L" + this.StartPoint.X + " " + this.EndPoint.Y
                + " L" + this.StartPoint.X + " " + this.StartPoint.Y +" Z"        ;
            
        }
    }
}
