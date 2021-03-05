

var Sp = window.NameSpace || {};
Sp.Control = (function () {
    var that = this;
    //弹出框
    that.alerts = {
        alert: function (title, message, callback) {
            if (title == null || title == "") title = '提示';
            that.alerts._show(title, message, null, 'alert', function (result) {
                if (callback) callback(result);
            });
        },

        confirm: function (title, message, callback) {
            if (title == null || title == "") title = '提示';
            that.alerts._show(title, message, null, 'confirm', function (result) {
                if (callback) callback(result);
            });
        },


        _show: function (title, msg, value, type, callback) {

            var _html = "";

            _html += '<div id="mb_box"></div><div id="mb_con"><span id="mb_tit">' + title + '</span>';
            _html += '<div id="mb_msg">' + msg + '</div><div id="mb_btnbox">';
            if (type == "alert") {
                _html += '<input id="mb_btn_ok" type="button" value="确定" />';
            }
            if (type == "confirm") {
                _html += '<input id="mb_btn_no" type="button" value="取消" />';
                _html += '<input id="mb_btn_ok" type="button" value="确定" />';
            }
            _html += '</div></div>';

            //必须先将_html添加到body，再设置Css样式  
            $("body").append(_html); that.alerts.GenerateCss();

            switch (type) {
                case 'alert':

                    $("#mb_btn_ok").click(function () {
                        that.alerts._hide();
                        callback(true);
                    });
                    $("#mb_btn_ok").focus().keypress(function (e) {
                        if (e.keyCode == 13 || e.keyCode == 27) $("#mb_btn_ok").trigger('click');
                    });
                    break;
                case 'confirm':

                    $("#mb_btn_ok").click(function () {
                        that.alerts._hide();
                        if (callback) callback(true);
                    });
                    $("#mb_btn_no").click(function () {
                        that.alerts._hide();
                        if (callback) callback(false);
                    });
                    $("#mb_btn_no").focus();
                    $("#mb_btn_ok, #mb_btn_no").keypress(function (e) {
                        if (e.keyCode == 13) $("#mb_btn_ok").trigger('click');
                        if (e.keyCode == 27) $("#mb_btn_no").trigger('click');
                    });
                    break;
            }
        },
        _hide: function () {
            $("#mb_box,#mb_con").remove();
        },
        GenerateCss: function () {
            $("#mb_box").css({
                width: '100%', height: '100%', zIndex: '99999', position: 'fixed',
                filter: 'Alpha(opacity=60)', backgroundColor: 'black', top: '0', left: '0', opacity: '0.6'
            });

            $("#mb_con").css({
                zIndex: '999999', width: '350px', height: '200px', position: 'fixed',
                backgroundColor: 'White',
            });

            $("#mb_tit").css({
                display: 'block', fontSize: '14px', color: '#444', padding: '10px 15px',
                backgroundColor: '#fff', borderRadius: '15px 15px 0 0',
                fontWeight: 'bold'
            });

            $("#mb_msg").css({
                padding: '20px', lineHeight: '40px', textAlign: 'center',
                fontSize: '18px', color: '#4c4c4c'
            });

            $("#mb_ico").css({
                display: 'block', position: 'absolute', right: '10px', top: '9px',
                border: '1px solid Gray', width: '18px', height: '18px', textAlign: 'center',
                lineHeight: '16px', cursor: 'pointer', borderRadius: '12px', fontFamily: '微软雅黑'
            });

            $("#mb_btnbox").css({ margin: '15px 0px 10px 0', textAlign: 'center' });
            $("#mb_btn_ok,#mb_btn_no").css({ width: '80px', height: '30px', color: 'white', border: 'none', borderRadius: '4px' });
            $("#mb_btn_ok").css({ backgroundColor: '#41a259' });
            $("#mb_btn_no").css({ backgroundColor: 'gray', marginRight: '40px' });


            //右上角关闭按钮hover样式  
            $("#mb_ico").hover(function () {
                $(this).css({ backgroundColor: 'Red', color: 'White' });
            }, function () {
                $(this).css({ backgroundColor: '#DDD', color: 'black' });
            });

            var _widht = document.documentElement.clientWidth; //屏幕宽  
            var _height = document.documentElement.clientHeight; //屏幕高  

            var boxWidth = $("#mb_con").width();
            var boxHeight = $("#mb_con").height();

            //让提示框居中  
            $("#mb_con").css({ top: (_height - boxHeight) / 2 + "px", left: (_widht - boxWidth) / 2 + "px" });
        }

    },

    //弹出窗
    that.openwindow = {
        //显示,title:标题，opt：属性设置，callback：回调
        open: function (opt, callback) {
            if (opt.url == null || typeof (opt.url) == "undefined") {
                Sp.Control.alerts.alert("提示", "请输入路径")
                return;
            }
            var opst = {}
            var optsetting = $.extend({}, opst, opt);
          
            optsetting.callback = callback;
            //that.openwindow._show();
            var html = "<div class=\"modal\" id=\"modal\" >"
                            +"<div class=\"modal-dialog\">"
                       +"<div class=\"modal-content\"></div>"
                      +"</div>"
                    +"</div>"
            $("body").append(html);
            that.openwindow.GenerateCss(optsetting);
        
            $('#modal').modal({
                keyboard: false,
                remote: optsetting.url,
               
            })
            $('#modal').on("hide.bs.modal", function (e) {
               
                that.openwindow.hide("#modal");
                if (optsetting.callback != null && typeof (optsetting.callback) == "function") {
                    optsetting.callback();
                }
            })
           
        },
        hide: function (e) {
            $(e).removeData("bs.modal");
            /*modal页面加载$()错误,由于移除缓存时加载到<div class="modal-content"></div>未移除的数据，            手动移除加载的内容*/

            $(e).find(".modal-content").children().remove();
            $("#modal").remove();

        },
        GenerateCss: function (opt) {
            if (opt.width) {
                $("#modal").find("div.modal-dialog").css({ width: opt.width});
            }
            if (opt.height) {
                $("#modal").find("div.modal-dialog").css({  height: opt.height });
            }
           

        },
      
        }
    that.showMask = function () {
        var html = "<div class=\"modal\" id=\"modalmask\">"
            + "<div class=\"modal-dialog\">"
            + "<div class=\"modal-content\"></div>"
            + "</div>"
            + "</div>"
        $("body").append(html);
        $('#modalmask').modal('show');
    }
    that.hideMask = function () {
   
        $("#modalmask").modal("hide");
        $("#modalmask").remove();
    }

    return that;
})(jQuery)