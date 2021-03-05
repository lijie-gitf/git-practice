
var Sheet = window.NameSpace || {};
Sheet = (function () {
    var that = this;
    that.setting = {};
    that.setting.Controller = ""; 
    that.setting.btns = [{ id: "btnAdd", icon: "glyphicon-plus", text: "新增" }, { id: "btnEdit", icon: "glyphicon-pencil", text: "编辑" }, { id: "btnDeltte", icon: "glyphicon-remove", text: "删除" }];
    that.initUi = function () {
        if (that.setting.Controller == "") {
            Sp.Control.alerts.alert(null, "请设置控制器");
            return;
        }
            bindDoloadquery();
           
            $.each(that.setting.btns, function (i, btn) {
                createBtn(btn);
            });
        };
        //绑定展开明细事件
        function bindDoloadquery() {
            $(".dowloadquery").on("click", function () {
                if ($(".dowloadparam").is(":hidden")) {

                    $(this).find("a>span").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up")
                    $(".dowloadparam").slideToggle(600);
                }
                else {
                    $(this).find("a>span").removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down")
                    $(".dowloadparam").slideToggle(600);
                }

            })
        }
        //生成按钮
    function createBtn(btn) {
        var toobar = $(".page-content>.btn-querytoobal>.btn-toobar");
            var btnhtml = "<div class=\"btn btn-white btn-no-border\" aria-label=\"Left Align\" id=\"" + btn.id + "\"><div class=\"glyphicon " + btn.icon + "\" aria-hidden=\"true\">" + btn.text+"</div></div>";
            toobar.append(btnhtml);
    }
    return that;
    }(jQuery))
