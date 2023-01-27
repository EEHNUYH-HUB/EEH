import { GetRect,Guid } from "@/zenc/js/Common"

export default class MNDrawText{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "TEXT";
        this.IsDown = false;
        
        this.Text = "";
        this.DEFAULTW = 200;
        this.DEFAULTH = 40;
        
        this.ColorObj = colorobj;

    }
    
    MouseDown(e) {
        this.Enter(e.offsetX, e.offsetY);

    }
    Enter(x,y) {
         if (this.Text) {
            
            var obj = new Object;
            obj.Type = this.Name;
            obj.ID = Guid(); 
            obj.Text = this.Text;
            obj.StrokeColor = this.ColorObj .Stroke;
            obj.FillColor = this.ColorObj.Fill;
            obj.Rect = GetRect(this.X, this.Y , this.X+ this.DEFAULTW, this.Y + this.DEFAULTH);
            this.ObjList.push(obj)
            this.Eventer.AddedMethod(obj);
        }
        
        this.Text = "";
        this.IsDown = true;
        
        this.StartPoint = new Object;
        
        this.StartPoint.X = x;
        this.StartPoint.Y = y;

        this.X = this.StartPoint.X - 10;
        this.Y = this.StartPoint.Y - 20;
        this.W = this.DEFAULTW;
        this.H = this.DEFAULTH;
    }
    
    KeyUp(e) {
        
        if (e.key == 'Enter' ) {
            this.Enter(this.StartPoint.X,this.StartPoint.Y + this.DEFAULTH)
        }
        else if (e.key == 'ArrowLeft') {
            this.Enter(this.StartPoint.X-this.DEFAULTW,this.StartPoint.Y)
        }
         else if (e.key == 'ArrowRight') {
            this.Enter(this.StartPoint.X+this.DEFAULTW,this.StartPoint.Y )
        }
         else if (e.key == 'ArrowDown') {
            this.Enter(this.StartPoint.X,this.StartPoint.Y + this.DEFAULTH)
        } else if (e.key == 'ArrowUp') {
            this.Enter(this.StartPoint.X,this.StartPoint.Y - this.DEFAULTH)
        }
        else if (e.key == 'Tab') {
            this.Enter(this.StartPoint.X + this.DEFAULTW, this.StartPoint.Y)
            e.preventDefault();
        }
        else if (e.key == 'Escape') {
            this.IsDown = false;
            this.Text = "";
        }
    }
}
