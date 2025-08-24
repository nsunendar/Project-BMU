
$(document).ready(function () {

    // Handle form submission Edit
    $('#editMachineForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Machine/Update',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Machine berhasil diperbarui!');
                $("#myPopupMachineEdit").dxPopup("hide");
                $("#dataGridMachine").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat memperbarui produk: ' + error);
            }
        });
    });

    // Handle form submission Edit
    $('#addMachineForm').on('submit', function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        $.ajax({
            url: 'Machine/Insert',
            type: 'POST',
            data: formData,
            //processData: false, 
            //contentType: false,
            success: function (response) {
                alert('Machine berhasil ditambahkan!');
                $("#myPopupMachineAdd").dxPopup("hide");
                $("#dataGridMachine").dxDataGrid("refresh");
            },
            error: function (xhr, status, error) {
                alert('Terjadi kesalahan saat menambahkan produk: ' + error);
            }
        });
    });

});