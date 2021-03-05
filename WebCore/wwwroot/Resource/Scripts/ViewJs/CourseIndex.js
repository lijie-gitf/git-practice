function edit(data) {

    window.location.href = "/Course/Edit?Id=" + data;
}

function Delete(Id) {
    Sp.Control.alerts.confirm("警告", "确定删除?", function (bl) {

        if (bl) {
            var options = {};
            options.data = Id;
            Common.SendAjax("/Course/DeleteCourseById", options, function (result) {
                if (result.code == 0) {
                    $("#divGride").DataTable().ajax.reload();
                    Sp.Control.alerts.alert("提示", "删除成功");
                }
                else {
                    Sp.Control.alerts.alert("提示", result.Data);
                }
            });
          
        }
    })

}

$(function () {
   
    Sheet.setting.Controller = "Course1";
    Sheet.initUi();
    $("#btnAdd").on("click", function () {
        window.location.href = "/Course/Edit";
    })
    $("#btnSearch").on("click", function () {
        $("#divGride").DataTable().ajax.reload();
    })
    //全选
    $("#checkall").on("click", function () {

        if ($(this).is(":checked")) {
            $('#divGride>tbody').find("input[type='checkbox']").attr("checked", true);
        }
        else {
            $('#divGride>tbody').find("input[type='checkbox']").attr("checked", false);
        }
    })
    //删除菜单
    $("#btnDeltte").on("click", function () {
        Sp.Control.alerts.confirm("警告", "确定删除?", function (bl) {
            if (bl) {
                var arryids = [];
                var tr = $("#divGride>tbody>tr");
                $.each(tr, function (i, row) {

                    var ckbox = $(row).find("input[type='checkbox']");

                    if (ckbox.is(":checked") == true) {
                        var rowdata = $("#divGride").DataTable().rows(row).data()[0];
                        arryids.push(rowdata.Id);
                    }
                })

                if (arryids.length <= 0) {
                    Sp.Control.alerts.alert(null, "请选择要删除的数据");

                    return;
                }
                var options = {};
                options.data = arryids;
                
                Common.SendAjax("/Course/DeleteCourseByIds", options, function (result) {
                    if (result.code == 0) {
                        $("#divGride").DataTable().ajax.reload();
                        Sp.Control.alerts.alert("提示", "删除成功");
                    }
                    else {
                        Sp.Control.alerts.alert("提示", result.Data);
                    }
                });
              
            }
        })
    })
    //加载表格
    $('#divGride').DataTable(
        {
            language: {
                sProcessing: "处理中...",
                sLengthMenu: "每页 _MENU_ 项",
                sZeroRecords: "没有匹配结果",
                sInfo: "当前显示第 _START_ 至 _END_ 项，共 _TOTAL_ 项。",
                sInfoEmpty: "当前显示第 0 至 0 项，共 0 项",
                sInfoFiltered: "(由 _MAX_ 项结果过滤)",
                sSearch: "搜索:",
                sEmptyTable: "表中数据为空",
                sLoadingRecords: "载入中...",
                oPaginate: {
                    sFirst: "首页",
                    sPrevious: "上页",
                    sNext: "下页",
                    sLast: "末页",
                    Jump: "跳转"
                },

            },
            processing: false,
            rowId: "Id",
            serverSide: true,  //启用服务器端分页
            searching: false,  //禁用原生搜索
            orderMulti: false,  //启用多列排序
            renderer: "bootstrap",  //渲染样式：Bootstrap和jquery-ui
            sAjaxDataProp: "Data",
            iDisplayLength: 5,
            aLengthMenu: [5, 10, 15],
            pagingType: "full_numbers",  //分页样式：simple,simple_numbers,full,full_numbers
            ajax: function (data, callback, settings) {

                var arry = [];
                var param = {};
                param.rows = data.length;//页面显示记录条数，在页面显示每页显示多少项的时候
                param.page = data.start;//开始的记录序号
                param.page = (data.start / data.length) + 1;//当前页码
                param.sidx = data.order.length > 0 ? data.columns[data.order[0].column].data : "Id";
                param.sord = (data.order.length > 0 ? data.order[0].dir : "asc") == "asc";
                param.querydata = { Keword: "", type: "0" },

                    $.ajax({
                        type: "POST",
                        beforeSend: function () {

                            Sp.Control.showMask();
                        },
                        complete: function () {

                            Sp.Control.hideMask();

                        },
                        url: "/Course/SeachCourse",
                        cache: false,  //禁用缓存
                        data: JSON.stringify(param),  //传入组装的参数
                        contentType: "application/json",
                        dataType: "json",
                        success: function (result) {

                            //setTimeout仅为测试延迟效果
                            setTimeout(function () {
                                //封装返回数据
                                var returnData = {};
                                returnData.draw = data.draw;//这里直接自行返回了draw计数器,应该由后台返回
                                returnData.recordsTotal = result.total;//返回数据全部记录
                                returnData.recordsFiltered = result.total;//后台不实现过滤功能，每次查询均视作全部结果
                                returnData.Data = result.Data;//返回的数据列表
                                //console.log(returnData);
                                //调用DataTables提供的callback方法，代表数据已封装完成并传回DataTables进行渲染
                                //此时的数据需确保正确无误，异常判断应在执行此回调前自行处理完毕
                                callback(returnData);
                            }, 200);
                        }
                    });

            },

            columns: [
                {
                    data: "Id", orderable: false, width: "5%",
                    render: function (e) {
                        return "<input type=\"checkbox\"  />";
                    }

                },
                { data: "Name" },
                { data: "CourseTime", orderable: false },
                {
                    data: "UseTime", orderable: false
                },
                { data: "CourseType", orderable: false },
                { data: "Priority", orderable: false },
                { data: "Status", orderable: false },
                { data: "CreateDate", orderable: false },
                { data: "CreateUserName", orderable: false },
               
                {
                    data: "Id", orderable: false, className: "operation", render: function (data) {
                        return "<a class=\"edit\" style=\"cursor:pointer\" onclick=\"edit(" + data + ")\">编辑</a>&nbsp&nbsp&nbsp<a  style=\"cursor:pointer\" onclick=\"Delete(" + data + ")\" > 删除</a > ";
                    }
                }
            ]
        }
    );
})