
$(document).ready(function () {
    dataGridInvSubType = $("#dataGridInvSubType").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'InvCode',
            loadUrl: 'InventoryType/GetDataSubType',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options": "BMU3|"
                };
                ajaxOptions.xhrFields = { withCredentials: true };
            },
        }),
        headerFilter: {
            visible: true,
            search: {
                enabled: true
            },
            hideSelectAllOnSearch: false
        },
        columns: [
            {
                type: "buttons",
                caption: "Actions",
                buttons: [
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var invType = e.row.data.Id;
                            openPopupInvTypeEdit(invType);
                        }
                    },
                ]
            },
            { dataField: "InvType", caption: "Type", width: 80, visible: false },
            { dataField: "InvTypeDesc", caption: "Description", width: 100, visible: false },
        ],
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
            allowedPageSizes: [12, 20, 50, 'all'],
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
                            openPopupInvTypeAdd();
                        }
                    },
                    location: "before"
                }
            ]
        }
    }).dxDataGrid("instance");

    
    /*******************/
    //FORM Edit

    $("#myPopupInvTypeEdit").dxPopup({
        title: "Inventory Type Edit",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvTypeEdit(inventoryId) {
        $("#myPopupInvTypeEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryType/EditType',
                    type: 'GET',
                    data: { id: inventoryId },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupInvTypeEdit").dxPopup("show");
    };

    //End: FORM Edit
    /*******************/

    /*******************/
    //FORM Add

    $("#myPopupInvTypeAdd").dxPopup({
        title: "Inventory Type Add",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvTypeAdd(invtype) {
        $("#myPopupInvTypeAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryType/AddType',
                    type: 'GET',
                    data: { id: invtype },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupInvTypeAdd").dxPopup("show");
    };

    //End: FORM Add
    /*******************/

});