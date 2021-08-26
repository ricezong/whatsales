    function dealDate(date){
		
        var myDate =  new Date(parseInt(date.replace(/\/Date\((-?\d+)\)\//, '$1')));
        var m = myDate.getMonth()+1;
        var d = myDate.getDate();
        var dateTime =  myDate.getFullYear()+"-"+(m>9?m:("0"+m))
        +"-"+(d>9?d:("0"+d))+" "+myDate.toLocaleTimeString().substring(2);
        return dateTime;
    }
    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, "");
    }