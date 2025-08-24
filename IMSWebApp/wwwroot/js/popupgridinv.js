
$(document).ready(function () {

    //const token = $('input[name="__RequestVerificationToken"]').val();
    dataGridInventory = $("#dataGridInventory").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'InvCode',
            loadUrl: 'Inventory/GetData',
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
                        hint: "View",
                        icon: "fa fa-eye",
                        onClick: function (e) {
                            var inventoryId = e.row.data.Id;
                            openPopupInvDetail(inventoryId);
                        }
                    },
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var inventoryId = e.row.data.Id;
                            openPopupInvEdit(inventoryId);
                        }
                    },
                ]
            },
            { dataField: "Id", visible: false },
            { dataField: "BussCode", caption: "Business Code", width: 80, visible: false },
            { dataField: "PlantCode", caption: "Plant Code", width: 100, visible: false },
            { dataField: "InvCode", caption: "Inventory Code", width: 120 },
            { dataField: "InvName", caption: "Inventory Name", width: 250 },
            { dataField: "RelasiCode", caption: "Relation", width: 150 },
            { dataField: "CSINVName", caption: "CSINV Name", width: 200 },
            { dataField: "InvStatus", caption: "Status", width: 60, dataType: "boolean" },
            { dataField: "Discontinue", caption: "Discontinue", width: 100, dataType: "boolean" },
            { dataField: "InvTypeDesc", caption: "Type", width: 80 },
            { dataField: "InvSubTypeDesc", caption: "SubType", width: 100 },
            { dataField: "BrandDesc", caption: "Brand", width: 70 },
            { dataField: "LargeUnitDesc", caption: "Large Unit", width: 100 },
            { dataField: "SmallUnitDesc", caption: "Small Unit", width: 100 },
            { dataField: "Crt", caption: "Crt", width: 80, allowFiltering: false },
            { dataField: "Fra", caption: "Fra", width: 80, allowFiltering: false },
            { dataField: "Norm", caption: "Norm", width: 80, allowFiltering: false },
            { dataField: "Process", caption: "Process", width: 80, visible: false },
            { dataField: "NoMachine", caption: "No Machine", width: 150, visible: false },
            { dataField: "People", caption: "People", width: 80, visible: false },
            { dataField: "CodeBOM", caption: "BOM Code", width: 100, visible: false },
            { dataField: "SalesPrice", caption: "Sales Price", width: 100, allowFiltering: false },
            { dataField: "BuyPrice", caption: "Buy Price", width: 100, allowFiltering: false },
            { dataField: "VendorName", caption: "Vendor", width: 200 },
            { dataField: "Barcode", caption: "Barcode", width: 150, allowFiltering: false },
            { dataField: "InvType", caption: "Type", width: 80, visible: false },
            { dataField: "InvSubType", caption: "SubType", width: 100, visible: false },
            { dataField: "Brand", caption: "Brand", width: 70, visible: false },
            { dataField: "LargeUnit", caption: "Large Unit", visible: false },
            { dataField: "SmallUnit", caption: "Small Unit", visible: false },
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
                            //window.open("/Customer/Add", "_blank");
                            openPopupInvAdd();
                        }
                    },
                    location: "before"
                }
            ]
        }
    }).dxDataGrid("instance");



    /*******************/
    //FORM Detil
    function openPopupInvDetail(inventoryId) {
        $("#myPopupInvDetail").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Inventory/DetailById',
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
        $("#myPopupInvDetail").dxPopup("show");
    };

    $("#myPopupInvDetail").dxPopup({
        title: "Inventory Details",
        visible: false,
        width: 950,
        height: 600,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    //End: FORM Detil
    /*******************/


    /*******************/
    //FORM Edit

    $("#myPopupInvEdit").dxPopup({
        title: "Inventory Edit",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvEdit(inventoryId) {
        $("#myPopupInvEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Inventory/Edit',
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
        $("#myPopupInvEdit").dxPopup("show");
    };

    //End: FORM Edit
    /*******************/

    /*******************/
    //FORM Add

    $("#myPopupInvAdd").dxPopup({
        title: "Inventory Add",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupInvAdd(inventoryId) {
        $("#myPopupInvAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Inventory/Add',
                    type: 'GET',
                    data: { id: inventoryId },
                    success: function (data) {
                        $('#BuyPrice').val(0);
                        $('#SalesPrice').val(0);
                        contentElement.html(data);
                    },
                    error: function (error) {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>" + error);
                    }
                });
            }
        });
        $("#myPopupInvAdd").dxPopup("show");
    };

    //End: FORM Add
    /*******************/

});