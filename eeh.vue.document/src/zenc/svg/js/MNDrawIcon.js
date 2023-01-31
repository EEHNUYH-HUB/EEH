import { GetRect } from "@/zenc/js/Common"

export  function GetIcons(colorObj){
    var icons = ['chat', 'circle', 'database', 'diagram', 'diamond', 'email', 'fonts', 'listTask', 'pc', 'server', 'square', 'table', 'triangle']
    var rtn = new Array;
    
    for(var i in icons){
        rtn.push(GetIcon(icons[i],8,8,16,colorObj));
    }
    return rtn;
}
export  function GetIcon(type,x,y,size ,colorObj){
    var obj = new Object;
    obj.IconType = type;
    obj.Type = "ICON";
    obj.IsShowDisplayName = false;
    obj.StrokeColor = colorObj .Stroke;
    obj.FillColor = colorObj.Fill;
    obj.Rect = GetRect(x-size/2, y-size/2, x+size/2, y+size/2);
    return obj;
}



