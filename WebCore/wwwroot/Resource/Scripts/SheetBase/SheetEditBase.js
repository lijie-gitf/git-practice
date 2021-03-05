
var SheetEdit = window.NameSpace || {};
SheetEdit = (function () {
    var that = this;
    that.setting = {};
    that.setting.Controller = "";
    that.setting.title = ""; 
    that.setting.btns = [{ id: "btnSave", icon: "glyphicon-floppy-saved", text: "保存" }, { id: "btnDeltte", icon: "glyphicon-remove", text: "删除" }, { id: "btnStar", icon: "glyphicon-star", text: "开始" }, { id: "btnEnd", icon: "glyphicon-send", text: "结束" }, { id: "btnRetrun", icon: "glyphicon-log-in", text: "返回" }];
    that.initControl = function () {
      
    }
    that.getParam = function () {

    }
    that.verifiCation = function () {
        var result = true;
       
        //获取页面控件
        var controls = $("input,select,textarea");
        $.each(controls, function (i, item) {
            var type = $(item).attr("type");
            var tag = $(item).prop("nodeName").toLowerCase();
           
            var Required = $(item).data("required");
            if (tag == "input" || tag == "textarea") {
                if (Required == true && ($(item).val() == null || $(item).val() == "")) {


                    if (!$(item).hasClass("Required")) {

                        $(item).addClass("Required");
                    }
                    result = false;

                }
                else {
                    if ($(item).hasClass("Required")) {
                        $(item).removeClass("Required");
                    }
                }
            }
            else if (tag == "select") {
                if(Required == true && ($(item).val() == null || $(item).val() == "")) {

                    if (!$(item).next("span").hasClass("Required")) {

                        $(item).next("span").addClass("Required");
                    }
                    result = false;

                }
                else {
                    if ($(item).next("span").hasClass("Required")) {
                        $(item).next("span").removeClass("Required");
                    }
                }
            }
          
          
        })
        return result;
    }
    that.doAction = function (action, options, callback) {

        if (!that.verifiCation()) {
            return;
        }
        var url = "/{0}/{1}";
        var optdefaults = {};
     
        optdefaults.Controller = that.setting.Controller;
        options = $.extend({}, optdefaults, options);

        url = url.format(options.Controller, action);
      
        options.data = that.getParam();
        Common.SendAjax(url, options, callback)
    }
    that.loadPage = function (action, options, callback)
    {
        
        var Id = Common.getUrlParam("Id");
       
        if (Id > 0) {
            $("#sheetId").val(Id);
            var url = "/{0}/{1}";
            var optdefaults = {};

            optdefaults.Controller = that.setting.Controller;
            options = $.extend({}, optdefaults, options);

            url = url.format(options.Controller, action);

            options.data = { Id: Id };
            Common.SendAjax(url, options, callback)
        }
    }
    that.initUi = function () {
        if (that.setting.Controller == "") {
            Sp.Control.alerts.alert(null, "请设置控制器");
            return;
        }
        $(".page-header>h5").html(that.setting.title);
        
        $.each(that.setting.btns, function (i, btn) {
            createBtn(btn);
          
        });
        that.initControl();
      
    };
   
    //生成按钮
    function createBtn(btn) {
        var toobar = $(".page-content>.btn-querytoobal>.btn-toobar");
        var btnhtml = "<div class=\"btn btn-white btn-no-border\" aria-label=\"Left Align\" id=\"" + btn.id + "\"><div class=\"glyphicon " + btn.icon + "\" aria-hidden=\"true\">" + btn.text + "</div></div>";
        toobar.append(btnhtml);
    }


    return that;
}(jQuery))
