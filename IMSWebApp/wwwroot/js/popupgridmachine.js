
$(document).ready(function () {

    //const token = $('input[name="__RequestVerificationToken"]').val();
    dataGridInventory = $("#dataGridMachine").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'MachineCode',
            loadUrl: 'Machine/GetData',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options": "BMU3|"
                };
                ajaxOptions.xhrFields = { withCredentials: true };
            },
        }),
        headerFilter: { visible: true, allowSearch: true, hideSelectAllOnSearch: false },
        columns: [
            {
                type: "buttons",
                caption: "Actions",
                buttons: [
                    {
                        hint: "View",
                        icon: "fa fa-eye",
                        onClick: function (e) {
                            var inventoryId = e.row.data.Id;
                            openPopupMachineDetail(inventoryId);
                        }
                    },
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var inventoryId = e.row.data.Id;
                            openPopupMachineEdit(inventoryId);
                        }
                    },
                ]
            },
            { dataField: 'Id', caption: 'ID', width: 10, visible: false },
            { dataField: 'BussCode', caption: 'Business Code', visible: false },
            { dataField: 'PlantCode', caption: 'Plant Code', visible: false },
            { dataField: 'MachineCode', caption: 'Machine Code', width: 140 },
            { dataField: 'MachineName', caption: 'Machine Name', width: 600 },
            { dataField: 'Status', caption: 'Status', dataType: 'boolean', width: 90 },
            { dataField: 'BuyDate', caption: 'Buy Date', dataType: 'date', width: 100 },
            { dataField: 'MaintDate', caption: 'Maintenance Date', dataType: 'date', width: 100 },
            { dataField: 'Usage', caption: 'Usage', width: 90 }
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
                        hint: "Add New",
                        onClick: function () {
                            openPopupMachineAdd();
                        }
                    },
                    location: "before"
                }
            ]
        }
    }).dxDataGrid("instance");



    /*******************/
    //FORM Detil
    function openPopupMachineDetail(inventoryId) {
        $("#myPopupMachineDetail").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Machine/DetailById',
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
        $("#myPopupMachineDetail").dxPopup("show");
    };

    $("#myPopupMachineDetail").dxPopup({
        title: "Machine Details",
        visible: false,
        width: 650,
        height: 460,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    //End: FORM Detil
    /*******************/


    /*******************/
    //FORM Edit

    $("#myPopupMachineEdit").dxPopup({
        title: "Machine Edit",
        visible: false,
        width: 650,
        height: 400,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupMachineEdit(inventoryId) {
        $("#myPopupMachineEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Machine/Edit',
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
        $("#myPopupMachineEdit").dxPopup("show");
    };

    //End: FORM Edit
    /*******************/

    /*******************/
    //FORM Add

    $("#myPopupMachineAdd").dxPopup({
        title: "Machine Add",
        visible: false,
        width: 650,
        height: 460,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupMachineAdd(inventoryId) {
        $("#myPopupMachineAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Machine/Add',
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
        $("#myPopupMachineAdd").dxPopup("show");
    };

    //End: FORM Add
    /*******************/

});