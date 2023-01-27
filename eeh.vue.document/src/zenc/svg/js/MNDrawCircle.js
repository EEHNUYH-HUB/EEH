
import { GetRect,GetCenterforRect,Guid } from "@/zenc/js/Common"

export default class MNDrawCircle{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "CIRCLE";
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
            var rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
          
            var r = rect.Width > rect.Height ? rect.Width : rect.Height;
            r = r / 2;
            var cp = GetCenterforRect(rect);
            var rect = GetRect(cp.X -r, cp.Y - r, cp.X +r, cp.Y + r);
            
            var obj = new Object;
            obj.Type = this.Name;
            obj.ID = Guid();
           
            obj.StrokeColor = this.ColorObj .Stroke;
            obj.FillColor = this.ColorObj.Fill;
            obj.Rect = rect;
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

        this.DrawItem = "M" + this.StartPoint.X + " " + this.StartPoint.Y;
    }
    MouseMove(e) {
        if (this.IsDown) {
            this.EndPoint.X = e.offsetX;
            this.EndPoint.Y = e.offsetY;
            
            var rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
          
            var r = rect.Width > rect.Height ? rect.Width : rect.Height;
            var cp = GetCenterforRect(rect);
            this.DrawItem = this.CirclePath(cp.X, cp.Y, r / 2);
            
        }
    }


    CirclePath(cx, cy, r) {
        return 'M '+cx+' '+cy+' m -'+r+', 0 a '+r+','+r+' 0 1,0 '+(r*2)+',0 a '+r+','+r+' 0 1,0 -'+(r*2)+',0';
    }


}
