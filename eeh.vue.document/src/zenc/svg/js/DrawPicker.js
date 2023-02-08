import { Guid } from "@/zenc/js/Common"
import { GetCenterforRect,GetIcon,GetOffsetPoint } from '@/zenc/svg/js/Common'

export default class MNDrawPicker {
    constructor() {
        this.ObjList = new Array;
        this.JoinList = new Array;
        this.Name = "PICK";
        this.IsDown = false;

        this.StartPoint = null;
        this.BeforeRotate = null;
        this.EndPoint = null;
        this.ColorObj = new Object;
        this.ColorObj.Stroke = "#000000FF";
        this.ColorObj.Fill = "#FFFFFF00";
        this.SelectedItems = new Array;

        this.SubRect = null;
        this.ChangeObj = null;
        this.IsDrawJoin = false;
        this.JoinItem = null;

        this.IsShowStandby = false;
        this.IsShowIcon = false;
        this.IsRightDown = false;
        this.SelectedItem = null;
    }

    MouseDown(eventArg) {
        
        var e = GetOffsetPoint(eventArg);
        this.IsDown = true;

        this.StartPoint = new Object;
        this.StartPoint.X =e.X;
        this.StartPoint.Y =e.Y;
        this.EndPoint = e;

        if (this.SelectedItems.length > 0) {
            for (var i in this.SelectedItems) {
                var obj = this.SelectedItems[i];

                if (obj && obj.Rect.SubObjs) {
                    for (var j in obj.Rect.SubObjs) {
                        var sub = obj.Rect.SubObjs[j];
                        if (sub.IsDown) {
                            this.SubRect = sub;

                            this.IsDrawJoin = this.SubRect.Type == "TC" || this.SubRect.Type == "BC" || this.SubRect.Type == "LC" || this.SubRect.Type == "RC";;
                            this.AllUnSelected()
                            obj.IsSelected = true;
                            this.ChangeObj = obj;
                            this.BeforeRotate = this.ChangeObj.Rect.Rotate;
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
            if (this.IsFind(item.Rect, e.X, e.Y)) {
                selItem = item;
                break;
            }
        }
        this.IsShowStandby = false;
        if (selItem) {

            if(this.SelectedItem == selItem){
                this.IsShowStandby = true;
            }

            this.SelectedItem = selItem;

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
            this.SelectedItem = null;
            this.AllUnSelected()
        }
    }
    MouseMove(eventArg) {
        
        var e = GetOffsetPoint(eventArg);
        if (this.IsDown && this.SelectedItems && this.SelectedItems.length > 0) {

            if (!this.IsDrawJoin) {
                var end =e;

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
                this.EndPoint =e ;
                this.DrawItem = this.GetDrawPathCurve(this.EndPoint);
            }
        }
        else if (this.IsDown) {
            this.EndPoint = e;
            

            this.DrawItem
                = " M" + this.StartPoint.X + " " + this.StartPoint.Y
                + " L" + this.EndPoint.X + " " + this.StartPoint.Y
                + " L" + this.EndPoint.X + " " + this.EndPoint.Y
                + " L" + this.StartPoint.X + " " + this.EndPoint.Y
                + " L" + this.StartPoint.X + " " + this.StartPoint.Y + " Z";
        }
    }

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
                    if (!this.IsShowIcon) {
                        this.IsShowIcon = true;
                        this.SubRect.IsDown = false;
                        return;
                    }
                    else {
                        this.AllUnSelected(true)
                    }
                }

            }
            else {
                this.AllUnSelected()

            }
        }
        this.IsShowIcon = false;

       this.Select();


        var xV = this.StartPoint.X - this.EndPoint.X;
        var yV = this.StartPoint.Y - this.EndPoint.Y;

        var isSize = (xV < 5 && xV > -5 && yV < 5 && yV > -5);
        if (this.SelectedItems.length > 0) {
            if (!isSize)
                this.IsShowStandby = false;

            this.IsShowIcon = false;
        }

        if(isSize && !this.IsShowStandby && !this.IsShowIcon){
            var isRight = false;

            if(eventArg){
                if ("which" in eventArg)
                    isRight = eventArg.which == 3;
                else if ("button" in eventArg)
                    isRight = eventArg.button == 2;
            }
            this.IsShowIcon = isRight;
        }
    }
    Select(){
        if (this.SelectedItems.length == 0) {
            for (var i in this.ObjList) {

                var item = this.ObjList[i];
                if (this.IsFind2(item.Rect, this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y)) {

                    item.IsSelected = true;
                    this.SelectedItems.push(item);

                }
            }
        }

    }
    DrawJoin(startObj, endObj, obj) {
        var sObj = this.GetJoinPoint(startObj, endObj.Rect, "ALL");
        var eObj = this.GetJoinPoint(endObj, sObj.P, sObj.IsH ? "LR" : "BT");
        var path = ""
        if (sObj.IsH)
            path = this.GetDrawPathCurveH(sObj.P, eObj.P);
        else
            path = this.GetDrawPathCurveV(sObj.P, eObj.P);

        if (!obj) {
            obj = new Object;
            obj.StartObj = startObj;
            obj.EndObj = endObj;
            obj.StrokeColor = this.ColorObj.Stroke;
            obj.FillColor = "none";
            obj.Path = path;
            if (!this.ChangeObj.JoinObjs) {
                this.ChangeObj.JoinObjs = new Array;
            }
            this.ChangeObj.JoinObjs.push(obj);

            if (!this.JoinItem.JoinObjs) {
                this.JoinItem.JoinObjs = new Array;
            }
            this.JoinItem.JoinObjs.push(obj);
            obj.StartType = "url(#Circle)";
            obj.EndType = "url(#Triangle)";
            obj.JoinType= "line";
            this.JoinList.push(obj);
            
        }
        else {
            obj.Path = path;
        }
    }

    DrawIcon(type, size) {

        var obj = GetIcon(type, this.EndPoint.X, this.EndPoint.Y, size, this.ColorObj);
        obj.ID = Guid();
        obj.IsShowDisplayName = true;
        this.ObjList.push(obj)
        

        if (this.IsDrawJoin) {
            this.JoinItem = obj
            this.MouseUp(null);
        }
    }

    AllUnSelected(isSubUnSelected) {
        for (var i in this.SelectedItems) {
            var item = this.SelectedItems[i];
            item.IsSelected = false;

            for (var i in item.Rect.SubObjs) {
                item.Rect.SubObjs[i].IsDown = false;
            }
        }
        this.SelectedItems = new Array;

        if(isSubUnSelected){
            if(this.JoinItem){

                this.JoinItem.IsSelected = false;
                for (var i in this.JoinItem.Rect.SubObjs) {
                    this.JoinItem.Rect.SubObjs[i].IsDown = false;
                }

                for (var i in this.ChangeObj.Rect.SubObjs) {
                    this.ChangeObj.Rect.SubObjs[i].IsDown = false;
                }
            }
            this.SubRect = null;
            this.ChangeObj = null;
            this.JoinItem = null;
            this.IsDrawJoin = false;
        }
    }

    SizeSvgObj(start, end) {

        var scalemode = true;

        if (this.SubRect.Type == "TR") {

            var cp = GetCenterforRect(this.ChangeObj.Rect);

            var cx = cp.X;
            var cy = cp.Y;
            var x = cx - end.X;
            var y = cy - end.Y;

            var r = - ((Math.atan(x / y) * (180 / Math.PI)));

            if (y < 0) {

                r = (180 + r)
            }


            this.ChangeObj.Rect.Rotate = r; //*(180/Math.PI);

        }
        else if (scalemode) {
            if (this.SubRect.Type.indexOf('R') != -1) {
                var newWidth = (this.ChangeObj.Rect.Width * this.ChangeObj.Rect.ScaleX) + end.X - start.X;
                this.ChangeObj.Rect.ScaleX = newWidth / this.ChangeObj.Rect.Width;
            }
            if (this.SubRect.Type.indexOf('L') != -1) {
                var newWidth = (this.ChangeObj.Rect.Width * this.ChangeObj.Rect.ScaleX) - (end.X - start.X);
                this.ChangeObj.Rect.X = end.X
                this.ChangeObj.Rect.ScaleX = newWidth / this.ChangeObj.Rect.Width;
            }
            if (this.SubRect.Type.indexOf('B') != -1) {
                var newHeight = (this.ChangeObj.Rect.Height * this.ChangeObj.Rect.ScaleY) + end.Y - start.Y;
                this.ChangeObj.Rect.ScaleY = newHeight / this.ChangeObj.Rect.Height;
            }
            if (this.SubRect.Type.indexOf('T') != -1) {
                var newHeight = (this.ChangeObj.Rect.Height * this.ChangeObj.Rect.ScaleY) - (end.Y - start.Y);

                this.ChangeObj.Rect.Y = end.Y
                this.ChangeObj.Rect.ScaleY = newHeight / this.ChangeObj.Rect.Height;
            }
        }
        else {
            var mw = end.X - start.X;
            var mh = end.Y - start.Y;
            var x = this.ChangeObj.Rect.X;
            var y = this.ChangeObj.Rect.Y;
            var w = this.ChangeObj.Rect.Width;
            var h = this.ChangeObj.Rect.Height;

            if (this.SubRect.Type.indexOf('R') != -1) {
                w = this.ChangeObj.Rect.Width + mw;

            }
            if (this.SubRect.Type.indexOf('L') != -1) {
                x = end.X;
                w = this.ChangeObj.Rect.Width - mw;


            }
            if (this.SubRect.Type.indexOf('B') != -1) {
                h = this.ChangeObj.Rect.Height + mh;

            }
            if (this.SubRect.Type.indexOf('T') != -1) {
                y = end.Y
                h = this.ChangeObj.Rect.Height - mh;
            }


            if (w <= 50) {
                this.ChangeObj.Rect.Width = 50;
                return
            }

            if (h <= 50) {
                this.ChangeObj.Rect.Height = 50;
                return;
            }
            this.ChangeObj.Rect.X = x;
            this.ChangeObj.Rect.Y = y;
            this.ChangeObj.Rect.Width = w;
            this.ChangeObj.Rect.Height = h;

        }
        
    }
    MoveSvgObj(start, end) {
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

            if (obj && obj.JoinObjs && obj.JoinObjs.length > 0) {

                for (var i in obj.JoinObjs) {
                    var jObj = obj.JoinObjs[i];
                    this.DrawJoin(jObj.StartObj, jObj.EndObj, jObj)
                }
            }

        }


        

    }

    ChangedColor(ColorObj) {
        if (this.SelectedItems && this.SelectedItems.length > 0) {
            for (var i in this.SelectedItems) {
                var obj = this.SelectedItems[i];
                obj.StrokeColor = ColorObj.Stroke;
                obj.FillColor = ColorObj.Fill;
            }
            
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
        
    }
    IsFind(rect, x, y) {
        if (rect && x && y) {
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
            var maxX = x1 > x2 ? x1 : x2;
            var maxY = y1 > y2 ? y1 : y2;
            var cp = GetCenterforRect(rect);

            return (cp.X > minX && cp.X < maxX && cp.Y > minY && cp.Y < maxY);
        }
        return false;
    }

    JoinObj(item) {
        if (item != this.ChangeObj) {
            if (this.IsDrawJoin) {
                item.IsSelected = true;
                this.JoinItem = item;
            }
        }
    }
    UnJoinObj(item) {
        if (item != this.ChangeObj) {
            if (this.IsDrawJoin) {
                item.IsSelected = false;
                this.JoinItem = null;
            }
        }
    }
    GetDrawPathCurve(endPoint) {


        var sObj = this.GetJoinPoint(this.ChangeObj, endPoint, "ALL");
        var e = new Object;
        e.X = endPoint.X;
        e.Y = endPoint.Y;
        if (this.JoinItem) {
            var eObj = this.GetJoinPoint(this.JoinItem, sObj.P, sObj.IsH ? "LR" : "BT");
            e.X = eObj.P.X;
            e.Y = eObj.P.Y;
        }
        if (sObj.IsH)
            return this.GetDrawPathCurveH(sObj.P, e);
        else
            return this.GetDrawPathCurveV(sObj.P, e);

    }

    GetJoinPoint(ChangeObj, endPoint, mode) {
        var x = endPoint.X;
        var y = endPoint.Y;
        var cx = ChangeObj.Rect.CX;
        var cy = ChangeObj.Rect.CY;
        var v = Math.atan2((y - cy), (x - cx)) * 180 / Math.PI + 90;
        if (v < 0) {
            v = v + 360
        }
        var findType = "";
        if (!mode || mode == "ALL") {
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
        else if (mode == "LR") {
            if (v > 0 && v <= 180) {
                findType = "RC";
            }
            else {
                findType = "LC";
            }
        }
        else if (mode == "BT") {
            if (v > 90 && v <= 270) {
                findType = "BC";
            }
            else {
                findType = "TC";
            }
        }
        var obj = new Object;
        obj.P = new Object;
        for (var j in ChangeObj.Rect.SubObjs) {
            var sub = ChangeObj.Rect.SubObjs[j];

            if (this.IsDrawJoin)
                sub.IsDown = sub.Type == findType;
            if (sub.Type == findType) {
                if (this.IsDrawJoin)
                    this.SubRect = sub;
                obj.P.X = ChangeObj.Rect.X + sub.cX;
                obj.P.Y = ChangeObj.Rect.Y + sub.cY;
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
            arry.push({ tag: 'C', x: obj.sX, y: obj.sY + h });
            arry.push({ tag: ',', x: obj.eX, y: obj.eY - h });

        } else if (obj.lineFlow == 'LRBT' || obj.lineFlow == 'RLBT') {
            arry.push({ tag: 'C', x: obj.sX, y: obj.sY - h });
            arry.push({ tag: ',', x: obj.eX, y: obj.eY + h });
        }
        arry.push({ tag: ',', x: obj.eX, y: obj.eY });

        var str = "";
        for (var i in arry) {
            var t = arry[i];
            str += " " + t.tag + " " + t.x + " " + t.y;
        }

        return str;
    }
    GetObj(startPoint, endPoint) {
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
            if (startPoint.Y == endPoint.Y) {
                obj.minY = obj.maxY = startPoint.Y;
            }
            else if (startPoint.Y > endPoint.Y) {
                obj.lineFlow += "BT";
                obj.minY = endPoint.Y;
                obj.maxY = startPoint.Y;
            }
            else {
                obj.lineFlow += "TB";
                obj.minY = startPoint.Y;
                obj.maxY = endPoint.Y;
            }
        }
        else if (startPoint.X < endPoint.X) {

            obj.minX = startPoint.X;
            obj.maxX = endPoint.X;
            obj.lineFlow = "LR";
            if (startPoint.Y == endPoint.Y) {
                obj.minY = obj.maxY = startPoint.Y;
            }
            else if (startPoint.Y > endPoint.Y) {
                obj.lineFlow += "BT";
                obj.minY = endPoint.Y;
                obj.maxY = startPoint.Y;
            }
            else {
                obj.lineFlow += "TB";
                obj.minY = startPoint.Y;
                obj.maxY = endPoint.Y;
            }
        }
        else {

            obj.minX = endPoint.X;
            obj.maxX = startPoint.X;
            obj.lineFlow = "RL";
            if (startPoint.Y == endPoint.Y) {
                obj.minY = obj.maxY = startPoint.Y;
            }
            else if (startPoint.Y > endPoint.Y) {
                obj.lineFlow += "BT";
                obj.minY = endPoint.Y;
                obj.maxY = startPoint.Y;
            }
            else {
                obj.lineFlow += "TB";
                obj.minY = startPoint.Y;
                obj.maxY = endPoint.Y;
            }
        }

        obj.w = obj.maxX - obj.minX;
        obj.h = obj.maxY - obj.minY;
        obj.cX = obj.minX + obj.w / 2;
        obj.cY = obj.minY + obj.h / 2;
        return obj;
    }
}
