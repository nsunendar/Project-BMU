$(function () {
    GetActiveBranch();
    $("#branch-options").change(function () {
        SetActiveBranch();
    });

    function GetActiveBranch() {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: "/Home/GetActiveBranch",
            data: {
                __RequestVerificationToken: token
            },
            success: function (data) {
                if (data == "0") {
                    SetActiveBranch();
                } else {
                    $("#branch-options").val(data);
                }
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    }

    function SetActiveBranch() {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: "/Home/SetActiveBranch",
            data: {
                __RequestVerificationToken: token,
                branchCode: $("#branch-options").val()
            },
            success: function (data) {
                if (data == "Ok") {    
                    location.reload();
                } else {
                    Swal.close()
                    Swal.fire({
                        icon: "info",
                        text: "Cannot set branch",
                    })
                }
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    }
});
