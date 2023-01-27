import { GetRect,Guid } from "@/zenc/js/Common"

export default class MNDrawLine{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "LINE";
        this.IsDown = false;
        this.DrawItem = "";
        this.StartPoint = null;
        this.EndPoint = null;     
       this.ColorObj = colorobj;
    }
    MouseUp(e) {
        if (this.IsDown) {
            this.DrawItem = "";
            this.IsDown = false;
            var obj = new Object;
            obj.Type = this.Name;
            obj.ID = Guid();
            var rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
            obj.Rect = rect;

            obj.X1 = this.StartPoint.X - rect.X;
            obj.Y1 = this.StartPoint.Y - rect.Y;
            obj.X2 = this.EndPoint.X - rect.X;
            obj.Y2 = this.EndPoint.Y - rect.Y;
            obj.StrokeColor = this.ColorObj .Stroke;
            obj.FillColor = this.ColorObj.Fill;
            
            this.StartPoint = null;
            this.EndPoint = null;
            //if(obj.Rect.Width>10 && obj.Rect.Height >10)
            this.ObjList.push(obj)
            this.Eventer.AddedMethod(obj);
        }
    }
    MouseDown(e) {
        this.IsDown = true;
        
        this.StartPoint = new Object;
        this.EndPoint = new Object;
        this.StartPoint.X = e.offsetX;
        this.StartPoint.Y = e.offsetY;
        this.EndPoint.X = e.offsetX;
        this.EndPoint.Y = e.offsetY;


        this.DrawItem = "M" + this.StartPoint.X + " " + this.StartPoint.Y +" Z";
    }
    MouseMove(e) {
        if (this.IsDown) {
            this.EndPoint.X = e.offsetX;
            this.EndPoint.Y = e.offsetY;

            
             this.DrawItem ="M" + this.StartPoint.X + " " + this.StartPoint.Y + " L" + this.EndPoint.X + " " + this.EndPoint.Y +" Z";
            
        }
    }

    
}
