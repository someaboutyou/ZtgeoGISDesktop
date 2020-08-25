define(["require", "exports"], function (require, exports) {
    "use strict"
    var testApi;
    (function (testApi) {
        testApi.AlertMessage = function (message) {
            alert(message)
        }
        window.TestApp2JSAdapterApi = testApi; 
    })(testApi = exports.testApi || (exports.testApi = {}));
})