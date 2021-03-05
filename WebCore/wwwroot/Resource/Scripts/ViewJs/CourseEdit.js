$(function ()
{
   
    SheetEdit.setting.Controller = "Course";
    SheetEdit.setting.title = "新增/ 编辑课程";
    SheetEdit.initControl = function(){
        $("#txtStartTime").datetimepicker({
            forceParse: 0,//设置为0，时间不会跳转1899，会显示当前时间。
            language: 'zh-CN',//显示中文
            format: 'yyyy-mm-dd',//显示格式
            minView: "month",//设置只显示到月份
            initialDate: new Date(),//初始化当前日期
            autoclose: true,//选中自动关闭
            todayBtn: true,//显示今日按钮
            //startDate: new Date(),

        }).on("changeDate", function (ev) {

            $('#txtEndTime').datetimepicker('setStartDate', ev.date);
        });
        $("#txtEndTime").datetimepicker({
            forceParse: 0,//设置为0，时间不会跳转1899，会显示当前时间。
            language: 'zh-CN',//显示中文
            format: 'yyyy-mm-dd',//显示格式
            minView: "month",//设置只显示到月份
            autoclose: true,//选中自动关闭
            todayBtn: true,//显示今日按钮

            ignoreReadonly: true
        }).on("changeDate", function (ev) {
           $('#txtStartTime').datetimepicker('setEndDate', ev.date);
        });

        $("#btnRetrun").on("click", function () {
            history.back();
        })
        $("#btnDeltte").on("click", function () {
            Sp.Control.alerts.confirm("警告", "确定删除?", function (bl) {

                if (bl) {
                    var options = {};
                    options.data = $("#sheetId").val();
                    Common.SendAjax("/Course/DeleteCourseById", options, function (result) {
                        if (result.code == 0) {
                             
                            Sp.Control.alerts.alert("提示", "删除成功");
                            history.back();
                        }
                        else {
                            Sp.Control.alerts.alert("提示", result.Data);
                        }
                    });

                }
            })
        })
        $("#btnSave").on("click", function () {
            SheetEdit.doAction("Save", {}, function (request) {
                Sp.Control.alerts.alert(null,request.Data);
            })
        })
        //初始化课程类型
        $("#dwCourseType").select2({
            tags: false,
            placeholder: '请选择课程类型',
            
            minimumResultsForSearch: -1,//去掉搜索框
            data: [
                { id: 0, text: "C#" }, { id: 1, text: "Sql Service" }, { id: 2, text: "其它" }
            ],
            allowClear: true,
            createSearchChoice: function (term, data) {
                if ($(data).filter(function () { return this.text.localeCompare(term) === 0; }).length === 0) { return { id: term, text: term }; }
            },
        });
     
        //初始优先级
        $("#dwPriority").select2({
            tags: false,
            placeholder: '请选择优先级',
            minimumResultsForSearch: -1,//去掉搜索框
            data: [
                { id: 0, text: "低" }, { id: 1, text: "中" }, { id: 2, text: "高" }
            ],
            allowClear: true,
            createSearchChoice: function (term, data) {
                if ($(data).filter(function () { return this.text.localeCompare(term) === 0; }).length === 0) { return { id: term, text: term }; }
            },
        });

        //初始化文本编辑器
        $('#txtContent').summernote({
            width: "100%",
            height:495,
            lang: 'zh-CN',
          
        });

        //加载页面数据
        SheetEdit.loadPage("GetCourseById", {}, function (request) {
            if (request.code == "0" && request.Data != null) {
                var Master = request.Data.Master;
                var Detailed = request.Data.Detailed;
                $("#txtCourseName").val(Master.Name);

                $("#txtStartTime").datetimepicker("setDate", new Date(Master.StartTime));
                $("#txtEndTime").datetimepicker("setDate", new Date(Master.EndTime));
                $("#txtCourseTime").val(Master.CourseTime);
                $("#txtUseTime").val(Master.UseTime);
                $("#dwCourseType").val(Master.CourseType).trigger("change");
                $("#dwPriority").val(Master.Priority).trigger("change");
                $("#txtCourseLink").val(Master.CourseLink);
                $("#hidestatus").val(Master.Status);

                $("#txtStatus").val(GetStatusName(Master.Status));

                $("#DetailedId").val(Detailed.Id);

                $('#txtContent').summernote('code', Detailed.Content);
               
            }
        })
        
    }
   
    SheetEdit.getParam = function () {
        var data = {};
        var Master = {};
        var Detailed = {};
        Master.Id = $("#sheetId").val();
        Master.Name = $("#txtCourseName").val();
        Master.UseTime = $("#txtUseTime").val();
        Master.StartTime = $("#txtStartTime").data("datetimepicker").getDate();
        Master.EndTime = $("#txtEndTime").data("datetimepicker").getDate();
        Master.CourseTime = $("#txtCourseTime").val();
        Master.CourseLink = $("#txtCourseLink").val();
        Master.CourseType = $("#dwCourseType").val();
        Master.Priority = $("#dwPriority").val();
        Detailed.Id = $("#DetailedId").val();
        Detailed.MasterId = $("#sheetId").val();
        Detailed.Content = $('#txtContent').summernote("code");
        data.Master = Master;
        
        data.Detailed = Detailed;
        return data;
    }
 
    SheetEdit.initUi();

    function GetStatusName(status) {
        var statusname = "";
        switch (status) {
            case "0":
                statusname = "未开始";
                break;
            case "1":
                statusname = "进行中";
                break;
            case "2":
                statusname = "已完成";
                break;
            case "3":
                statusname = "已废弃";
                break;
            default:
                statusname = "未开始";
                break;
        }
        return statusname;
    }

 

})