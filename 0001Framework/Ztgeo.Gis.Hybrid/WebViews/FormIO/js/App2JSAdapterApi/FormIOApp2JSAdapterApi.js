define(["require", "exports"], function (require, exports) {
    "use strict"
    var FormIOApp2JSAdapterApi;
    (function (FormIOApp2JSAdapterApi) {
        FormIOApp2JSAdapterApi.SetFormIOComponentAndData = function(component,data) { 
            console.log(arguments) 
            Formio.createForm(document.getElementById('formio'), JSON.parse(component)).then(
                form => { 
                    if(data)
                        form.submission = { data: JSON.parse(data) };
                    form.on("submit", (e) => { 
                        __FormIOJs2AppAdapterApi.Save(e);
                    });
                }
            ); 
        }
        window.FormIOApp2JSAdapterApi = FormIOApp2JSAdapterApi; 
    })(FormIOApp2JSAdapterApi = exports.FormIOApp2JSAdapterApi || (exports.FormIOApp2JSAdapterApi = {}));
})