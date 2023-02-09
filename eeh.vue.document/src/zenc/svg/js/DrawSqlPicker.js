import DrawPicker from '@/zenc/svg/js/DrawPicker'
import { GetRect} from '@/zenc/svg/js/Common'
export default class MNDrawSqlPicker extends DrawPicker {
    
    MouseUp(eventArg) {
        
        this.IsDown = false;
        this.DrawItem = "";
        

        if (this.SubRect) {

            if (this.IsDrawJoin) {
                if (this.JoinItem) {
                    this.DrawJoin(this.ChangeObj, this.JoinItem, null);
                    this.AllUnSelected(true)
                    this.IsDrawJoin = false;
                }
                else {
                    this.AllUnSelected(true)   
                }
            }
            else {
                this.AllUnSelected()
            }
        }
        this.Select();
       
    }
    
    DrawJoin(startObj, endObj, obj) {
        
        var sObj = this.GetJoinPoint(startObj, endObj.Rect, "ALL");
        var eObj = this.GetJoinPoint(endObj, sObj.P, sObj.IsH ? "LR" : "BT");
        var rect = GetRect(sObj.P.X,sObj.P.Y,eObj.P.X ,eObj.P.Y)

        
        sObj.P.X -= rect.CX ;
        sObj.P.Y -= rect.CY;
        eObj.P.X -= rect.CX ;
        eObj.P.Y -= rect.CY;
        var cp1 = new Object;
        cp1.X =  sObj.P.X > eObj.P.X ? 8:-8;
        cp1.Y = sObj.P.Y > eObj.P.Y ? 8:-8;
        var cp2 = new Object;
        cp2.X = sObj.P.X > eObj.P.X ? -8:8;
        cp2.Y = sObj.P.Y > eObj.P.Y ? -8:8;
        var path = ""
        var path2 = ""
        

        
        
        if (sObj.IsH){
            path = this.GetDrawPathCurveH(sObj.P, cp1);
            path2 = this.GetDrawPathCurveH(cp2, eObj.P);
        }
        else{
            path = this.GetDrawPathCurveV(sObj.P, cp1);
            path2 = this.GetDrawPathCurveV(cp2, eObj.P);
        }

        if (!obj) {
            obj = new Object;
            obj.StartObj = startObj;
            obj.EndObj = endObj;
            obj.StrokeColor = this.ColorObj.Stroke;
            obj.Rect= rect;
            obj.FillColor = "none";
            
            if (!this.ChangeObj.JoinObjs) {
                this.ChangeObj.JoinObjs = new Array;
            }
            this.ChangeObj.JoinObjs.push(obj);

            if (!this.JoinItem.JoinObjs) {
                this.JoinItem.JoinObjs = new Array;
            }
            this.JoinItem.JoinObjs.push(obj);
            obj.StartType = "url(#Circle)";
            obj.EndType = "url(#Circle)";
            obj.JoinType = "sql";
            this.JoinList.push(obj);
            
        }
      
        obj.ColumnSP = cp1;  
        obj.ColumnEP = cp2;
            obj.Path = path;
            obj.Path2 = path2;
            obj.Rect= rect;
      
    }
}