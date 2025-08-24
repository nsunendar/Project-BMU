
$(document).ready(function () {
    dataGridInvType = $("#dataGridInvType").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'InvType',
            loadUrl: 'InventoryType/GetDataType',
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
                            var invType = e.row.data.Id;
                            openPopupInvTypeEdit(invType);
                        }
                    },
                ],
                width: 50
            },
            { dataField: "InvType", caption: "Code", width: 50},
            { dataField: "InvTypeDesc", caption: "Type Description"  },
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
                            openPopupInvTypeAdd();
                        }
                    },
                    location: "before"
                }
            ]
        },
        onSelectionChanged: function (e) {
            dataGridInvSubType.getDataSource().reload();
        }
    }).dxDataGrid("instance");

    var InvSubTypeStore = new DevExpress.data.CustomStore({
        key: "InvSubType",
        load: function (loadOptions) {
            var selectedData = dataGridInvType.getSelectedRowKeys()[0];
            if (!selectedData) return [];
            return $.getJSON('InventoryType/GetDataSubType', { invtype: selectedData });
        }
    });

    var dataGridInvSubType = $("#dataGridInvSubType").dxDataGrid({
        dataSource: InvSubTypeStore,
        columns: [
            {
                type: "buttons",
                caption: " ",
                buttons: [
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var invType = e.row.data.Id;
                            openPopupInvTypeEdit(invType);
                        }
                    },
                ],
                width: 50
            },
            { dataField: 'InvSubType', caption: 'Code', width: 50 },
            { dataField: 'InvSubTypeDesc', caption: 'SubType Description' }
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
                            openPopupInvTypeAdd();
                        }
                    },
                    location: "before"
                }
            ]
        },
    }).dxDataGrid("instance");


    /*******************/
    //FORM Edit

    $("#myPopupInvTypeEdit").dxPopup({
        title: "Type Edit",
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
                    url: 'InventoryType/EditInvType',
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
        title: "Inventory Add",
        visible: false,
        width: 350,
        height: 280,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvTypeAdd(inventoryId) {
        $("#myPopupInvTypeAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryType/AddInvType',
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
        $("#myPopupInvTypeAdd").dxPopup("show");
    };

    //End: FORM Add
    /*******************/


    /*******************/
    //FORM Edit

    $("#myPopupInvSubTypeEdit").dxPopup({
        title: "Type Edit",
        visible: false,
        width: 350,
        height: 280,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });
    function openPopupInvSubTypeEdit(inventoryId) {
        $("#myPopupInvSubTypeEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryType/EditInvSubType',
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
        $("#myPopupInvSubTypeEdit").dxPopup("show");
    };

    //End: FORM Edit
    /*******************/

    /*******************/
    //FORM Add

    $("#myPopupInvSubTypeAdd").dxPopup({
        title: "Sub Type Add",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvSubTypeAdd(inventoryId) {
        $("#myPopupInvSubTypeAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'InventoryType/AddInvSubType',
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
        $("#myPopupInvSubTypeAdd").dxPopup("show");
    };

    //End: FORM Add
    /*******************/

});