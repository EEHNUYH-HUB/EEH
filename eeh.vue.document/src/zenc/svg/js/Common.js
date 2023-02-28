import { Guid } from "@/zenc/js/Common"
export function GetIcons(colorObj, type, obj) {

    var rtn = new Array;
    var grp = new Object;
    grp.DisplayName = "ICONS";
    grp.Items = new Array;
    rtn.push(grp);
    if (type == 'none') {
        grp.Items.push(GetIcon('database', 8, 8, 16, colorObj));
        grp.Items.push(GetIcon('diagram', 8, 8, 16, colorObj));
        grp.Items.push(GetIcon('server', 8, 8, 16, colorObj));
        grp.Items.push(GetIcon('chat', 8, 8, 16, colorObj));
        grp.Items.push(GetIcon('diamond', 8, 8, 16, colorObj));

    }
    else if (type == 'database') {
        grp.Items.push(GetIcon('table', 8, 8, 16, colorObj));
    }
    else if (type == 'table') {

        grp.Items.push(GetIcon('sql', 8, 8, 16, colorObj));

        grp = new Object;
        grp.DisplayName = "Foreign Table";
        grp.Items = new Array;

        var isFirst = true;
        if (obj.Columns) {
            for (var i in obj.Columns) {
                var col = obj.Columns[i];
                if (col && col.obj && col.obj.foreign_table_name && col.obj.foreign_column_name) {

                    var tmp = new Array;
                    if (obj.JoinObjs) {
                        for (var j in obj.JoinObjs) {
                            var join = obj.JoinObjs[j];
                            if (join && join.StartObj && join.EndObj) {
                                var v = join.StartJoinColumn + join.StartObj.TableName + join.EndJoinColumn + join.EndObj.TableName;
                                var v2 = join.EndJoinColumn + join.EndObj.TableName + join.StartJoinColumn + join.StartObj.TableName;
                                tmp.push(v);
                                tmp.push(v2);
                            }
                        }
                    }

                    var chk = col.obj.column_name + obj.TableName + col.obj.foreign_column_name + col.obj.foreign_table_name;
                    var isNext = false;
                    for (var k in tmp) {
                        if (tmp[k] == chk) {
                            isNext = true;
                            break;
                        }
                    }
                    if (!isNext) {
                        var icon = GetIcon('table', 8, 8, 16, colorObj);
                        icon.DisplayName = col.obj.foreign_table_name;
                        icon.TableName = col.obj.foreign_table_name;
                        icon.IsShowDisplayName = true;

                        grp.Items.push(icon);
                        if (isFirst) {
                            rtn.push(grp);
                            isFirst = false;
                        }
                    }

                }
            }

        }
    }
    else if (type == 'sql') {
        rtn.push(GetIcon('sql', 8, 8, 16, colorObj));
    }

    return rtn;
}
export function GetIcon(type, x, y, size, colorObj) {
    var obj = new Object;
    obj.ID = Guid();
    obj.IconType = type;
    obj.Type = "ICON";
    obj.IsShowDisplayName = false;
    obj.StrokeColor = colorObj.Stroke;
    obj.FillColor = colorObj.Fill;
    obj.Rect = GetRect(x - size / 2, y - size / 2, x + size / 2, y + size / 2);

    return obj;
}
export function GetIcon2(type, x, y, w, h, colorObj) {
    var obj = new Object;
    obj.IconType = type;
    obj.Type = "ICON";
    obj.IsShowDisplayName = false;
    obj.StrokeColor = colorObj.Stroke;
    obj.FillColor = colorObj.Fill;
    obj.Rect = GetRect(x - w / 2, y - h / 2, x + w / 2, y + h / 2);

    return obj;
}

export function GetDatabaseInfo(item) {


    if (item.IconType == "database") {
        var rtn = new Object;
        rtn.DBType = item.DBType;
        rtn.Server = item.Server;
        rtn.Port = item.Port;
        rtn.DatabaseName = item.DatabaseName;
        rtn.UserID = item.UserID;
        rtn.Password = item.Password;
        return rtn;
    }
    else if (item.JoinObjs && item.JoinObjs.length > 0) {
        var joins = item.JoinObjs;
        if (joins) {
            for (var i in joins) {
                var beforeItem = joins[i].StartObj;
                var rtn = GetDatabaseInfo(beforeItem);
                if (rtn != null) return rtn
            }
        }
    }

    return null;
}


export function GetOffsetPoint(e) {
    var rtn = new Object;
    rtn.X = e.offsetX;
    rtn.Y = e.offsetY;
    if (e.touches) {
        rtn.X = e.touches[0].clientX;
        rtn.Y = e.touches[0].clientY;
    }
    return rtn;
}


export function GetCenterforRect(rect) {
    var p = new Object;
    p.X = rect.Width / 2 + rect.X;
    p.Y = rect.Height / 2 + rect.Y;
    return p;
}


export function GetRect(x1, y1, x2, y2) {
    var rect = new Object;

    var minX = x1 > x2 ? x2 : x1;
    var minY = y1 > y2 ? y2 : y1;
    var maxX = x1 > x2 ? x1 : x2;
    var maxY = y1 > y2 ? y1 : y2;

    rect.X = minX;
    rect.Y = minY;
    rect.X2 = maxX;
    rect.Y2 = maxY;

    rect.Width = maxX - rect.X;
    rect.Height = maxY - rect.Y;
    rect.CX = minX + rect.Width / 2;
    rect.CY = minY + rect.Height / 2;

    rect.ScaleX = 1;
    rect.ScaleY = 1;
    rect.Rotate = 0;
    rect.SubObjs = new Array;

    var size = 5;

    var joinPoint = 15;
    var joinSize = 4;

    //rect.SubObjs.push(GetSubObj(rect.Width / 2 - 5, -30, size, "TR"));//top_roate


    rect.SubObjs.push(GetSubObj(-joinPoint, rect.Height / 2, joinSize, "LC", "white", "#96C6DE"));
    rect.SubObjs.push(GetSubObj(rect.Width + joinPoint, rect.Height / 2, joinSize, "RC", "white", "#96C6DE"));
    rect.SubObjs.push(GetSubObj(rect.Width / 2, -joinPoint, joinSize, "TC", "white", "#96C6DE"));
    rect.SubObjs.push(GetSubObj(rect.Width / 2, rect.Height + joinPoint, joinSize, "BC", "white", "#96C6DE"));

    //  rect.SubObjs.push(GetSubObj( 0,0,size,"LT","#96C6DE","white"));
    //  rect.SubObjs.push(GetSubObj( rect.Width,0,size,"RT","#96C6DE","white"));
    //  rect.SubObjs.push(GetSubObj( 0,rect.Height,size,"LB","#96C6DE","white"));
    //  rect.SubObjs.push(GetSubObj(rect.Width, rect.Height, size, "RB","#96C6DE","white"));


    function GetSubObj(x, y, r, type, s, f) {
        var obj = new Object;

        obj.R = r;
        obj.cX = x;
        obj.cY = y;
        obj.S = s;
        obj.F = f;
        obj.Type = type;

        return obj;
    }


    return rect;
}

export function ExcelCellName(index) {
    var arry = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
    var len = arry.length;

    var share = Math.floor((index / len + 1));
    var rest = index % len;
    var rtn = '';

    for (var i = 0; i < share; i++) {
        rtn += arry[rest];
    }
    return rtn;
}