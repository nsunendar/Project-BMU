
$(document).ready(function () {

    const selCustType = document.getElementById("hdnCustType").value;
    const selCustSubType = document.getElementById("hdnCustSubType").value;
    const selCustArea = document.getElementById("hdnArea").value;
    const selSalesman = document.getElementById("hdnSalesman").value;

    // Populate Inventory Type (ComboBox)
    $.ajax({
        url: 'Customer/GetCustType',
        type: 'GET',
        success: function (data) {
            $('#CustType').empty();
            $('#CustType').append('<option value="">Select Type</option>');
            $.each(data, function (index, val) {
                $('#CustType').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#CustType').val(selCustType);
        }
    });

    // Populate Inventory Sub Type (ComboBox)
    $.ajax({
        url: 'Customer/GetCustSubType',
        type: 'GET',
        success: function (data) {
            $('#CustSubType').empty();
            $('#CustSubType').append('<option value="">Select Sub Type</option>');
            $.each(data, function (index, val) {
                $('#CustSubType').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#CustSubType').val(selCustSubType);
        }
    });

    // Populate Brand (ComboBox)
    $.ajax({
        url: 'Customer/GetCsArea',
        type: 'GET',
        success: function (data) {
            $('#Area').empty();
            $('#Area').append('<option value="">Select Area</option>');
            $.each(data, function (index, val) {
                $('#Area').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#Area').val(selCustArea);
        }
    });

    // Populate Vendor Code (ComboBox)
    $.ajax({
        url: 'Customer/GetCsSalesman',
        type: 'GET',
        success: function (data) {
            $('#Salesman').empty();
            $('#Salesman').append('<option value="">Select Salesman</option>');
            $.each(data, function (index, val) {
                $('#Salesman').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#Salesman').val(selSalesman);
        }
    });

    // Handle form submission Edit
    $('#editCustomerForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        console.log(formData);
        $.ajax({
            url: 'Customer/UpdCustomer',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Customer berhasil diperbarui!');
                $("#myPopupCustEdit").dxPopup("hide");
                $("#dataGridCustomer").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat memperbarui produk: ' + error);
            }
        });
    });

    // Handle form submission Edit
    $('#addCustomerForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Customer/InsCustomer',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Customer berhasil ditambahkan!');
                $("#myPopupInvAdd").dxPopup("hide");
                $("#dataGridInventory").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat menambahkan produk: ' + error);
            }
        });
    });

});