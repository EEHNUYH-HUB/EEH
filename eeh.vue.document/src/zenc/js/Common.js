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
    if (obj && obj.length > 0 && obj[0].key) {
        var rtn = new Object;
        for (var i in obj) {
            rtn[obj[i].key] = obj[i].value;
        }
        return rtn;
    }
    if (obj)
        return obj;
    else
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


export function GetBirthDay(str){
    
    return str.replace(/(\d{4})(\d{2})(\d{2})/g, '$1년$2월$3일');
    
}

export function GetPhoneNumber(str){
    
    return str.replace(/(\d{3})(\d{4})(\d{4})/g, '$1-$2-$3');
    
}

export function OnlyAllowNumber(value) { return !value || /^\d+$/.test(value);}
export function OnlyAllowDouble(value) { return !value || /^\d{0,22}(\.\d{0,22}){0,1}$/.test(value);}
export function ConvertDateToYYYYMMDD(date){
    var strYear =String(date.getFullYear());
    var strMonth =String(date.getMonth() + 1);
    var strDay =String(date.getDate());
    if(strMonth.length == 1){
        strMonth =  "0"+strMonth;
    }
    if(strDay.length == 1){
        strDay =  "0"+strDay;
    }
    return strYear +  strMonth +strDay;

}
export function ConvertYYYYMMDDToDate(yyyymmdd){
    var y = parseInt(yyyymmdd.substring(0,4));
    var m = parseInt(yyyymmdd.substring(4,6))-1;
    var d = parseInt(yyyymmdd.substring(6,8));
    return new Date( y, m, d);
}

export function ConvertYYYYMMDDToStringDate(yyyymmdd){
    var y = parseInt(yyyymmdd.substring(0,4));
    var m = parseInt(yyyymmdd.substring(4,6));
    var d = parseInt(yyyymmdd.substring(6,8));
    return  y+'년 '+ m+'월 '+ d+'일';
}