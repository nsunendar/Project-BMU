
$(document).ready(function () {
    dataGridVendor = $("#dataGridVendor").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'BrandCode',
            loadUrl: 'BillOfMaterial/GetData',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options" : "BMU|"
                };
                ajaxOptions.xhrFields = { withCredentials: true };
            },
        }),
        columns: [
            {
                type: "buttons",
                caption: " ",
                buttons: [
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var vndcode = e.row.data.VendoCode;
                            openPopupVendorEdit(vndcode);
                        }
                    },
                    {
                        hint: "Delete",
                        icon: "fa fa-trash",
                        onClick: function (e) {
                            var vndcode = e.row.data.VendoCode;
                            deleteVendorRow(vndcode);
                        }
                    },
                ],
                width: 70
            },
            { dataField: "VendorCode", caption: "Vendor Code", width: 120, },
            { dataField: "VendorName", caption: "Vendor Name", width: 200, },
            { dataField: "VendorAddress", caption: "Vendor Address", width: 250, },
            { dataField: "City", caption: "City", width: 150, },
            { dataField: "Phone", caption: "Phone", width: 150, },
            { dataField: "Email", caption: "Email", width: 200, },
            { dataField: "Status", caption: "Status", dataType: "boolean", width: 100, },
            { dataField: "PKP", caption: "PKP", dataType: "boolean", width: 100, },
            { dataField: "TaxName", caption: "Tax Name", width: 150, },
            { dataField: "TaxAddress", caption: "Tax Address", width: 250, },
            { dataField: "TaxCity", caption: "Tax City", width: 150, },
            { dataField: "NPWP", caption: "NPWP", width: 200, },
            { dataField: "VendorType", caption: "Vendor Type", width: 120, visible:false },
            { dataField: "VndTypeDesc", caption: "Vendor Type Description", width: 200, },
            { dataField: "Term", caption: "Term", width: 100, },
            { dataField: "LeadTime", caption: "Lead Time", width: 100, },
            { dataField: "PriceCode", caption: "Price Code", width: 100, },
            { dataField: "InsertUser", caption: "Insert User", width: 150, visible: false }
        ],
        selection: {
            mode: "single"
        },
        columnAutoWidth: false,
        showRowLines: true,
        paging: {
            pageSize: 12
        },
        editing: {
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            mode: "popup"
        },
        pager: {
            visible: true,
            allowedPageSizes: [10, 50, 100, 'all'],
            showPageSizeSelector: true,
            showInfo: true,
            showNavigationButtons: true
        },
        toolbar: {
            items: [
                {
                    widget: "dxButton",
                    options: {
                        icon: "fa fa-file",
                        hint: "Add New InventoryId",
                        onClick: function () {
                            openPopupVendorAdd();
                        }
                    },
                    location: "before"
                }
            ]
        },
    }).dxDataGrid("instance");


    /*******************/
    //FORM Edit

    $("#myPopupVendorEdit").dxPopup({
        title: "Brand Edit",
        visible: false,
        width: 550,
        height: 280,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });
    function openPopupVendorEdit(vndcode) {
        $("#myPopupVendorEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Vendor/EditVendor',
                    type: 'GET',
                    data: { vendorcode: vndcode },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupInvBrandEdit").dxPopup("show");
    };

    //End: FORM Edit
    /*******************/

    /*******************/
    //FORM Add

    $("#myPopupVendorAdd").dxPopup({
        title: "Brand Add",
        visible: false,
        width: 550,
        height: 280,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvBrandAdd(vndcode) {
        $("#myPopupVendorAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryBrand/AddVendor',
                    type: 'GET',
                    data: { vendorcode: vndcode },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupInvBrandAdd").dxPopup("show");
    };

    //End: FORM Add
    /*******************/

    function deleteRow(vendorCode) {
        if (confirm("Are you sure you want to delete this brand?")) {
            $.ajax({
                url: 'Vendor/Delete',
                type: 'POST',
                data: { vendorcode:vendorCode },
                success: function (response) {
                    if (response.success) {
                        dataGridVendor.refresh();
                        alert("Brand deleted successfully.");
                    } else {
                        alert("Failed to delete brand.");
                    }
                },
                error: function (error) {
                    alert("Error: " + error);
                }
            });
        }
    };


    // Handle form submission Edit
    $('#editVendorForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Vendor/Update',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Vendor berhasil diperbarui!');
                $("#myPopupVendorEdit").dxPopup("hide");
                $("#dataGridVendor").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat memperbarui produk: ' + error);
            }
        });
    });

    // Handle form submission Edit
    $('#addVendorForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Vendor/Insert',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Vendor berhasil ditambahkan!');
                $("#myPopupVendorAdd").dxPopup("hide");
                $("#dataGridVendor").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat menambahkan produk: ' + error);
            }
        });
    });

});