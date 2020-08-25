define(["require", "exports"], function (require, exports) {
    "use strict"
    var MenuSettingJs2AppAdapterApi; 
    (function (MenuSettingJs2AppAdapterApi) {
 
        MenuSettingJs2AppAdapterApi.Close = function () {
            window.MenuSettingJs2AppAdapterApi.onClose();
        }
        MenuSettingJs2AppAdapterApi.SaveMenuSetting = function (menuSettingStr) {
            window.MenuSettingJs2AppAdapterApi.onSaveMenuSetting(menuSettingStr);
        }
        window.__MenuSettingJs2AppAdapterApi = MenuSettingJs2AppAdapterApi;  
    })(MenuSettingJs2AppAdapterApi = exports.MenuSettingJs2AppAdapterApi || (exports.MenuSettingJs2AppAdapterApi = {}));
})