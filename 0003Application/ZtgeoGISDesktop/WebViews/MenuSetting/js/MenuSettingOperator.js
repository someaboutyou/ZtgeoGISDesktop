define(["require", "exports", 'jquery', 'zhCN', 'easyui','MenuSetting/js/Js2AppAdapterApi/MenuSettingJs2AppAdapterApi'], function (require, exports , $, zhCN, easyui, js2AppAdapter) {
    var MenuSettingOperator;
    (function (operator) {
		function moveOperation(isUp) {
			var selectNode = $("#MenuTreeGrid").treegrid("getSelected");
			if (selectNode == null) {
				$.messager.alert('操作提示', '请选择数据！', 'warning');
				return;
			}
			//动态获取datagrid-row的ID
			var rowL = $('.datagrid-row').attr('id').split('-');
			rowL.splice(rowL.length - 1, 1);
			var newRowL = rowL.join('-');
			var selectRow = $('#' + newRowL + '-' + selectNode.MenuId);
			if (isUp) {
				var pre = selectRow.prev();//获得上一节点
				if (pre.length == 0) {
					$.messager.alert('操作提示', '无法移动！', 'warning'); 
					return;
				}
				var preClass = pre.attr("class");
				while (preClass == 'treegrid-tr-tree') {
					pre = pre.prev();
					preClass = pre.attr("class");
				}
				var preId = pre.attr("node-id");
				var selectNode2 = $("#MenuTreeGrid").treegrid("pop", selectNode.MenuId);
				$("#MenuTreeGrid").treegrid("insert", { before: preId, data: selectNode2 });
				$("#MenuTreeGrid").treegrid("select", selectNode.MenuId);
			} else {
				var next = selectRow.next();//获得下一节点
				if (next.length == 0) {
					$.messager.alert('操作提示', '无法移动！', 'warning'); 
					return false;
				}
				var nextClass = next.attr("class");
				while (nextClass == 'treegrid-tr-tree') {
					next = next.next();
					if (next.length === 0) {
						$.messager.alert('操作提示', '无法移动！', 'warning'); 
						return false;
					}
					nextClass = next.attr("class");
				}
				var nextId = next.attr("node-id");
				var selectNode2 = $("#MenuTreeGrid").treegrid("pop", selectNode.MenuId);
				$("#MenuTreeGrid").treegrid("insert", { after: nextId, data: selectNode2 });
				$("#MenuTreeGrid").treegrid("select", selectNode.MenuId);
			}
        }
        operator.rowUp = function () {
			moveOperation(true);
		}
		operator.rowDown = function () {
			moveOperation(false);
		}
		operator.save = function () { //保存 
			var allDatas = $("#MenuTreeGrid").treegrid('getData');
			var settingData = [];
			function getType(obj) {
				//tostring会返回对应不同的标签的构造函数
				var toString = Object.prototype.toString;
				var map = {
					'[object Boolean]': 'boolean',
					'[object Number]': 'number',
					'[object String]': 'string',
					'[object Function]': 'function',
					'[object Array]': 'array',
					'[object Date]': 'date',
					'[object RegExp]': 'regExp',
					'[object Undefined]': 'undefined',
					'[object Null]': 'null',
					'[object Object]': 'object'
				};
				if (obj instanceof Element) {
					return 'element';
				}
				return map[toString.call(obj)];
			}
			function deepClone(data) {
				var type = getType(data);
				var obj;
				if (type === 'array') {
					obj = [];
				} else if (type === 'object') {
					obj = {};
				} else {
					//不再具有下一层次
					return data;
				}
				if (type === 'array') {
					for (var i = 0, len = data.length; i < len; i++) {
						obj.push(deepClone(data[i]));
					}
				} else if (type === 'object') {
					for (var key in data) {
						obj[key] = deepClone(data[key]);
					}
				}
				return obj;
			}
			function ergodic(ergodicData) {
				ergodicData.forEach((item, index, arr) => {
					item.Order = index;
					settingData.push(deepClone( item));
					if (item.children) {
						ergodic(item.children)
                    }
				});
			}
			ergodic(allDatas);
			settingData.forEach((item, index, arr) => {
				delete item.children;
				delete item._parentId;
				delete item.state;
			}); 
			if (!window.CefSharp)
				console.log(JSON.stringify(settingData))
			else {
				js2AppAdapter.MenuSettingJs2AppAdapterApi.SaveMenuSetting(JSON.stringify(settingData))
			}
		}
        window.MenuSettingOperator = operator
    })(MenuSettingOperator = exports.MenuSettingOperator || (exports.MenuSettingOperator={}))
})