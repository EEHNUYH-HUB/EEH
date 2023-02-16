import DrawPicker from '@/zenc/svg/js/DrawPicker'
import { GetRect } from '@/zenc/svg/js/Common'
export default class MNDrawSqlPicker extends DrawPicker {


    DrawJoin(startObj, endObj, jobj) {
        
        var sObj = this.GetJoinPoint(startObj, endObj.Rect, "ALL");
        var eObj = this.GetJoinPoint(endObj, sObj.P, sObj.IsH ? "LR" : "BT");
        var rect = GetRect(sObj.P.X, sObj.P.Y, eObj.P.X, eObj.P.Y)


        sObj.P.X -= rect.CX;
        sObj.P.Y -= rect.CY;
        eObj.P.X -= rect.CX;
        eObj.P.Y -= rect.CY;

        var cp1 = new Object;
        cp1.X = sObj.P.X > eObj.P.X ? 8 : -8;
        cp1.Y = sObj.P.Y > eObj.P.Y ? 8 : -8;
        var cp2 = new Object;
        cp2.X = sObj.P.X > eObj.P.X ? -8 : 8;
        cp2.Y = sObj.P.Y > eObj.P.Y ? -8 : 8;
        var path = ""
        var path2 = ""




        if (sObj.IsH) {
            path = this.GetDrawPathCurveH(sObj.P, cp1);
            path2 = this.GetDrawPathCurveH(cp2, eObj.P);
        }
        else {
            path = this.GetDrawPathCurveV(sObj.P, cp1);
            path2 = this.GetDrawPathCurveV(cp2, eObj.P);
        }

        if (!jobj) {
            if(this.IsJoin(startObj,endObj))return;
            jobj = new Object;
            jobj.StartObj = startObj;
            jobj.EndObj = endObj;
            jobj.StrokeColor = this.ColorObj.Stroke;
            jobj.Rect = rect;
            jobj.FillColor = "none";

            if (!startObj.JoinObjs) {
                startObj.JoinObjs = new Array;
            }
            startObj.JoinObjs.push(jobj);

            if (!endObj.JoinObjs) {
                endObj.JoinObjs = new Array;
            }
            endObj.JoinObjs.push(jobj);
            jobj.StartType = "url(#Circle)";
            jobj.EndType = "url(#Circle)";
            jobj.JoinType = "sql";
            this.JoinList.push(jobj);

        }

    
        jobj.ColumnSP = new Object;
        jobj.ColumnEP = new Object;
        
        if(sObj.P.X < eObj.P.X){
            jobj.ColumnSP.X = -12;
            jobj.ColumnSP.Y = -17;
            jobj.ColumnSP.W = 0;
            jobj.ColumnEP.X = 12;
            jobj.ColumnEP.Y = -17;
            jobj.ColumnEP.W = 200;
        }
        else{
            jobj.ColumnEP.X = -12;
            jobj.ColumnEP.Y = -17;
            jobj.ColumnEP.W = 0;
            jobj.ColumnSP.X = 12;
            jobj.ColumnSP.Y = -17;
            jobj.ColumnSP.W = 200;
        }
    
        
        jobj.Path = path;
        jobj.Path2 = path2;
        jobj.Rect = rect;
        return jobj;

    }
}