
$(document).ready(function () {

    const selInvType = document.getElementById("hdnInvType").value;
    const selInvSubType  = document.getElementById("hdnInvSubType").value;
    const selVendorCode  = document.getElementById("hdnVendorCode").value;
    const selSatuanKecil = document.getElementById("hdnSmallUnit").value;
    const selSatuanBesar = document.getElementById("hdnLargeUnit").value;
    const selBrand = document.getElementById("hdnBrand").value;

    // Populate Inventory Type (ComboBox)
    $.ajax({
        url: 'Inventory/GetInvTypes',
        type: 'GET',
        success: function (data) {
            console.log(data);
            $('#InvType').empty();
            $('#InvType').append('<option value="">Select Inventory Type</option>');
            $.each(data, function (index, val) { 
                console.log(val);
                $('#InvType').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#InvType').val(selInvType);
        }
    });

    // Populate Inventory Sub Type (ComboBox)
    $.ajax({
        url: 'Inventory/GetInvSubTypes',
        type: 'GET',
        success: function (data) {
            $('#InvSubType').empty();
            $('#InvSubType').append('<option value="">Select Inventory Sub Type</option>');
            $.each(data, function (index, val) {
                $('#InvSubType').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#InvSubType').val(selInvSubType);
        }
    });

    // Populate Brand (ComboBox)
    $.ajax({
        url: 'Inventory/GetBrand',
        type: 'GET',
        success: function (data) {
            $('#Brand').empty();
            $('#Brand').append('<option value="">Select Brand</option>');
            $.each(data, function (index, val) {
                $('#Brand').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#Brand').val(selBrand);
        }
    });

    // Populate Vendor Code (ComboBox)
    $.ajax({
        url: 'Inventory/GetVendor',
        type: 'GET',
        success: function (data) {
            $('#VendorCode').empty();
            $('#VendorCode').append('<option value="">Select Vendor Code</option>');
            $.each(data, function (index, val) {
                $('#VendorCode').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#VendorCode').val(selVendorCode);
        }
    });

    // Populate Small Unit (ComboBox)
    $.ajax({
        url: 'Inventory/GetUnit',
        type: 'GET',
        success: function (data) {
            $('#SmallUnit').empty();
            $('#SmallUnit').append('<option value="">Select Unit</option>');
            $.each(data, function (index, val) {
                $('#SmallUnit').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#SmallUnit').val(selSatuanKecil);
        }
    });

    // Populate Large Unit (ComboBox)
    $.ajax({
        url: 'Inventory/GetUnit',
        type: 'GET',
        success: function (data) {
            $('#LargeUnit').empty();
            $('#LargeUnit').append('<option value="">Select Unit</option>');
            $.each(data, function (index, val) {
                $('#LargeUnit').append('<option value="' + val.Value + '">' + val.Text + '</option>');
            });
            $('#LargeUnit').val(selSatuanBesar)
        }
    });

    //console.log(selInvType + selInvSubType + selBrand + selVendorCode + selSatuanKecil + selSatuanBesar)

    // Handle form submission Edit
    $('#editProductForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Inventory/UpdInventory',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Produk berhasil diperbarui!');
                $("#myPopupInvEdit").dxPopup("hide");
                $("#dataGridInventory").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat memperbarui produk: ' + error);
            }
        });
    });

    // Handle form submission Edit
    $('#addProductForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Inventory/AddInventory',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Produk berhasil ditambahkan!');
                $("#myPopupInvAdd").dxPopup("hide");
                $("#dataGridInventory").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat menambahkan produk: ' + error);
            }
        });
    });

});