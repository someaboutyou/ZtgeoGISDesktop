define(["require", "exports"], function (require, exports) {
    "use strict"
    var testApi;
    (function (testApi) {
        testApi.MessageBox = function (msg) {
            window.TestJs2AppAdapterApi.onMessageBox(msg);
        }
        window.__TestJs2AppAdapterApi = testApi;  
    })(testApi = exports.testApi || (exports.testApi = {}));
})