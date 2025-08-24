
$(document).ready(function () {

    //const token = $('input[name="__RequestVerificationToken"]').val();
    dataGridProcess = $("#dataGridProcess").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'ProcCode',
            loadUrl: 'ProcessMachine/GetData',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options": "BMU3"
                };
                ajaxOptions.xhrFields = { withCredentials: true };
            },
        }),
        columns: [
            {
                type: "buttons",
                caption: "Actions",
                buttons: [
                    //{
                    //    hint: "View",
                    //    icon: "fa fa-eye",
                    //    onClick: function (e) {
                    //        var bomId = e.row.data.Id;
                    //        //openPopupDetailProcess(inventoryId);
                    //    }
                    //},
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var processId = e.row.data.Id;
                            //window.open("/Inventory/Edit/" + inventoryId, "_blank");
                            openPopupEditProcess(processId);
                        }
                    },
                    {
                        hint: "Delete",
                        icon: "trash",
                        onClick: function (e) {
                            if (confirm("Are you sure you want to delete this item?")) {
                                gridDataSource.remove(e.row.data.Id);
                            }
                        }
                    }
                ]
            },
            { dataField: 'Id', caption: 'ID', width: 50, visible:false },
            { dataField: 'BussCode', caption: 'Business Code' },
            { dataField: 'PlantCode', caption: 'Plant Code' },
            { dataField: 'ProcCode', caption: 'Process Code' },
            { dataField: 'NoProcess', caption: 'Process Number' },
            { dataField: 'CodeMachine', caption: 'Machine Code' },
            { dataField: 'QtyUsage', caption: 'Quantity Usage' },
            { dataField: 'TarProDay', caption: 'Tariff per Day' },
            { dataField: 'TarProHours', caption: 'Tariff per Hour' },
            { dataField: 'DateProcess', caption: 'Date Process', dataType: 'date' },
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
                        hint: "Add New Process",
                        onClick: function () {
                            openPopupAddProcess();
                        }
                    },
                    location: "before"
                }
            ]
        }
    }).dxDataGrid("instance");




    ///*******************/
    ////FORM Detil
    // function openPopupDetail(inventoryId) {
    //    $("#myPopupDetail").dxPopup("option", {
    //        contentTemplate: function (contentElement) {
    //            $.ajax({
    //                url: 'Inventory/DetailById',
    //                type: 'GET',
    //                data: { id: inventoryId },
    //                success: function (data) {
    //                    contentElement.html(data);
    //                },
    //                error: function (error) {
    //                    contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
    //                }
    //            });
    //        }
    //    });
    //    $("#myPopupDetail").dxPopup("show");
    //};

    //$("#myPopupDetail").dxPopup({
    //    title: "Inventory Details",
    //    visible: false,
    //    width: 950,
    //    height: 600,
    //    showCloseButton: true,
    //    dragEnabled: false,
    //    hideOnOutsideClick: false
    //});

    ////End: FORM Detil
    ///*******************/


    /*******************/
    //FORM Edit

    $("#myPopupEditProcess").dxPopup({
        title: "Process Machine Edit",
        visible: false,
        width: 750,
        height: 600,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupEditProcess(processId) {
        $("#myPopupEditProcess").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'ProcessMachine/Edit',
                    type: 'GET',
                    data: { id: processId },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupEditProcess").dxPopup("show");
    };

    //End: FORM Edit
    /*******************/

    /*******************/
    //FORM Add

    $("#myPopupAddProcess").dxPopup({
        title: "Process Machine Add",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupAddProcess() {
        $("#myPopupAddProcess").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'ProcessMachine/Add',
                    type: 'GET',
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupAddProcess").dxPopup("show");
    };

    //End: FORM Add
    /*******************/



});