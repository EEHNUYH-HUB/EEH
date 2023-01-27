
import { GetRect,Guid } from "@/zenc/js/Common"
export default class MNDrawPolygon{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "POLYGON";
        this.IsDown = false;
        this.DrawItem = "";
        
        this.StartPoint = null;
        this.EndPoint = null;
        this.PointList = null;
        this.ColorObj = colorobj;
    }
    MouseUp(e) {
        if (!this.IsDown){
            this.DrawItem = "";
            if (this.PointList) {
                var obj = new Object;
                obj.Type = this.Name;
                obj.ID = Guid();
                obj.Ps = new Array;
              
                for (var i in this.PointList) {
                    
                    obj.Ps.push(this.PointList[i].X- this.StartPoint.X);
                    obj.Ps.push(this.PointList[i].Y- this.StartPoint.Y);

                }
                obj.StrokeColor = this.ColorObj .Stroke;
                obj.FillColor = this.ColorObj.Fill;
                obj.Rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
                //if(obj.Rect.Width>10 && obj.Rect.Height >10)
                this.ObjList.push(obj)
                this.Eventer.AddedMethod(obj);
            }
            this.PointList = null;
            this.StartPoint = null;
            this.EndPoint = null;
        }
    }
    MouseDown(e) {
        this.IsDown = true;

        if (e.button == 2)
        {
            this.IsDown = false;
        }
        
        if (!this.StartPoint) {
            this.StartPoint = new Object;
            this.StartPoint.X = e.offsetX;
            this.StartPoint.Y = e.offsetY;
            
        }
        if (!this.EndPoint) {
            this.EndPoint = new Object;
            this.EndPoint.X = e.offsetX;
            this.EndPoint.Y = e.offsetY;
        }
       
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
        

        if (!this.PointList)
            this.PointList = new Array;
        
        this.PointList.push({X: e.offsetX,Y:e.offsetY});
        
    }
    MouseMove(e) {
        if (this.IsDown) {
           

            this.PolygonPath(e.offsetX,e.offsetY);
        }
    }

    PolygonPath(x,y) {
        this.DrawItem = "";
        var len = this.PointList.length;
        for (var i in this.PointList) {
            var str = " L ";
            if (i == 0) {
                str = " M ";
            }
            var p = this.PointList[i];
            this.DrawItem += str + " " + p.X + " " + p.Y ;
           

        }

        this.DrawItem += " L " + x + " " + y + " Z";
    }


   


}
