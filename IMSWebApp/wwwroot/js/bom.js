
$(document).ready(function () {


    $("#btnSaveHeader").click(function () {
        alert("Handler for .click() called.");
    });




});




////$(document).ready(function () {

////    //// Handle form submission Edit
////    //$('#editBOMForm').on('submit', function (e) {
////    //    e.preventDefault();
////    //    var formData = $(this).serialize();
////    //    console.log(formData);
////    //    $.ajax({
////    //        url: 'BillOfMaterial/UpdateBOMFG',
////    //        type: 'POST',
////    //        data: formData,
////    //        headers: { "RequestVerificationToken": $(this).find('input[name="__RequestVerificationToken"]').val() },
////    //        xhrFields: { withCredentials: true },
////    //        success: function (response) {
////    //            alert('BOM-FG berhasil diperbarui!');
////    //            $("#myPopupFGEdit").dxPopup("hide");
////    //            $("#dataGridFG").dxDataGrid("refresh");
////    //        },
////    //        error: function (xhr, status, error) {
////    //            alert('Terjadi kesalahan saat memperbarui produk: ' + error);
////    //        }
////    //    });
////    //});

////    //// Handle form submission Edit
////    //$('#addBOMForm').on('submit', function (e) {
////    //    e.preventDefault();
////    //    var formData = $(this).serialize();
////    //    console.log(formData);
////    //    $.ajax({
////    //        url: 'BillOfMaterial/InsertBOMFG',
////    //        type: 'POST',
////    //        data: formData,
////    //        headers: { "RequestVerificationToken": $(this).find('input[name="__RequestVerificationToken"]').val() },
////    //        xhrFields: { withCredentials: true },
////    //        success: function (response) {
////    //            alert('BOM-FG berhasil ditambahkan!');
////    //            $("#myPopupFGAdd").dxPopup("hide");
////    //            $("#dataGridFG").dxDataGrid("refresh");
////    //        },
////    //        error: function (xhr, status, error) {
////    //            alert('Terjadi kesalahan saat memperbarui produk: ' + error);
////    //        }
////    //    });
////    //});

////    $('input[name="ItemCode"]').dxAutocomplete({
////        dataSource: DevExpress.data.AspNet.createStore({
////            key: 'ItemCode',
////            loadUrl: 'BillOfMaterial/SearchInventory',
////            onBeforeSend(method, ajaxOptions) {
////                ajaxOptions.headers = {
////                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
////                };
////                ajaxOptions.xhrFields = { withCredentials: true };
////            },
////        }),
////        valueExpr: 'Value',
////        displayExpr: 'Text',
////        minSearchLength: 2,
////        onSelectionChanged: function (e) {
////            if (e.selectedItem) {
////                $('input[name="ItemName"]').val(e.selectedItem.name);
////            }
////        }
////    });



////    //$('input[name="ItemCode"]').autocomplete({
////    //    source: function (request, response) {
////    //        console.log(request.term);
////    //        $.ajax({
////    //            url: 'BillOfMaterial/SearchInventory',
////    //            data: { invcode: request.term },
////    //            headers: { "RequestVerificationToken": $(this).find('input[name="__RequestVerificationToken"]').val() },
////    //            xhrFields: { withCredentials: true },
////    //            success: function (data) {
////    //                response($.map(data, function (item) {
////    //                    return {
////    //                        label: item.label + (item.name ? ' - ' + item.name : ''),
////    //                        value: item.value,
////    //                        itemName: item.name
////    //                    };
////    //                }));
////    //            }
////    //        });
////    //    },
////    //    minLength: 2, // mulai autocomplete setelah ketik 2 huruf
////    //    select: function (event, ui) {
////    //        // set juga ItemName otomatis
////    //        $('input[name="ItemName"]').val(ui.item.name);
////    //    }
////    //});


////    //// Handle form submission Edit
////    //$('#addProductForm').on('submit', function (e) {
////    //    e.preventDefault();
////    //    var formData = $(this).serializeArray();
////    //    $.ajax({
////    //        url: 'Inventory/AddInventory',
////    //        type: 'POST',
////    //        data: formData,
////    //        //processData: false,
////    //        //contentType: false,
////    //        success: function (response) {
////    //            alert('Produk berhasil ditambahkan!');
////    //            $("#myPopupAdd").dxPopup("hide");
////    //            $("#dataGridInventory").dxDataGrid("refresh");
////    //        },
////    //        error: function (xhr, status, error) {
////    //            alert('Terjadi kesalahan saat menambahkan produk: ' + error);
////    //        }
////    //    });
////    //});


////    //// Populate Inventory Type (ComboBox)
////    //$.ajax({
////    //    url: 'BillOfMaterial/GetInventoryFG',
////    //    type: 'GET',
////    //    success: function (data) {
////    //        $('#FGCode').empty();
////    //        $('#FGCode').append('<option value="">Select Inventory</option>');
////    //        $.each(data, function (index, val) {
////    //            $('#FGCode').append('<option value="' + val.Value + '"> [' + val.Value +'] '+ val.Text + '</option>');
////    //        });
////    //        $('#FGCode').val(selFGCode);
////    //    }
////    //});

////    //const sBOMCode = document.getElementById("BOMCode").value;

////    //$("#bomMaterialGrid").dxDataGrid({
////    //    dataSource: DevExpress.data.AspNet.createStore({
////    //        key: 'BOMCode',
////    //        loadUrl: 'BillOfMaterial/GetBOMMaterials',
////    //        onBeforeSend(method, ajaxOptions) {
////    //            ajaxOptions.headers = {
////    //                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
////    //                "Options": "BMU3|" + sBOMCode
////    //            };
////    //            ajaxOptions.xhrFields = { withCredentials: true };
////    //        },
////    //    }),
////    //    columns: [
////    //        {
////    //            type: "buttons",
////    //            caption: "Actions",
////    //            buttons: [
////    //                {
////    //                    hint: "Edit",
////    //                    icon: "fa fa-edit",
////    //                    onClick: function (e) {
////    //                        var bomId = e.row.data.Id;
////    //                        //openPopupDetailBOM(inventoryId);
////    //                    }
////    //                },
////    //                {
////    //                    hint: "Remove",
////    //                    icon: "far fa-trash-alt",
////    //                    onClick: function (e) {
////    //                        var bomId = e.row.data.Id;
////    //                        //openPopupEditBOM(bomId);
////    //                    }
////    //                },
////    //            ]
////    //        },
////    //        { dataField: "MaterialCode", caption: "Material Code" },
////    //        { dataField: "MaterialName", caption: "Material Name" },
////    //        { dataField: "QtyUsage", caption: "Quantity" },
////    //        { dataField: "UnitDesc", caption: "UOM" },
////    //    ],
////    //    columnAutoWidth: false,
////    //    showRowLines: true,
////    //});

////    // GRID DETAIL ADD
////    //*********************/

////    let headerId = 0;

////    function setDetailEnabled(enabled) {
////        $("#gridDetail").dxDataGrid("instance")?.option("editing", {
////            mode: "row",
////            allowAdding: enabled,
////            allowUpdating: enabled,
////            allowDeleting: enabled
////        });
////        $("#detailHint").toggle(!enabled);
////    }

////    // Detail store
////    var detailStore = new DevExpress.data.AspNet.createStore({
////        key: 'Id',
////        loadUrl: 'BillOfMaterial/GetDataLevelFG',
////        onBeforeSend(method, ajaxOptions) {
////            ajaxOptions.headers = {
////                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
////            };
////            ajaxOptions.xhrFields = { withCredentials: true };
////        },
////    });
////    //const detailStore = new DevExpress.data.CustomStore({
////    //    key: "id",
////    //    load: () => headerId ? fetch(`${API_BASE}/api/bomheaders/${headerId}/details`).then(r => r.json()) : [],
////    //    insert: (values) => fetch(`${API_BASE}/api/bomheaders/${headerId}/details`, {
////    //        method: "POST",
////    //        headers: { "Content-Type": "application/json" },
////    //        body: JSON.stringify(values)
////    //    }).then(r => r.json()),
////    //    update: (key, values) => fetch(`${API_BASE}/api/bomdetails/${key}`, {
////    //        method: "PUT",
////    //        headers: { "Content-Type": "application/json" },
////    //        body: JSON.stringify({ id: key, parentId: headerId, ...values })
////    //    }),
////    //    remove: (key) => fetch(`${API_BASE}/api/bomdetails/${key}`, { method: "DELETE" })
////    //});

////    // Grid init
////    $("#gridDetail").dxDataGrid({
////        dataSource: detailStore,
////        showBorders: true,
////        columnAutoWidth: true,
////        height: 400,
////        editing: { mode: "row", allowAdding: !!headerId, allowUpdating: !!headerId, allowDeleting: !!headerId },
////        columns: [
////            { dataField: "id", caption: "ID", width: 70, allowEditing: false },
////            { dataField: "itemCode", caption: "Component Code" },
////            { dataField: "itemName", caption: "Component Name" },
////            { dataField: "qtyUsage", caption: "Qty", dataType: "number", format: "#,##0.####" },
////            { dataField: "satuan", caption: "UoM" },
////            { type: "buttons", buttons: ["edit", "delete"] }
////        ]
////    });
////    setDetailEnabled(!!headerId);

////    // Save header
////    $('#btnSaveHeader').on("click", async function (e) {
////        console.log('test2', 'test1');
////        e.preventDefault();
////        const payload = {
////            id: headerId,
////            itemCode: $("#ItemCode").val(),
////            itemName: $("#ItemName").val(),
////            plantCode: $("#PlantCode").val(),
////            bussCode: $("#BussCode").val()
////        };
////        if (!payload.itemCode) { alert("Item Code wajib diisi."); return; }

////        console.log('test2', test2);

////        // Create
////        //const res = await fetch('${API_BASE}/api/bomheaders', {
////        //    method: "POST",
////        //    headers: { "Content-Type": "application/json" },
////        //    body: JSON.stringify(payload)
////        //}).then(r => r.json());
////        //headerId = res.id;
////        setDetailEnabled(true);
////        $("#gridDetail").dxDataGrid("instance").refresh();
////        alert("Header berhasil dibuat.");
////    });

////    //$('#addBOMForm').on('submit', function (e) {
////    //    e.preventDefault();
////    //    var formData = $(this).serialize();
////    //    console.log(formData);
////    //    $.ajax({
////    //        url: 'BillOfMaterial/InsertBOMFG',
////    //        type: 'POST',
////    //        data: formData,
////    //        headers: { "RequestVerificationToken": $(this).find('input[name="__RequestVerificationToken"]').val() },
////    //        xhrFields: { withCredentials: true },
////    //        success: function (response) {
////    //            alert('BOM-FG berhasil ditambahkan!');
////    //            $("#myPopupFGAdd").dxPopup("hide");
////    //            $("#dataGridFG").dxDataGrid("refresh");
////    //        },
////    //        error: function (xhr, status, error) {
////    //            alert('Terjadi kesalahan saat memperbarui produk: ' + error);
////    //        }
////    //    });
////    //});

////});


//$(function () {
//    function antiForgery() {
//        return $('input[name="__RequestVerificationToken"]').val() || "";
//    };

//    function enableDetail() {
//        const grid = $("#gridDetail").dxDataGrid("instance");
//        if (!grid) return;
//        grid.option("editing", {mode: "row", allowAdding: true, allowUpdating: true, allowDeleting: true });
//        grid.refresh();
//        $("#detailHint").hide();
//    };

//    $("#btnSaveHeader").on("click", function (e) {
//        e.preventDefault();

//        var payload = {
//            id: 0,
//            itemCode: $("#ItemCode").val(),
//            itemName: $("#ItemName").val(),
//            plantCode: $("#PlantCode").val(),
//            bussCode: $("#BussCode").val()
//        };

//        console.log(payload);

//        if (!payload.itemCode) {
//            alert("Item Code wajib diisi.");
//        return;
//        }

//        enableDetail();

//        //// CREATE
//        //$.ajax({
//        //    url: "",
//        //    type: "POST",
//        //    data: JSON.stringify(payload),
//        //    contentType: "application/json; charset=utf-8",
//        //    headers: { "RequestVerificationToken": antiForgery() },
//        //    success: function (res) {
//        //        // res: { id: <newId> }
//        //        $("#Id").val(res.id);
//        //        enableDetail();
//        //        alert("Header berhasil dibuat.");
//        //    },
//        //    error: function (xhr) {
//        //        console.error(xhr);
//        //        alert("Gagal membuat header: " + (xhr.responseText || xhr.statusText));
//        //    }
//        //});

//    });
//});

