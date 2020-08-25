define(["require", "exports"], function (require, exports) {
    "use strict"
    var FormIOJs2AppAdapterApi;
    (function (FormIOJs2AppAdapterApi) {
        FormIOJs2AppAdapterApi.Save = function (submissionData) {
            window.FormIOJs2AppAdapterApi.onSave(JSON.stringify(submissionData));
        }
        window.__FormIOJs2AppAdapterApi = FormIOJs2AppAdapterApi;  
    })(FormIOJs2AppAdapterApi = exports.FormIOJs2AppAdapterApi || (exports.FormIOJs2AppAdapterApi = {}));
})