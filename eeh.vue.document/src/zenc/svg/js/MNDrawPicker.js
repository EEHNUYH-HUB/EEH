import { GetRect ,GetCenterforRect, Guid} from "@/zenc/js/Common"

export default class MNDrawPicker{
    constructor(list,jlist,colorobj, eventer) {
        this.Eventer = eventer;
        this.ObjList = list;
        this.JoinList = jlist;
        this.Name = "PICK";
        this.IsDown = false;
        
        this.StartPoint = null;
        this.BeforeRotate = null;
        this.EndPoint = null;
        this.ColorObj = colorobj;
        this.SelectedItems = new Array;

        this.SubRect = null;
        this.SelectObj = null;
        this.IsDrawJoin = false;
        this.JoinItem = null;
        

    }
    
    MouseDown(e) {
        this.IsDown = true;
        
        
        
        this.StartPoint = new Object;
        this.StartPoint.X = e.offsetX;
        this.StartPoint.Y = e.offsetY;
        this.EndPoint = new Object;
        this.EndPoint.X = e.offsetX;
        this.EndPoint.Y = e.offsetY;

        //this.AllUnSelected()

        if (this.SelectedItems.length > 0) {
            for (var i in this.SelectedItems){
                var obj = this.SelectedItems[i];
                
                if (obj && obj.Rect.SubObjs) {
                    for (var j in obj.Rect.SubObjs) {
                        var sub = obj.Rect.SubObjs[j];
                        if (sub.IsDown) {
                            this.SubRect = sub;
                            
                            this.IsDrawJoin = this.SubRect.Type == "TC" || this.SubRect.Type == "BC" || this.SubRect.Type == "LC" || this.SubRect.Type == "RC";;
                            this.AllUnSelected()
                            obj.IsSelected = true;
                            this.SelectObj = obj;
                            this.BeforeRotate = this.SelectObj.Rect.Rotate;
                            this.SelectedItems.push(obj);
                            return;
                        }
                    }
                }
            }
        }

        var selItem = null;
        for (var i in this.ObjList) {

            var item = this.ObjList[i];
            if (this.IsFind(item.Rect,e.offsetX, e.offsetY)) {
                selItem = item;  
                break;
            }
        }

        if (selItem) {
            if (this.SelectedItems.length > 1) {
                if (!selItem.IsSelected) {
                    this.AllUnSelected() 
                selItem.IsSelected = true;
                this.SelectedItems.push(selItem);
                }
            }
            else {
                this.AllUnSelected() 
                selItem.IsSelected = true;
                this.SelectedItems.push(selItem);
            }
                
        }
        else {
            this.AllUnSelected() 
        }
        

    }
    AllUnSelected() {
        for (var i in this.SelectedItems) {
            this.SelectedItems[i].IsSelected = false;
        }
        this.SelectedItems = new Array;
    }
   
    MouseMove(e) {
        if (this.IsDown && this.SelectedItems && this.SelectedItems.length > 0) {

            if (!this.IsDrawJoin) {
                var end = new Object;
                end.X = e.offsetX;
                end.Y = e.offsetY;

                if (this.SubRect) {
                    this.SizeSvgObj(this.StartPoint, end)
                }
                else {
                    this.MoveSvgObj(this.StartPoint, end)
                }

                this.StartPoint.X = end.X;
                this.StartPoint.Y = end.Y;
            }
            else {
                this.EndPoint.X = e.offsetX;
                this.EndPoint.Y = e.offsetY;

                
                this.DrawItem = this.GetDrawPathCurve( this.EndPoint);
                
                    
            }
        }
        else if (this.IsDown) {
            this.EndPoint.X = e.offsetX;
            this.EndPoint.Y = e.offsetY;

            this.DrawItem
                = " M" + this.StartPoint.X + " " + this.StartPoint.Y
                + " L" + this.EndPoint.X + " " + this.StartPoint.Y
                + " L" + this.EndPoint.X + " " + this.EndPoint.Y
                + " L" + this.StartPoint.X + " " + this.EndPoint.Y
                + " L" + this.StartPoint.X + " " + this.StartPoint.Y + " Z";
        }
    }
    DrawJoin(startObj,endObj,obj){
        var sObj = this.GetJoinPoint(startObj, endObj.Rect,"ALL");
        var eObj = this.GetJoinPoint(endObj, sObj.P,sObj.IsH?"LR":"BT");
        var path = ""
        if (sObj.IsH)
            path = this.GetDrawPathCurveH(sObj.P, eObj.P);
        else
            path = this.GetDrawPathCurveV(sObj.P, eObj.P);

        if(!obj)
        {
            obj = new Object;   
            obj.StartObj = startObj;
            obj.EndObj = endObj;
            obj.StrokeColor= this.ColorObj .Stroke;
            obj.FillColor = "none";
            obj.Path = path;
            if(!this.SelectObj.JoinObjs){
                this.SelectObj.JoinObjs = new Array;
            }
            this.SelectObj.JoinObjs.push(obj);
            
            if(!this.JoinItem.JoinObjs){
                this.JoinItem.JoinObjs = new Array;
            }
            this.JoinItem.JoinObjs.push(obj);
            
            this.JoinList.push(obj);
            this.Eventer.AddedMethod(obj);
        }
        else{
            obj.Path = path;
        }
    }
    MouseUp(e) {
        this.IsDown = false;
        this.DrawItem = "";
        if (this.SubRect) {
            
            if(this.IsDrawJoin){
                if (this.JoinItem) {
                    this.DrawJoin(this.SelectObj, this.JoinItem, null);

                    this.JoinItem.IsSelected = false;
                    for (var i in this.JoinItem.Rect.SubObjs) {
                        this.JoinItem.Rect.SubObjs[i].IsDown = false;
                    }

                    for (var i in this.SelectObj.Rect.SubObjs) {
                        this.SelectObj.Rect.SubObjs[i].IsDown = false;
                    }
                }
                this.AllUnSelected()
            }

            this.IsDrawJoin = false;
            this.SubRect.IsDown = false;
            this.SubRect = null;
            this.JoinItem = null;
            this.SelectObj = null;
        }
        
        if(this.SelectedItems.length == 0){
            for (var i in this.ObjList) {

                var item = this.ObjList[i];
                if (this.IsFind2(item.Rect,this.StartPoint.X , this.StartPoint.Y ,this.EndPoint.X ,this.EndPoint.Y )) {
                    
                    item.IsSelected = true;
                    this.SelectedItems.push(item);
                    
                }
            }
        }

        this.Eventer.SelectedMethod(this.SelectedItems);
    }
    SizeSvgObj(start, end) {

        var scalemode = true;
        
        if (this.SubRect.Type == "TR") {


            
            var cp = GetCenterforRect(this.SelectObj.Rect);
            
            var cx = cp.X;
            var cy = cp.Y;
            var x = cx - end.X;
            var y = cy - end.Y;
            
            var r = - ((Math.atan(x / y) * (180 / Math.PI)));
            
            if (y < 0) { 
                
                r = (180 + r)
            }

            
            this.SelectObj.Rect.Rotate =  r; //*(180/Math.PI);
            
        }
        else if(scalemode){
            if (this.SubRect.Type.indexOf('R') != -1) {
                var newWidth = (this.SelectObj.Rect.Width * this.SelectObj.Rect.ScaleX) + end.X - start.X;
                this.SelectObj.Rect.ScaleX = newWidth / this.SelectObj.Rect.Width;
            }
            if (this.SubRect.Type.indexOf('L') != -1) {
                var newWidth = (this.SelectObj.Rect.Width * this.SelectObj.Rect.ScaleX) - (end.X - start.X);
                this.SelectObj.Rect.X = end.X
                this.SelectObj.Rect.ScaleX = newWidth / this.SelectObj.Rect.Width;
            }
            if (this.SubRect.Type.indexOf('B') != -1) {
                var newHeight = (this.SelectObj.Rect.Height * this.SelectObj.Rect.ScaleY) + end.Y - start.Y;
                this.SelectObj.Rect.ScaleY = newHeight / this.SelectObj.Rect.Height;
            }
            if (this.SubRect.Type.indexOf('T') != -1) {
                var newHeight = (this.SelectObj.Rect.Height * this.SelectObj.Rect.ScaleY) - (end.Y - start.Y);
            
                this.SelectObj.Rect.Y = end.Y
                this.SelectObj.Rect.ScaleY = newHeight / this.SelectObj.Rect.Height;
            }
        }
        else{
            var mw = end.X - start.X;
            var mh = end.Y - start.Y;
            var x = this.SelectObj.Rect.X;
            var y = this.SelectObj.Rect.Y;
            var w = this.SelectObj.Rect.Width;
            var h = this.SelectObj.Rect.Height;
            
            if (this.SubRect.Type.indexOf('R') != -1) {
                w = this.SelectObj.Rect.Width + mw;
                
            }
            if (this.SubRect.Type.indexOf('L') != -1) {
                x = end.X;
                w = this.SelectObj.Rect.Width - mw;
               
                
            }
            if (this.SubRect.Type.indexOf('B') != -1) {
                h = this.SelectObj.Rect.Height + mh;
               
            }
            if (this.SubRect.Type.indexOf('T') != -1) {
                y = end.Y
                h = this.SelectObj.Rect.Height - mh;
            }


            if(w <=50){
                this.SelectObj.Rect.Width = 50;
               return
            }
            
            if(h <= 50){
                this.SelectObj.Rect.Height = 50;
                return;
             }
            this.SelectObj.Rect.X = x;
            this.SelectObj.Rect.Y = y;
            this.SelectObj.Rect.Width = w;
            this.SelectObj.Rect.Height = h;
            
        }
         this.Eventer.ChangedMethod(this.SelectedItems);
    }
    MoveSvgObj(start,end) {
        for (var i in this.SelectedItems) {
            var obj = this.SelectedItems[i];
            var x = end.X - start.X;
            var y = end.Y - start.Y;
            obj.Rect.X = obj.Rect.X + x;
            obj.Rect.Y = obj.Rect.Y + y;
            obj.Rect.X2 = obj.Rect.X2 + x;
            obj.Rect.Y2 = obj.Rect.Y2 + y;
            obj.Rect.CX = obj.Rect.CX + x;
            obj.Rect.CY = obj.Rect.CY + y;

            if(obj && obj.JoinObjs && obj.JoinObjs.length > 0){
            
                for(var  i in obj.JoinObjs){
                    var jObj = obj.JoinObjs[i];
                    this.DrawJoin(jObj.StartObj,jObj.EndObj,jObj)
                }
            }

        }
        
       
        this.Eventer.ChangedMethod(this.SelectedItems);
        
    }

    ChangedColor(ColorObj) {
        if (this.SelectedItems && this.SelectedItems.length > 0) {
            for (var i in this.SelectedItems) {
                var obj = this.SelectedItems[i];
                obj.StrokeColor = ColorObj.Stroke;
                obj.FillColor = ColorObj.Fill;
            }
            this.Eventer.ChangedMethod(this.SelectedItems);
        }
    }

    DeleteSvgObj() {
        var deleteItems = new Array;
        if (this.SelectedItems && this.SelectedItems.length > 0) {
            for (var i in this.SelectedItems) {
                var obj = this.SelectedItems[i];
                obj.IsSelected = false;
                
                for (var j in this.ObjList) {
                    if (this.ObjList[j].ID == obj.ID) {
                        this.ObjList.splice(j, 1);
                        deleteItems.push(obj);            
                        break;
                    }
                }
                
            }
        }
        
        this.AllUnSelected()    
        this.Eventer.RemovedMethod(deleteItems);
    }
    IsFind(rect, x, y) {
        if (rect  && x && y) {
            var x1 = rect.X;
            var y1 = rect.Y;
            var x2 = (rect.Width * rect.ScaleX) + rect.X;
            var y2 = (rect.Height * rect.ScaleY) + rect.Y;
        
            var minX = x1 > x2 ? x2 : x1;
            var minY = y1 > y2 ? y2 : y1;
            var maxX = x1 > x2 ? x1 : x2;
            var maxY = y1 > y2 ? y1 : y2;
        
            return (minX < x && maxX > x && minY < y && maxY > y);
        }
        return false;
    }
    IsFind2(rect, x1, y1, x2, y2) {
        if (rect && x1 && y1 && x2 && y2) {
            
            var minX = x1 > x2 ? x2 : x1;
            var minY = y1 > y2 ? y2 : y1;
            var maxX= x1 > x2 ? x1  : x2 ;
            var maxY= y1 > y2 ? y1  : y2 ;    
            var cp = GetCenterforRect(rect);
        
            return (cp.X > minX && cp.X < maxX && cp.Y > minY && cp.Y < maxY);
        }
        return false;
    }

    JoinObj(item){
        if(item != this.SelectObj){
            if(this.IsDrawJoin){
                item.IsSelected = true;
                this.JoinItem = item;
            }
        }
    }
    UnJoinObj(item){
        if(item != this.SelectObj){
            if(this.IsDrawJoin){
                item.IsSelected = false;
                this.JoinItem = null;
            }
        }
    }
    GetDrawPathCurve( endPoint){
        

        var sObj = this.GetJoinPoint(this.SelectObj, endPoint,"ALL");
        var e = new Object;
        e.X = endPoint.X;
        e.Y = endPoint.Y;
        if(this.JoinItem){
            var eObj = this.GetJoinPoint(this.JoinItem, sObj.P,sObj.IsH?"LR":"BT");
            e.X = eObj.P.X;
            e.Y = eObj.P.Y;
        }
        if (sObj.IsH)
            return this.GetDrawPathCurveH(sObj.P, e);
        else
            return this.GetDrawPathCurveV(sObj.P, e);

    }
    
    GetJoinPoint(selectObj,endPoint,mode){
        var x = endPoint.X;
        var y = endPoint.Y;
        var cx = selectObj.Rect.CX;
        var cy = selectObj.Rect.CY;
        var v = Math.atan2((y - cy), (x - cx)) * 180 / Math.PI + 90;
        if (v < 0) {
            v = v + 360
        }
        var findType = "";
        if(!mode || mode == "ALL"){
            if (v > 45 && v <= 135) {
                findType = "RC";
            }
            else if (v > 135 && v <= 225) {
                findType = "BC";
            }
            else if (v > 225 && v <= 315) {
                findType = "LC";
            }
            else {
                findType = "TC";
            }
        }
        else if(mode == "LR"){
            if (v > 0 && v <= 180) {
                findType = "RC";
            }
            else  {
                findType = "LC";
            }
        }
        else if(mode == "BT"){
            if (v > 90 && v <= 270) {
                findType = "BC";
            }
            else {
                findType = "TC";
            }
        }
        var obj = new Object;
        obj.P = new Object;
        for (var j in selectObj.Rect.SubObjs) {
            var sub = selectObj.Rect.SubObjs[j];

            if(this.IsDrawJoin)
                sub.IsDown = sub.Type == findType;
            if (sub.Type == findType) {
                if(this.IsDrawJoin)
                    this.SubRect = sub;
                obj.P.X = selectObj.Rect.X + sub.cX;
                obj.P.Y = selectObj.Rect.Y + sub.cY;
                obj.IsH = findType == "RC" || findType == "LC";

            }

        }

        return obj;
    
    }
    GetDrawPathCurveH(startPoint, endPoint) {
        var obj = this.GetObj(startPoint, endPoint);
        var sub = 2
        var w = obj.w / sub;
        var h = obj.h / sub;
        var arry = new Array;
        arry.push({ tag: 'M', x: obj.sX, y: obj.sY });
        if (obj.lineFlow == 'LRTB' || obj.lineFlow == 'LRBT') {
            arry.push({ tag: 'C', x: obj.sX + w, y: obj.sY });
            arry.push({ tag: ',', x: obj.eX - w, y: obj.eY });

        } else if (obj.lineFlow == 'RLTB' || obj.lineFlow == 'RLBT') {
            arry.push({ tag: 'C', x: obj.sX - w, y: obj.sY });
            arry.push({ tag: ',', x: obj.eX + w, y: obj.eY });
        } 

        arry.push({ tag: ',', x: obj.eX, y: obj.eY });

        var str = "";
        for (var i in arry) {
            var t = arry[i];
            str += " " + t.tag + " " + t.x + " " + t.y;
        }
        
        return str;
    }
    GetDrawPathCurveV(startPoint, endPoint) {
        var obj = this.GetObj(startPoint, endPoint);
        var sub = 2
        var w = obj.w / sub;
        var h = obj.h / sub;
        var arry = new Array;
        arry.push({ tag: 'M', x: obj.sX, y: obj.sY });
        if (obj.lineFlow == 'LRTB' || obj.lineFlow == 'RLTB') {
            arry.push({ tag: 'C', x: obj.sX , y: obj.sY + h});
            arry.push({ tag: ',', x: obj.eX , y: obj.eY - h});

        } else if (obj.lineFlow == 'LRBT' || obj.lineFlow == 'RLBT') {
            arry.push({ tag: 'C', x: obj.sX , y: obj.sY - h });
            arry.push({ tag: ',', x: obj.eX , y: obj.eY + h});
        } 
        arry.push({ tag: ',', x: obj.eX, y: obj.eY });

        var str = "";
        for (var i in arry) {
            var t = arry[i];
            str += " " + t.tag + " " + t.x + " " + t.y;
        }

        return str;
    }
    GetObj (startPoint,endPoint){
        var obj = new Object;
        
        obj.minX = -1;
        obj.minY = -1;
        obj.maxX = -1;
        obj.maxY = -1;
        obj.w = -1;
        obj.h = -1;
        obj.cX = -1;
        obj.cY = -1;
        obj.sX = startPoint.X;
        obj.sY = startPoint.Y;
        obj.eX = endPoint.X;
        obj.eY = endPoint.Y;
        obj.lineFlow = "";
        if (startPoint.X == endPoint.X) {
            obj.minX = obj.maxX = startPoint.X;
            if(startPoint.Y == endPoint.Y){
                obj.minY = obj.maxY  =startPoint.Y;
            }
            else if (startPoint.Y > endPoint.Y) {
                obj.lineFlow +="BT";
                obj.minY = endPoint.Y;
                obj.maxY = startPoint.Y;
            }
            else {
                obj.lineFlow +="TB";
                obj.minY = startPoint.Y;
                obj.maxY = endPoint.Y;
            }
        }
        else if (startPoint.X < endPoint.X) {
            
            obj.minX = startPoint.X;
            obj.maxX = endPoint.X;
            obj.lineFlow = "LR";
            if(startPoint.Y == endPoint.Y){
                obj.minY = obj.maxY  =startPoint.Y;
            }
            else if (startPoint.Y > endPoint.Y) {
                obj.lineFlow +="BT";
                obj.minY = endPoint.Y;
                obj.maxY = startPoint.Y;
            }
            else {
                obj.lineFlow +="TB";
                obj.minY = startPoint.Y;
                obj.maxY = endPoint.Y;
            }
        }
        else {
            
            obj.minX = endPoint.X;
            obj.maxX = startPoint.X;
            obj.lineFlow = "RL";
            if(startPoint.Y == endPoint.Y){
                obj.minY = obj.maxY  =startPoint.Y;
            }
            else if (startPoint.Y > endPoint.Y) {
                obj.lineFlow +="BT";
                obj.minY = endPoint.Y;
                obj.maxY = startPoint.Y;
            }
            else {
                obj.lineFlow +="TB";
                obj.minY = startPoint.Y;
                obj.maxY = endPoint.Y;
            }
        }
    
        obj.w = obj.maxX - obj.minX;
        obj.h = obj.maxY - obj.minY;
        obj.cX = obj.minX + obj.w /2;
        obj.cY = obj.minY + obj.h /2;
        return obj;
    }
}
