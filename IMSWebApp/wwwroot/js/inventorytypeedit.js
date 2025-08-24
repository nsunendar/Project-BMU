
$(document).ready(function () {

    // Handle form submission Edit
    $('#editInvTypeForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Inventory/UpdInventory',
            type: 'POST',
            data: formData,
            success: function (response) {
                alert('Inv Type berhasil diperbarui!');
                $("#myPopupInvTypeEdit").dxPopup("hide");
                $("#dataGridInvType").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat memperbarui produk: ' + error);
            }
        });
    });

    // Handle form submission Edit
    $('#addInvTypeForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Inventory/AddInventory',
            type: 'POST',
            data: formData,
            success: function (response) {
                alert('Inv Type berhasil ditambahkan!');
                $("#myPopupInvTypeAdd").dxPopup("hide");
                $("#dataGridInvType").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat menambahkan produk: ' + error);
            }
        });
    });

});