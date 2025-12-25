
$(document).ready(function () {

    const FGId = null;
    const FGCode = null;
    const FGName = null;

    dataGridFG = $("#dataGridFG").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'ItemCode',
            loadUrl: 'BillOfMaterial/GetDataLevelFG',
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
                            var bomId = e.row.data.Id;
                            openPopupBOMFGEdit(bomId);
                        }
                    },
                ],
                width: 50
            },
            { dataField: "ItemCode", caption: "Code", width: 120 },
            { dataField: "ItemName", caption: "Type Description" },
            { dataField: "QtyUsage", caption: "Qty", width: 50 },
            { dataField: "Satuan", caption: "EA", width: 50 },
            { dataField: "Id", caption: "ID", visible: false, width: 30 },
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
                        hint: "Add New FG",
                        onClick: function () {
                            openPopupBOMFGAdd();
                        }
                    },
                    location: "before"
                },
                {
                    template: '<div>Finish Good &nbsp;&nbsp;&nbsp;</div>',
                    location: "after"
                }
            ]
        },
        onSelectionChanged: function (e) {

            //FGId = e.row.data.Id;
            //FGCode = e.row.data.ItemCode;
            //FGName = e.row.data.ItemName;

            //FGId = e.selectedRowsData[0].id;
            //FGCode = e.selectedRowsData[0].itemCode;
            //FGName = e.selectedRowsData[0].itemName;

            dataGridRM.getDataSource().reload();
            dataGridCP.getDataSource().reload();
        }
    }).dxDataGrid("instance");

    var LevelRMStore = new DevExpress.data.CustomStore({
        key: "ItemCode",
        load: function (loadOptions) {
            var selectedData = dataGridFG.getSelectedRowKeys()[0];
            if (!selectedData) return [];
            return $.getJSON('BillOfMaterial/GetDataLevelRM', { parentItem: selectedData });
        }
    });

    var dataGridRM = $("#dataGridRM").dxDataGrid({
        dataSource: LevelRMStore,
        columns: [
            {
                type: "buttons",
                caption: " ",
                buttons: [
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var fdid = e.row.data.Id;
                            openPopupBOMRMEdit(fdid);
                        }
                    },
                ],
                width: 50
            },
            { dataField: "ItemCode", caption: "Code", width: 120 },
            { dataField: "ItemName", caption: "Type Description" },
            { dataField: "QtyUsage", caption: "Qty", width: 50 },
            { dataField: "Satuan", caption: "EA", width: 50 },
            { dataField: "Id", caption: "ID", visible: false, width: 30 },
        ],
        selection: {
            mode: "single"
        },
        columnAutoWidth: false,
        showRowLines: true,
        paging: {
            pageSize: 5
        },
        editing: {
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            mode: "popup"
        },
        pager: {
            visible: true,
            allowedPageSizes: [5, 10, 'all'],
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
                        hint: "Add New RM",
                        onClick: function () {
                            openPopupBOMRMAdd(FGId, FGCode, FGName);
                        }
                    },
                    location: "before"
                },
                {
                    template: '<div>Raw Material &nbsp;&nbsp;&nbsp;</div>',
                    location: "after"
                }
            ]
        },
        onSelectionChanged: function (e) {
            dataGridCP.getDataSource().reload();
        }
    }).dxDataGrid("instance");

    var LevelCPStore = new DevExpress.data.CustomStore({
        key: "ItemCode",
        load: function (loadOptions) {
            var selectedData = dataGridRM.getSelectedRowKeys()[0];
            if (!selectedData) return [];
            return $.getJSON('BillOfMaterial/GetDataLevelCP', { parentItem: selectedData });
        }
    });

    var dataGridCP = $("#dataGridCP").dxDataGrid({
        dataSource: LevelCPStore,
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
            { dataField: "ItemCode", caption: "Code", width: 120 },
            { dataField: "ItemName", caption: "Type Description" },
            { dataField: "QtyUsage", caption: "Qty", width: 50 },
            { dataField: "Satuan", caption: "EA", width: 50 },
            { dataField: "Id", caption: "ID", visible: false, width: 30 },
        ],
        columnAutoWidth: false,
        showRowLines: true,
        paging: {
            pageSize: 5
        },
        editing: {
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            mode: "popup"
        },
        pager: {
            visible: true,
            allowedPageSizes: [5, 10, 'all'],
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
                        hint: "Add New CP",
                        onClick: function () {
                            openPopupInvTypeAdd();
                        }
                    },
                    location: "before"
                },
                {
                    template: '<div>Child Parts &nbsp;&nbsp;&nbsp;</div>',
                    location: "after"
                }
            ]
        },
    }).dxDataGrid("instance");



    /*******************/
    //FORM FG

    $("#myPopupFGAdd").dxPopup({
        title: "BOM FG Add",
        visible: false,
        width: 750,
        height: 430,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupBOMFGAdd(bomId) {
        $("#myPopupFGAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'BillOfMaterial/AddFG',
                    type: 'GET',
                    data: { id: bomId },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupFGAdd").dxPopup("show");
    };

    $("#myPopupFGEdit").dxPopup({
        title: "BOM FG Updated",
        visible: false,
        width: 750,
        height: 430,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupBOMFGEdit(bomId) {
        $("#myPopupFGEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'BillOfMaterial/EditFG',
                    type: 'GET',
                    data: { id: bomId },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupFGEdit").dxPopup("show");
    };

    //RM
    $("#myPopupRMAdd").dxPopup({
        title: "BOM RM Add",
        visible: false,
        width: 750,
        height: 430,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupBOMRMAdd(fgId, fgCode, fgName) {
        console.log(fgId);
        console.log(fgCode);
        console.log(fgName);
        $("#myPopupRMAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'BillOfMaterial/AddRM',
                    type: 'GET',
                    data: { FgId: fgId, FGCode: fgCode, FGName: fgName },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupRMAdd").dxPopup("show");
    };
    //End: FORM FG
    /*******************/


});