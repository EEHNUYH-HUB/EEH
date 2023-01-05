export function ConvertJsonToKeyValueObject(json){
    var property =Object.getOwnPropertyNames(json);
    
    var rtn = new Array;
    for(var i in property){
        var pName = property[i];
         rtn.push({key: pName ,value:json[pName]});
    }
    return rtn;
}

export function ConvertKeyValueObjectToObject(obj){
    if(obj && obj.length > 0 && obj[0].key ){
        var rtn = new Object;
        for(var i in obj){
        rtn[obj[i].key] = obj[i].value;
        }
        return rtn;
    }
    return new Object;

}

export function GetColumnsforDataTable(dt){
    var rtn = new Array;
    if(dt && dt.length > 0){
        var obj = ConvertJsonToKeyValueObject(dt[0]);
        for(var i in obj){
            rtn.push(obj[i].key);
        }
    }
    return rtn;
}

export function Guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}