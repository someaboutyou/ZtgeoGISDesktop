define(["require", "exports", 'jquery', 'zhCN', 'easyui'], function (require, exports, $, zhCN, easyui) {
    "use strict" 
    var MenuSettingApp2JSAdapterApi;
    (function (MenuSettingApp2JSAdapterApi) {
        MenuSettingApp2JSAdapterApi.SetMenuSettingData = function (sdata) { //设置table的数据  
            console.log(sdata);
            var data
            if (!(sdata instanceof Object))
                data = JSON.parse(sdata);
            else
                data = sdata
            //组织树形数据
            var root= data.filter(d => ! d.ParentMenuKey)
            function ergodic(data,p) {
                data.forEach((item, index, arr) => {
                    if (item.ParentMenuKey === p.MenuKey) {
                        if (!p.children) {
                            p.children=[]
                        }
                        p.children.push(item) 
                        p.children = p.children.sort(function (s1,s2) {
                            return s1.Order - s2.Order;
                        })
                        ergodic(data,item)
                    }
                })
            } 
            root.forEach((item, index, arr) => {
                ergodic(data,item)
            });

            $('#MenuTreeGrid').treegrid({ 
                data: root,
                rownumbers: true, // 显示行号列
                singleSelect: true,// 只能单选行
                checkOnSelect: true,
                loadMsg: '请稍候，数据加载中...',// 自定义等待消息
                emptyMsg: '查询数据为空...', 
                idField: 'MenuId',
                treeField:'MenuName',
                columns: [[ 
                    { title: '菜单名称', field: 'MenuName', width: 180 },
                    { title: '描述', field: 'MenuDescription', width: 540 ,align: 'right' },
                    //{
                    //    title: '操作', field: 'MenuKey', width: 160, align: 'center',  formatter: function (value, row, index) {
                    //        return "<a id=\"btnUp\" href=\"#\" class=\"easyui-linkbutton\" data-options=\"iconCls: 'icon-up'\"></a>"
                    //            + "<a id=\"btnDown\" href=\"#\" class=\"easyui-linkbutton\" data-options=\"iconCls: 'icon-down'\"></a>";
                    //    }
                    //}
                ]]
            }); 

        }
        window.MenuSettingApp2JSAdapterApi = MenuSettingApp2JSAdapterApi; 
    })(MenuSettingApp2JSAdapterApi = exports.MenuSettingApp2JSAdapterApi || (exports.MenuSettingApp2JSAdapterApi = {}));
})