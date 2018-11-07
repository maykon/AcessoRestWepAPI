(function () {

    function resultadoLog(log) {
        var textArea = document.querySelector('textarea');
        textArea.value = log;

    }

    $('#CalcularAumento').click(function () {
        var _url = 'http://' + window.location.host + '/';
        $.ajax({
            url: _url + "Home/CalcularAumento",
            success: function (result) {
                resultadoLog(result);

            },
            error: function (data) {

            },
            complete: function () {

            }
        })
    });

    $('#CalcularAumentoWebAPI').click(function () {

        var _url = 'http://' + window.location.host + '/';
        $.ajax({
            url: _url + "api/values",
            type: 'POST',
            datatype: 'json',
            contentType: 'application/text',
            //Server para passar um valor para p server via post ou put
            //data: JSON.stringify(obj), 
            success: function (logresult) {

                resultadoLog(logresult);

            },
            error: function (jqXHR, textStatus, errorThrown) {

                throw errorThrown;

            }
            
        }); 
    });

    $('#AlterarStatus').click(function () {        
        var _url = 'http://' + window.location.host + '/';
        var employeeId = $('#employeeId').val();
        if (employeeId == "") {
            alert("Por favor, preencha o ID do funcionário.");
            return;
        }
        var status = $('#status').val();
        if (status == "") {
            alert("Por favor, preencha o status do funcionário.");
            return;
        }
        $.ajax({
            url: _url + "api/employees/" + employeeId,
            type: 'PUT',
            datatype: 'json',
            contentType: 'application/json',
            //Server para passar um valor para p server via post ou put
            data: JSON.stringify(status), 
            success: function (logresult) {

                resultadoLog(logresult);

            },
            error: function (jqXHR, textStatus, errorThrown) {

                throw errorThrown;

            }

        });
    });

    $('#DeletarInativos').click(function () {
        var _url = 'http://' + window.location.host + '/';
        $.ajax({
            url: _url + "api/employees/deleteInactive",
            type: 'DELETE',
            datatype: 'json',
            contentType: 'application/json',
            //Server para passar um valor para p server via post ou put
            success: function (logresult) {

                resultadoLog(logresult);

            },
            error: function (jqXHR, textStatus, errorThrown) {

                throw errorThrown;

            }

        });
    });

})();