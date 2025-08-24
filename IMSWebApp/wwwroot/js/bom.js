
$(document).ready(function () {

    const selFGCode = document.getElementById("hdnFGCode").value;
    var selFGName = "";

    //// Handle form submission Edit
    //$('#editProductForm').on('submit', function (e) {
    //    e.preventDefault();
    //    var formData = $(this).serializeArray();
    //    $.ajax({
    //        url: 'Inventory/UpdateInventory',
    //        type: 'POST',
    //        data: formData,
    //        //processData: false, 
    //        //contentType: false,
    //        success: function (response) {
    //            alert('Produk berhasil diperbarui!');
    //            $("#myPopupEdit").dxPopup("hide");
    //            $("#dataGridInventory").dxDataGrid("refresh");
    //        },
    //        error: function (xhr, status, error) {
    //            alert('Terjadi kesalahan saat memperbarui produk: ' + error);
    //        }
    //    });
    //});

    //// Handle form submission Edit
    //$('#addProductForm').on('submit', function (e) {
    //    e.preventDefault();
    //    var formData = $(this).serializeArray();
    //    $.ajax({
    //        url: 'Inventory/AddInventory',
    //        type: 'POST',
    //        data: formData,
    //        //processData: false, 
    //        //contentType: false,
    //        success: function (response) {
    //            alert('Produk berhasil ditambahkan!');
    //            $("#myPopupAdd").dxPopup("hide");
    //            $("#dataGridInventory").dxDataGrid("refresh");
    //        },
    //        error: function (xhr, status, error) {
    //            alert('Terjadi kesalahan saat menambahkan produk: ' + error);
    //        }
    //    });
    //});


    // Populate Inventory Type (ComboBox)
    $.ajax({
        url: 'BillOfMaterial/GetInventoryFG',
        type: 'GET',
        success: function (data) {
            $('#FGCode').empty();
            $('#FGCode').append('<option value="">Select Inventory</option>');
            $.each(data, function (index, val) {
                $('#FGCode').append('<option value="' + val.Value + '"> [' + val.Value +'] '+ val.Text + '</option>');
            });
            $('#FGCode').val(selFGCode);
        }
    });

    const sBOMCode = document.getElementById("BOMCode").value;

    $("#bomMaterialGrid").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'BOMCode',
            loadUrl: 'BillOfMaterial/GetBOMMaterials',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options": "BMU3|" + sBOMCode
                };
                ajaxOptions.xhrFields = { withCredentials: true };
            },
        }), 
        columns: [
            {
                type: "buttons",
                caption: "Actions",
                buttons: [
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var bomId = e.row.data.Id;
                            //openPopupDetailBOM(inventoryId);
                        }
                    },
                    {
                        hint: "Remove",
                        icon: "far fa-trash-alt",
                        onClick: function (e) {
                            var bomId = e.row.data.Id;
                            //openPopupEditBOM(bomId);
                        }
                    },
                ]
            },
            { dataField: "MaterialCode", caption: "Material Code" },
            { dataField: "MaterialName", caption: "Material Name" },
            { dataField: "QtyUsage", caption: "Quantity" },
            { dataField: "UnitDesc", caption: "UOM" },
        ],
        columnAutoWidth: false,
        showRowLines: true,
    });

});