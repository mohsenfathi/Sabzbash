var MessageType = { Succes: "#1bb394", Error: "#ed5666", Warning: "#f8ac5a", Information: "#1d84c6" };
var CKEditorToolbars = [
    ['Save', 'Preview', 'Print', 'NewPage'],
    ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'Undo', 'Redo'],
    ['Find', 'Replace', 'SelectAll', 'Scayt'],
    ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'],
    ['NumberedList', 'BulletedList', 'Indent', 'Outdent', 'Blockquote', 'CreateDiv', 'JustifyLeft', 'JustifyRight', 'JustifyCenter', 'JustifyBlock', 'BidiLtr', 'BidiRtl'],
    ['Link', 'Unlink', 'Anchor'],
    ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'],
    ['Styles', 'Format', 'Font', 'FontSize'],
    ['UIColor', 'Maximize', 'ShowBlocks'],
    ['Table', 'Image', 'Flash'],
    ['TextColor', 'BGColor'],
];

$(document).ready(function (e) {
    $("#AllLightBoxAlert").fadeOut(0);
    $("#AllLightBoxConfirm").fadeOut(0);
    $(".txtDate").persianDatepicker();
    setNumberValidationEvent();
});

function CallMethod(PageUrl, dataParam, ParentDivId, SuccessFunction) {
    var peogressSection = document.createElement('div');
    $(peogressSection).addClass("AllLoeading");
    $(peogressSection).html("<svg class='spinner' width='100px' height='100px' viewBox='0 0 66 66' xmlns='http://www.w3.org/2000/svg'><circle class='path' fill='none' stroke-width='6' stroke-linecap='round' cx='33' cy='33' r='30'></circle></svg>");
    $("#" + ParentDivId).append(peogressSection);
    $(peogressSection).fadeOut(0);
    $(peogressSection).fadeIn(500);
    $.ajax({
        type: "POST",
        url: PageUrl,
        data: dataParam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        timeout: 999999,
        success: function (response) {
            $(peogressSection).fadeOut(250, function () {
                $(peogressSection).replaceWith("");
                if (response == 'Error') {
                    ShowMessage("خطا", "خطایی در سیستم به وجود امده", MessageType.Error, null, null);
                    return;
                }
                if (response.hasOwnProperty("LogMessage") == true) {
                    ShowMessage("خطا", response["LogMessage"], MessageType.Warning, null, null);
                    return;
                }
                SuccessFunction(response);
            });
        },
        failure: function (response) {
            $(peogressSection).fadeOut(250, function () {
                $(peogressSection).replaceWith("");
                ShowMessage("خطا", "خطا در اتصال به سرویس دهنده", MessageType.Error, null, null);
            });
        }
    });
}

function ShowMessage(Title, Content, Type, OkFunction, CancelFunction) {
    if (OkFunction == null || OkFunction == undefined)
        OkFunction = function () { };
    if (CancelFunction == null || CancelFunction == undefined)
        CancelFunction = function () { };
    $("#btnAlertLightBoxOk").off("click");
    $("#btnAlertLightBoxOk").click(function () {
        $("#LightBoxAlertContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxAlert").fadeOut(150);
        OkFunction();
    });
    $("#AllLightBoxAlert").off("click");
    $("#AllLightBoxAlert").click(function (e) {
        if (e.target !== e.currentTarget)
            return;
        $("#LightBoxAlertContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxAlert").fadeOut(150);
        CancelFunction();
    });
    $("#imgAlertLightBoxCancel").off("click");
    $("#imgAlertLightBoxCancel").click(function () {
        $("#LightBoxAlertContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxAlert").fadeOut(150);
        CancelFunction();
    });
    var nowDate = new Date();
    $("#spnAlertLightBoxContentTime").html("این پیغام در ساعت " + nowDate.getSeconds() + " : " + nowDate.getMinutes() + " : " + nowDate.getHours() + " نمایش داده شده است");
    $("#spnAlertLightBoxHeader").html(Title);
    $("#spnAlertLightBoxContent").html(Content);
    $("#LightBoxAlertContent").addClass("AllLightBoxContetnShow");
    $(".AllLightBoxHeader").css("background", Type);
    $("#AllLightBoxAlert").fadeIn(150);
}

function ShowConfirm(Title, Content, Type, OkFunction, CancelFunction) {
    if (OkFunction == null || OkFunction == undefined)
        OkFunction = function () { };
    if (CancelFunction == null || CancelFunction == undefined)
        CancelFunction = function () { };
    $("#btnBoxConfirmLightBoxYes").off("click");
    $("#btnBoxConfirmLightBoxYes").click(function () {
        $("#LightBoxConfirmContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        OkFunction();
    });
    $("#btnConfirmLightBoxCancel").off("click");
    $("#btnConfirmLightBoxCancel").click(function () {
        $("#LightBoxConfirmContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        CancelFunction();
    });
    $("#AllLightBoxConfirm").off("click");
    $("#AllLightBoxConfirm").click(function (e) {
        if (e.target !== e.currentTarget)
            return;
        $("#LightBoxConfirmContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        CancelFunction();
    });
    $("#imgConfirmLightBoxCancel").off("click");
    $("#imgConfirmLightBoxCancel").click(function () {
        $("#LightBoxConfirmContent").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        CancelFunction();
    });
    var nowDate = new Date();
    $("#spnConfirmLightBoxContentTime").html("این پیغام در ساعت " + nowDate.getSeconds() + " : " + nowDate.getMinutes() + " : " + nowDate.getHours() + " نمایش داده شده است");
    $("#spnConfirmLightBoxHeader").html(Title);
    $("#spnConfirmLightBoxContent").html(Content);
    $("#LightBoxConfirmContent").addClass("AllLightBoxContetnShow");
    $(".AllLightBoxHeader").css("background", Type);
    $("#AllLightBoxConfirm").fadeIn(150);
}

$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
}

function validateForm() {
    var result = true;
    $("*[mf-required]").each(function (index, element) {
        if ($(this).is("input") || $(this).is("select")) {
            if ($(this).val() == "" || $(this).val() == null || ($(this).is("select") && $(this).val() == "0")) {
                if ($(this).hasAttr("disabled") == false) {
                    result = false;
                    $(this).addClass("RequiredFieldError");
                }
            }
            else
                $(this).removeClass("RequiredFieldError");
        }
    });
    $("*[mf-required]").off("blur");
    $("*[mf-required]").blur(function (e) {
        if ($(this).val() == "" || $(this).val() == null || ($(this).is("select") && $(this).val() == "0")) {
            if ($(this).hasAttr("disabled") == false) {
                $(this).addClass("RequiredFieldError");
            }
        }
        else
            $(this).removeClass("RequiredFieldError");
    });
    if (!result)
        ShowMessage("توجه", "لطفا فیلدهای اجباری را تکمیل کنید", MessageType.Warning);
    return result;
}

function setNumberValidationEvent() {
    $('input[mf-type=IntNumber]').off('keydown');
    $('input[mf-type=number]').off('keydown');
    $('input[mf-type=IntNumber]').off('keyup');
    $('input[mf-type=number]').off('keyup');
    $("input[mf-type=number]").on('keydown', function (e) {
        if ((e.keyCode == 110 || e.keyCode == 190) && ($(this).val().indexOf('.') != -1 || $(this).val() == ''))/* for second '.' character */
            return false;
        if (!((e.keyCode == 86 || e.keyCode == 67)))/* vKey = 86 & cKey = 67 */ {
            if (e.keyCode != 109 && e.keyCode != 116) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 110, 190]) !== -1 ||
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    return;
                }
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            }
            else {
                if (e.keyCode == 109 && $(this).val().length > 0)
                    return false;
            }
        }
    }).on('keyup', function () {
        if (e.keyCode == 37 || e.keyCode == 38 || e.keyCode == 39 || e.keyCode == 40)
            return;
        $(this).val($(this).val().replace(/[^\d.-]/g, ''));
        if ($(this).hasClass("txtComma"))
            addCommaIntoNumberInput(this);
    });
    $("input[mf-type=IntNumber]").on('keydown', function (e) {
        if ((e.keyCode == 110 || e.keyCode == 190))/* for '.' character */
            return false;
        if (!((e.keyCode == 86 || e.keyCode == 67)))/* vKey = 86 & cKey = 67 */ {
            if (e.keyCode == 109 || e.keyCode == 189) /* for '-' character */
                return false;
            if (!(e.keyCode >= 37 && e.keyCode <= 40)) /* for '-' Arrows */
                return;
            if (e.keyCode != 109 && e.keyCode != 116) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 110, 190]) !== -1 ||
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    return;
                }
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            }
        }
    }).on('keyup', function (e) {
        if (e.keyCode == 37 || e.keyCode == 38 || e.keyCode == 39 || e.keyCode == 40)
            return;
        $(this).val($(this).val().replace(/[^\d]/g, ''));
        if ($(this).hasClass("txtComma"))
            addCommaIntoNumberInput(this);
    });
}

function addCommaIntoNumberInput(element) {
    var start = element.selectionStart, end = element.selectionEnd;
    var num = $(element).val().replace(/,/g, '');
    num += '';
    var x = num.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    $(element).val(x1 + x2);
}

function addCommaIntoNumber(num) {
    num += '';
    num = num.replace(/,/g, '');
    var x = num.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function setCommaEvent() {
    $(".txtComma[mtype!='number'][mtype!='IntNumber']").off('keyup');
    $(".txtComma[mtype!='number'][mtype!='IntNumber']").keyup(function (e) {
        addCommaIntoNumberInput(this);
    })
}

function UploadChangeEvent(element, IsImage, DefaultImageName, PreviewElemenetId, ImageAddress) {
    if (ImageAddress == undefined)
        ImageAddress = "/Areas/Admin/Images/Profile/";
    if ($(element).val() == "") {
        $(".fileUploadAddress[mf-upload-role=" + $(element).attr("mf-upload-role") + "]").html("فایلی انتخاب نشده");
        if (IsImage)
            $("#" + PreviewElemenetId).attr("src", ImageAddress + DefaultImageName);
        return;
    }

    var isValidFile = true;

    if (IsImage == null || IsImage == undefined)
        IsImage = false;
    var address = element.value.split('\\');
    var fileExt = element.value;
    fileExt = fileExt.substring(fileExt.lastIndexOf('.'));

    if ($(element).attr("mf-accept") != "*") {
        var extentions = $(element).attr("mf-accept").split(',');
        if (extentions != undefined && extentions != null && extentions != "" && extentions.length != 0) {
            var isValiExt = false;
            $(extentions).each(function () {
                if (("." + this).toUpperCase() == fileExt.toUpperCase())
                    isValiExt = true;
            });
            if (!isValiExt) {
                $(".fileUploadAddress[mf-upload-role=" + $(element).attr("mf-upload-role") + "]").html("پسوند فایل صحیح نیست");
                ShowMessage("خطا", "پسوند فایل صحیح نیست , پسوندهای قابل قبول " + $(element).attr("mf-accept"), MessageType.Warning, null, null);
                isValidFile = false;
            }
        }
        else
            extentions = "";
    }
    var maxLenght = $(element).attr("mf-max-lenght");
    if (maxLenght != undefined && maxLenght != null && maxLenght != "" && maxLenght.length != 0) {
        var files = $(element).get(0).files;
        if (((files[0].size / 1024) / 1024) > maxLenght) {
            $(".fileUploadAddress[mf-upload-role=" + $(element).attr("mf-upload-role") + "]").html("حجم فایل باید از " + maxLenght + " مگابایت کمتر باشد");
            ShowMessage("خطا", "حجم فایل باید از " + maxLenght + " مگابایت کمتر باشد", MessageType.Warning, null, null);
            isValidFile = false;
        }
    }

    if (IsImage) {
        var _URL = window.URL || window.webkitURL;
        var imageSize = $(element).attr("mf-image-size");
        if (imageSize != undefined && imageSize != null && imageSize != "" && imageSize.length != 0) {
            imageSize = imageSize.split('*');
            var file, img;
            if ((file = element.files[0])) {
                img = new Image();
                img.onload = function () {
                    if (this.width != imageSize[0] && this.height != imageSize[1]) {
                        $(".fileUploadAddress[mf-upload-role=" + $(element).attr("mf-upload-role") + "]").html("اندازه تصویر مجاز نیست");
                        ShowMessage("خطا", "طول تصویر باید " + imageSize[0] + " و عرض آن " + imageSize[1] + " پیکسل باشد", MessageType.Warning, null, null);
                        $(element).val("");
                        $("#" + PreviewElemenetId).attr("src", ImageAddress + DefaultImageName);
                        isValidFile = false;
                    }
                };
                img.src = _URL.createObjectURL(file);
            }
        }
    }

    if (isValidFile) {
        $(".fileUploadAddress[mf-upload-role=" + $(element).attr("mf-upload-role") + "]").html(address[address.length - 1]);
        if (IsImage)
            PreviewImage(element, $("#" + PreviewElemenetId));
    }
    else {
        $(element).val("");
        if (IsImage)
            $("#" + PreviewElemenetId).attr("src", ImageAddress + DefaultImageName);
    }
}

function PreviewImage(input, outPut) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            outPut.attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function UploadFile(FunctionUrl, FilesInput, ParentDivId, SuccessFunction) {
    var peogressSection = document.createElement('div');
    $(peogressSection).addClass("AllLoeading");
    $(peogressSection).html("<svg class='spinner' width='100px' height='100px' viewBox='0 0 66 66' xmlns='http://www.w3.org/2000/svg'><circle class='path' fill='none' stroke-width='6' stroke-linecap='round' cx='33' cy='33' r='30'></circle></svg>");
    $("#" + ParentDivId).append(peogressSection);
    $(peogressSection).fadeOut(0);
    $(peogressSection).fadeIn(200);
    var formdata = new FormData();
    for (i = 0; i < FilesInput.files.length; i++) {
        formdata.append(FilesInput.files[i].name, FilesInput.files[i]);
    }
    var xhr = new XMLHttpRequest();
    xhr.open('POST', FunctionUrl);
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $(peogressSection).replaceWith("");
            var res;
            try {
                res = xhr.responseText;
                if (res == 'Error') {
                    ShowMessage("خطا", "خطا در آپلود فایل", MessageType.Error, null, null);
                    return;
                }
                if (response.hasOwnProperty("LogMessage") == true) {
                    ShowMessage("خطا", response["LogMessage"], MessageType.Warning, null, null);
                    return;
                }
            } catch (e) { }
            SuccessFunction(xhr.responseText);
        }
        $(peogressSection).replaceWith("");
    }
}

function UploadFileWithHeader(FunctionUrl, FilesInput, ParentDivId, SuccessFunction, HeaderKeyValues) {
    var peogressSection = document.createElement('div');
    $(peogressSection).addClass("AllLoeading");
    $(peogressSection).html("<svg class='spinner' width='100px' height='100px' viewBox='0 0 66 66' xmlns='http://www.w3.org/2000/svg'><circle class='path' fill='none' stroke-width='6' stroke-linecap='round' cx='33' cy='33' r='30'></circle></svg>");
    $("#" + ParentDivId).append(peogressSection);
    $(peogressSection).fadeOut(0);
    $(peogressSection).fadeIn(200);
    var formdata = new FormData();
    for (i = 0; i < FilesInput.files.length; i++) {
        formdata.append(FilesInput.files[i].name, FilesInput.files[i]);
    }
    var xhr = new XMLHttpRequest();
    xhr.open('POST', FunctionUrl);
    for (var i = 0; i < HeaderKeyValues.length; i++) {
        xhr.setRequestHeader(HeaderKeyValues[i].Key, HeaderKeyValues[i].Value);
    }
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $(peogressSection).replaceWith("");
            var res;
            try {
                res = xhr.responseText;
                if (res == 'Error') {
                    ShowMessage("خطا", "خطا در آپلود فایل", MessageType.Error, null, null);
                    return;
                }
                if (response.hasOwnProperty("LogMessage") == true) {
                    ShowMessage("خطا", response["LogMessage"], MessageType.Warning, null, null);
                    return;
                }
            } catch (e) { }
            SuccessFunction(xhr.responseText);
        }
        $(peogressSection).replaceWith("");
    }
}

// ~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/ =| GetDatasFunctions |= \~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/

function ConverMiladiToShamsi(Year, Month, Day) {
    Year = parseInt(Year);
    Month = parseInt(Month);
    Day = parseInt(Day);
    g_d_m = [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];
    jy = (Year <= 1600) ? 0 : 979;
    Year -= (Year <= 1600) ? 621 : 1600;
    gy2 = (Month > 2) ? (Year + 1) : Year;
    days = (365 * Year) + (parseInt((gy2 + 3) / 4)) - (parseInt((gy2 + 99) / 100))
        + (parseInt((gy2 + 399) / 400)) - 80 + Day + g_d_m[Month - 1];
    jy += 33 * (parseInt(days / 12053));
    days %= 12053;
    jy += 4 * (parseInt(days / 1461));
    days %= 1461;
    jy += parseInt((days - 1) / 365);
    if (days > 365) days = (days - 1) % 365;
    jm = (days < 186) ? 1 + parseInt(days / 31) : 7 + parseInt((days - 186) / 30);
    jd = 1 + ((days < 186) ? (days % 31) : ((days - 186) % 30));
    return jy + "/" + jm + "/" + jd;
}

function ConverShamsiToMiladi(Year, Month, Day) {
    Year = parseInt(Year);
    Month = parseInt(Month);
    Day = parseInt(Day);
    gy = (Year <= 979) ? 621 : 1600;
    Year -= (Year <= 979) ? 0 : 979;
    days = (365 * Year) + ((parseInt(Year / 33)) * 8) + (parseInt(((Year % 33) + 3) / 4))
        + 78 + Day + ((Month < 7) ? (Month - 1) * 31 : ((Month - 7) * 30) + 186);
    gy += 400 * (parseInt(days / 146097));
    days %= 146097;
    if (days > 36524) {
        gy += 100 * (parseInt(--days / 36524));
        days %= 36524;
        if (days >= 365) days++;
    }
    gy += 4 * (parseInt((days) / 1461));
    days %= 1461;
    gy += parseInt((days - 1) / 365);
    if (days > 365) days = (days - 1) % 365;
    gd = days + 1;
    sal_a = [0, 31, ((gy % 4 == 0 && gy % 100 != 0) || (gy % 400 == 0)) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    for (gm = 0; gm < 13; gm++) {
        v = sal_a[gm];
        if (gd <= v) break;
        gd -= v;
    }
    return gy + "/" + gm + "/" + gd;
}

function IsValidShamsiDate(DateStr, IsPast) {
    if (IsPast === undefined)
        IsPast = true;
    if (DateStr.length >= 8 && DateStr.length <= 10) {
        var s = DateStr.split('/');
        if (s.length != 3)
            return false;
        if (parseInt(s[0]) == NaN || parseInt(s[1]) == NaN || parseInt(s[2]) == NaN)
            return false;
        if (((parseInt(s[1]) <= 6 && parseInt(s[1]) >= 1) && (parseInt(s[2]) <= 31 && parseInt(s[2]) >= 1))
            || (((parseInt(s[1]) <= 12 && parseInt(s[1])) >= 7) && (parseInt(s[2]) <= 30 && parseInt(s[2]) >= 1))) {
            var tmpDate = ConverShamsiToMiladi(parseInt(s[0]), parseInt(s[1]), parseInt(s[2]));
            tmpDate = tmpDate.split('/');
            var dt1 = new Date(parseInt(tmpDate[0]), parseInt(tmpDate[1]), parseInt(tmpDate[2]));
            var dt2 = new Date();
            var dd = dt2.getDate();
            var mm = dt2.getMonth() + 1;
            var yyyy = dt2.getFullYear();
            dt2 = new Date(yyyy, mm, dd);
            var timeDiff = dt2.getTime() - dt1.getTime();
            if (Math.ceil(timeDiff / (1000 * 3600 * 24)) < 0 && IsPast == true)
                return false;
        }
        else
            return false;
    }
    else
        return false;
    return true;
}

function ToJavaScriptDate(value, fieldName) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);

    if (results == null) {
        return "";
    }

    var dt = new Date(parseFloat(results[1]));

    if (dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate() == "1/1/1")
        return "";

    return dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate();
}

function ToJavaScriptTime(value, fieldName) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);

    if (results == null) {
        return "";
    }

    var dt = new Date(parseFloat(results[1]));

    if (dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate() == "1/1/1")
        return "";

    return dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
}

function CloseNotification(element) {
    var $noti = $($(element).closest(".NotificationItem"));
    $noti.removeClass("NotificationItemShow");
    setTimeout(function () {
        $noti.remove();
    }, 500);
}

var Sort = function () {
    var fields = [].slice.call(arguments),
        n_fields = fields.length;

    return function (A, B) {
        var a, b, field, key, primer, reverse, result;
        for (var i = 0, l = n_fields; i < l; i++) {
            result = 0;
            field = fields[i];

            key = typeof field === 'string' ? field : field.name;

            a = A[key];
            b = B[key];

            if (typeof field.primer !== 'undefined') {
                a = field.primer(a);
                b = field.primer(b);
            }

            reverse = (field.reverse) ? -1 : 1;

            if (a < b) result = reverse * -1;
            if (a > b) result = reverse * 1;
            if (result !== 0) break;
        }
        return result;
    }
}

// ~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/ =| GetDatasFunctions |= \~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/

//Signalr
/*
function makeNofitication(title, description, image) {
    var nofitication = document.createElement('div');
    $(nofitication).addClass("NotificationItem");
    var nowDate = new Date();
    $(nofitication).html(
        "<div class='NotificationItemImg'><img src='" + (image != undefined ? image : "/Images/AlarmIcon.png") + "' /></div>"
        + "<div class='NotificationItemText'>" + title + "</div>"
        + "<div class='NotificationItemDetaile'>" + description + "</div>"
        + "<div class='NotificationItemTime'>" + nowDate.getHours() + ":" + nowDate.getMinutes() + "</div>"
        + "<div class='NotificationItemTimeClose' onclick='CloseNotification(this);'></div>"
    );
    return nofitication;
}

function showNotification(noti) {
    $("#AllNotification").append(noti);
    $(noti).addClass("NotificationItemShow");
    setTimeout(function () {
        $(noti).removeClass("NotificationItemShow");
        setTimeout(function () {
            $(noti).remove();
        }, 500);
    }, 10000);
}

$(function () {

    var requestProxy;
    $.connection.hub.url = SignalRUrl;
    $.connection.hub.logging = true;
    requestProxy = $.connection.ServicesRequest;

    requestProxy.client.ResponseNewRequest = function (response) {
        var noti = makeNofitication("درخواست جدید در سیستم ثبت شده", "برای مشاهده به قسمت درخواست های جدید مراجعه کنید");
        showNotification(noti);
    };

    requestProxy.client.ResponseFactoreConfigure = function (FactorId, ServicesRequestsId, FactorNumber, TotalPrice, RequestTypeName, FactorCreateDate) {
        var noti = makeNofitication("فاکتور با شماره " + FactorNumber + " بسته شد", "مبلغ کل فاکتور " + addCommaIntoNumber(TotalPrice) + " تومان است برای " + RequestTypeName);
        showNotification(noti);
    };

    $.connection.hub.start().done(function () {
        requestProxy.server.login(cLayoutPersonelId, cLayoutPersonelUnicKey, 2);
    });
});
*/

//Signalr