import { GetRect,Guid } from "@/zenc/js/Common"

export default class MNDrawTriangle{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "TRIANGLE";
        this.IsDown = false;
        this.DrawItem = "";
        this.StartPoint = null;
        this.EndPoint = null;
        this.Ps = null;
        this.ColorObj = colorobj;
    }
    MouseUp(e) {
        if (this.IsDown) {
            this.DrawItem = "";
            this.IsDown = false;
            
            var obj = new Object;
            obj.Type = this.Name;
            obj.ID = Guid();
            
            obj.Ps = this.Ps;
            obj.StrokeColor = this.ColorObj .Stroke;
            obj.FillColor = this.ColorObj.Fill;
            obj.Rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
            //if(obj.Rect.Width>10 && obj.Rect.Height >10)
            this.ObjList.push(obj)
            this.Eventer.AddedMethod(obj);
            this.StartPoint = null;
            this.EndPoint = null;
            this.Ps = null;
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

            
            var isUP = this.EndPoint.Y < this.StartPoint.Y;
            
            var rect = GetRect(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
   

            var lu = new Object;
            var ru = new Object;
            var ld = new Object;
            var rd = new Object;

            lu.X = 0;
            lu.Y = 0;

            ru.X = rect.Width;
            ru.Y = 0;

            ld.X = 0;
            ld.Y = rect.Height;

            rd.X =  rect.Width;
            rd.Y =  rect.Height;


            
              if (isUP) {
                 this.Ps = [ld.X, ld.Y, rd.X, rd.Y,lu.X + rect.Width / 2,lu.Y ];
            }
            else {
                this.Ps = [lu.X, lu.Y, ru.X, ru.Y,ld.X + rect.Width / 2,ld.Y ];
                
            }

              this.DrawItem
                 = " M" + (this.Ps[0]+ rect.X ) + " " + (this.Ps[1] + rect.Y)
                 + " L" +(this.Ps[2]+ rect.X ) + " " + (this.Ps[3] + rect.Y)
                 + " L" + (this.Ps[4]+ rect.X ) + " " + (this.Ps[5] + rect.Y)+ " Z";
        }
    }
}
