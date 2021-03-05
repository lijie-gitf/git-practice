String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
};


/**
 * ajax post提交
 *  url 路径
 * options 参数
 * callback 回调
 */

var Common = window.NameSpace || {};
Common=(function () {
    var that = this;
  
   
    that.SendAjax = function (url, options, callback) {
        var defaults = {};
        defaults.type = "POST";
        defaults.dataType = "json";
        defaults.contentType = "application/json";
        if (url == null || url == "" || typeof (url) == undefined) {
            Sp.Control.alerts.alert("提示", "确少Url参数");
            return;
        }
        options = $.extend({}, defaults, options);
       
        $.ajax({
            url: url,
            type: options.type,
            data: JSON.stringify(options.data),
            dataType: options.dataType,
            contentType: options.contentType,
            success: function (result) {
                if (callback != null && typeof (callback) == "function") {
                    callback(result);
                }
            },
            error: function (jqXHR) {
                Sp.Control.alerts.alert("发生错误：" + jqXHR.status)
            }
        })
    }
    that.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }

    return that;
}(jQuery))