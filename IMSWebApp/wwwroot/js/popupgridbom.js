
$(document).ready(function () {

    var treeView = $("#treeViewContainer").dxTreeView({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'VendorCode',
            loadUrl: 'Vendor/GetData',
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.headers = {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                    "Options": "BMU|"
                };
                ajaxOptions.xhrFields = { withCredentials: true };
            },
        }), 
        idField: "id",             
        parentIdField: "parentId", 
        displayExpr: "ItemName",       
        valueExpr: "ItemCode",           
        width: '100%',             
        height: 500,               
        showCheckBoxesMode: "normal", 
        onItemClick: function (e) {
            selectedNode = e.node;
        }
    }).dxTreeView("instance");

    var selectedNode;

    // Handle Add Item button
    $('#addItemBtn').on('click', function () {
        if (!selectedNode) {
            alert("Please select a parent node first.");
            return;
        }

        var newItem = {
            ItemCode: "NewCode",
            ItemName: "NewItem",
            ParentId: selectedNode.value, 
            QtyUsage: 1,
            Satuan: "PCS",
            LevelSeqn: selectedNode.level + 1 
        };

        $.ajax({
            url: '/api/items/Add',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(newItem),
            success: function (response) {
                alert(response.message);
                treeView.refresh(); 
            },
            error: function (error) {
                alert("Error adding item");
            }
        });
    });

    // Handle Edit Item button
    $('#editItemBtn').on('click', function () {
        if (!selectedNode) {
            alert("Please select an item to edit");
            return;
        }

        var updatedItem = {
            ItemCode: "UpdatedCode",
            ItemName: "UpdatedItem",
            ParentId: selectedNode.parentId,
            QtyUsage: 2,
            Satuan: "PCS",
            LevelSeqn: selectedNode.level // Use the selected node's level
        };

        $.ajax({
            url: '/api/items/Edit/' + selectedNode.value,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(updatedItem),
            success: function (response) {
                alert(response.message);
                treeView.refresh(); // Refresh TreeView after editing item
            },
            error: function (error) {
                alert("Error editing item");
            }
        });
    });

    // Handle Delete Item button
    $('#deleteItemBtn').on('click', function () {
        if (!selectedNode) {
            alert("Please select an item to delete");
            return;
        }

        $.ajax({
            url: '/api/items/Delete/' + selectedNode.value,
            type: 'DELETE',
            success: function (response) {
                alert(response.message);
                treeView.refresh(); // Refresh TreeView after deleting item
            },
            error: function (error) {
                alert("Error deleting item");
            }
        });
    });




    /*
    dataGridBOM = $("#dataGridBOM").dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'BOMCode',
            loadUrl: 'BillOfMaterial/GetData',
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
                            var bomId = e.row.data.Id;
                            //openPopupDetailBOM(inventoryId);
                        }
                    },
                    {
                        hint: "Edit",
                        icon: "fa fa-edit",
                        onClick: function (e) {
                            var bomId = e.row.data.Id;
                            //window.open("/Inventory/Edit/" + inventoryId, "_blank");
                            openPopupEditBOM(bomId);
                        }
                    },
                ]
            },
            //Id	BussCode	PlantCode	BOMCode	FGCode	FGName	BOMDescription
            { dataField: "Id", caption: "Id", visible: false },
            { dataField: "BussCode", caption: "BussCode", visible: false },
            { dataField: "PlantCode", caption: "PlantCode" },
            { dataField: "BOMCode", caption: "BOM Code" },
            { dataField: "FGCode", caption: "FG Code" },
            { dataField: "FGName", caption: "Material Code" },
            { dataField: "BOMDescription", caption: "Material Name" },
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
                        hint: "Add New BOM",
                        onClick: function () {
                            openPopupAddBOM();
                        }
                    },
                    location: "before"
                }
            ]
        }
    }).dxDataGrid("instance");
    */





});