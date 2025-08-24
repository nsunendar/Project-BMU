
$(document).ready(function () {
    
    dataGridCustomer = $("#dataGridCustomer").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'CustCode',
            loadUrl: 'Customer/GetData',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options": "BMU3|"
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
                        hint: "View",
                        icon: "fa fa-eye",
                        onClick: function (e) {
                            var customerId = e.row.data.Id;
                            openPopupCustDetail(customerId);
                        }
                    },
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var customerId = e.row.data.Id;
                            openPopupCustEdit(customerId);
                        }
                    }
                ]
            },
            { dataField: "Id", caption: "Id" },
            { dataField: "CustCode", caption: "Code", width: 80 },
            { dataField: "CustName", caption: "Customer Name", width: 300 },
            { dataField: "CustAddress", caption: "Customer Address", width: 350 },
            { dataField: "City", caption: "City", width: 150 },
            { dataField: "Status", caption: "Sts", width: 40 },
            { dataField: "Phone", caption: "Phone", width: 150 },
            { dataField: "Email", caption: "Email", width: 200 },
            { dataField: "OwnerName", caption: "Owner Name", width: 200 },
            { dataField: "PKP", caption: "PKP", width: 40 },
            { dataField: "TaxName", caption: "Tax Name", width: 200 },
            { dataField: "TaxAddress", caption: "Tax Address", width: 250 },
            { dataField: "TaxCity", caption: "Tax City", width: 150 },
            { dataField: "NPWP", caption: "NPWP", width: 130 },
            { dataField: "PriceCode", caption: "Prc", width: 45 },
            { dataField: "JoinDate", caption: "Join Date", width: 100 },
            { dataField: "CsTypeDesc", caption: "Customer Type", width: 150 },
            { dataField: "CsSubTypeDesc", caption: "Sub Type", width: 150 },
            { dataField: "CsAreaDesc", caption: "Area", width: 150 },
            { dataField: "SlsName", caption: "Salesman", width: 200 },
            { dataField: "Term", caption: "Term", width: 60 }
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
                        hint: "Add New Customer",
                        onClick: function () {
                            openPopupCustAdd();
                        }
                    },
                    location: "before"
                }
            ]
        }
    }).dxDataGrid("instance");

    $("#myPopupCustDetail").dxPopup({
        title: "Customer Details",
        visible: false,
        width: 950,
        height: 600,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });
    function openPopupCustDetail(customerId) {
        $("#myPopupCustDetail").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Customer/DetailById', 
                    type: 'GET',
                    data: { id: customerId },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function () {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>");
                    }
                });
            }
        });
        $("#myPopupCustDetail").dxPopup("show");
    };

    $("#myPopupCustEdit").dxPopup({
        title: "Customer Edit",
        visible: false,
        width: 950,
        height: 700,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });
    function openPopupCustEdit(customerId) {
        $("#myPopupCustEdit").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Customer/Edit',
                    type: 'GET',
                    data: { id: customerId },
                    success: function (data) {
                        contentElement.html(data);
                    },
                    error: function () {
                        contentElement.html("<p style='color:red'>Gagal memuat data.</p>");
                    }
                });
            }
        });
        $("#myPopupCustEdit").dxPopup("show");
    };


    $("#myPopupCustAdd").dxPopup({
        title: "Customer Add",
        visible: false,
        width: 950,
        height: 680,
        showCloseButton: true,
        dragEnabled: false,
        hideOnOutsideClick: false
    });

    function openPopupCustAdd(inventoryId) {
        $("#myPopupCustAdd").dxPopup("option", {
            contentTemplate: function (contentElement) {
                $.ajax({
                    url: 'Customer/Add',
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
        $("#myPopupCustAdd").dxPopup("show");
    };







});