
import { GetRect,Guid } from "@/zenc/js/Common"

export default class MNDrawPen{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "PEN";
        this.IsDown = false;
        this.DrawItem = "";
        this.Path = "";
        this.StartPoint = null;
        this.EndPoint = null;
        this.ColorObj = colorobj;
        this.DrawItems = new Array;
    }
    MouseUp(e) {
    
        if (this.IsDown) {
            this.IsDown = false;
            this.Path = this.DrawItem;

            var obj = new Object;
            obj.Type = this.Name;
            obj.ID = Guid();
            obj.Path = "";
            var startX = this.StartPoint.X;
            var startY = this.StartPoint.Y;

            for (var i in this.DrawItems) {
                var item = this.DrawItems[i];
                var x = item.X - startX;
                var y = item.Y - startY;
                if (i == 0) {
                    obj.Path = "M "+ x +" "+y;
                }
                else {
                    obj.Path  += " L " + x  + " " + y;
                    obj.Path  += "  " +x + " " + y;
                }
            }

           
            this.DrawItems = new Array;
            obj.StrokeColor = this.ColorObj .Stroke;
            obj.FillColor = this.ColorObj .Fill;
        
            var rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
            obj.Rect = rect;
            //if(obj.Rect.Width>10 && obj.Rect.Height >10)
            this.ObjList.push(obj)
            this.Eventer.AddedMethod(obj);
            this.StartPoint = null;
            this.EndPoint = null;
            this.DrawItem = "";
        }
        return false;
    }
    MouseDown(e) {
        this.IsDown = true;
        this.DrawItem = "M " + e.offsetX + " " + e.offsetY;

        this.DrawItems.push({ X: e.offsetX, Y: e.offsetY });
        this.StartPoint = new Object;
        this.EndPoint = new Object;
        this.StartPoint.X = e.offsetX;
        this.StartPoint.Y = e.offsetY;
        this.EndPoint.X = e.offsetX;
        this.EndPoint.Y = e.offsetY;
    }
    MouseMove(e) {
        if (this.IsDown) {
            this.DrawItem += " L " + e.offsetX + " " + e.offsetY;
            
            this.DrawItems.push({ X: e.offsetX, Y: e.offsetY });

            if (e.offsetX < this.StartPoint.X) {
                this.StartPoint.X = e.offsetX;
            }
            if (e.offsetY < this.StartPoint.Y) {
                this.StartPoint.Y = e.offsetY;
            }
            if (e.offsetX > this.EndPoint.X) {
                this.EndPoint.X = e.offsetX;
            }
            if (e.offsetY > this.EndPoint.Y) {
                this.EndPoint.Y = e.offsetY;
            }

        }
    }

    
}