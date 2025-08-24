
$(document).ready(function () {
    dataGridInvType = $("#dataGridInvBrand").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'BrandCode',
            loadUrl: 'InventoryBrand/GetData',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
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
                            var invBrand = e.row.data.BrandCode;
                            openPopupInvBrandEdit(invBrand);
                        }
                    },
                    {
                        hint: "Delete",
                        icon: "fa fa-trash",
                        onClick: function (e) {
                            var invBrand = e.row.data.BrandCode;
                            deleteRow(invBrand);
                        }
                    },
                ],
                width: 70
            },
            { dataField: "BrandCode", caption: "Code", width: 50},
            { dataField: "BrandDesc", caption: "Description"  },
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
                            openPopupInvBrandAdd();
                        }
                    },
                    location: "before"
                }
            ]
        },
    }).dxDataGrid("instance");


    /*******************/
    //FORM Edit

    $("#myPopupInvBrandEdit").dxPopup({
        title: "Brand Edit",
        visible: false,
        width: 550,
        height: 280,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });
    function openPopupInvBrandEdit(brand) {
        $("#myPopupInvBrandEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryBrand/EditInvBrand',
                    type: 'GET',
                    data: { brandcode: brand },
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

    $("#myPopupInvBrandAdd").dxPopup({
        title: "Brand Add",
        visible: false,
        width: 550,
        height: 280,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvBrandAdd(brand) {
        $("#myPopupInvBrandAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryBrand/AddInvBrand',
                    type: 'GET',
                    data: { brandCode: brand },
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

    function deleteRow(brandCode) {
        if (confirm("Are you sure you want to delete this brand?")) {
            $.ajax({
                url: 'InventoryBrand/Delete',
                type: 'POST',
                data: { brandCode: brandCode },
                success: function (response) {
                    if (response.success) {
                        dataGridInvType.refresh();
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
    $('#editInvBrandForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'InventoryBrand/Update',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Brand berhasil diperbarui!');
                $("#myPopupInvBrandEdit").dxPopup("hide");
                $("#dataGridInvBrand").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat memperbarui produk: ' + error);
            }
        });
    });

    // Handle form submission Edit
    $('#addInvBrandForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'InventoryBrand/Add',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Brand berhasil ditambahkan!');
                $("#myPopupInvBrandAdd").dxPopup("hide");
                $("#dataGridInvBrand").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat menambahkan produk: ' + error);
            }
        });
    });

});