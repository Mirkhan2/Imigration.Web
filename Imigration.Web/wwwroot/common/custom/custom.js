function OpenAvatarInput() {
    $("#UserAvatar").click();
}

function UploadUserAvatar(url) {

    var avatarInput = document.getElementById("UserAvatar");

    if (avatarInput.files.length) {

        var file = avatarInput.files[0];

        var formData = new FormData();

        formData.append("userAvatar", file);

        $.ajax({
            url: url,
            type: "post",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                StartLoading('#UserInfoBox');
            },
            success: function (response) {
                EndLoading('#UserInfoBox');
                if (response.status === "Success") {
                    location.reload();
                } else {
                    swal({
                        title: "خطا",
                        text: "فرمت فایل ارسال شده معتبر نمی باشد .",
                        icon: "error",
                        button: "باشه"
                    });
                }
            },
            error: function () {
                EndLoading('#UserInfoBox');
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                    icon: "error",
                    button: "باشه"
                });
            }
        });
    }

}