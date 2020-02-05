var MessageType = { Succes: "#72bf44", Error: "#ed5666", Warning: "#fbb716", Information: "#1d84c6" };
$(document).ready(function (e) {
    $("#AllLightBoxAlert").fadeOut(0);
    $("#AllLightBoxConfirm").fadeOut(0);
    $("#AllProgress").fadeOut(0);
    $("#AllAddAddressesDialogBox").fadeOut(0);

    //delete sidarmap
    var sidarIntervalId = setInterval(function () {
        try {
            $(".leaflet-bottom").each(function () {
                if ($(this).hasClass("leaflet-right")) {
                    $(this).remove();
                }
            });
            clearInterval(sidarIntervalId);
        }
        catch
        {
        
        }
    }, 3);
    //delete sidarmap
});

function CallMethod(PageUrl, dataParam, SuccessFunction) {
    $("#AllProgress").fadeIn(500);
    $("#AllContent").addClass("AllContentBlur");
    $.ajax({
        type: "POST",
        url: PageUrl,
        data: dataParam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        timeout: 999999,
        success: function (response) {
            $("#AllContent").removeClass("AllContentBlur");
            $("#AllProgress").fadeOut(250, function () {
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
            $("#AllContent").removeClass("AllContentBlur");
            $("#AllProgress").fadeOut(250, function () {
                ShowMessage("خطا", "خطا در اتصال به سرویس دهنده", MessageType.Error, null, null);
            });
        }
    });
}

function UploadFile(FunctionUrl, FilesInput, SuccessFunction) {
    $("#AllProgress").fadeIn(500);
    $("#AllContent").addClass("AllContentBlur");

    if (SuccessFunction == undefined)
        SuccessFunction = function () { };

    var formdata = new FormData();
    for (i = 0; i < FilesInput.files.length; i++) {
        formdata.append(FilesInput.files[i].name, FilesInput.files[i]);
    }
    var xhr = new XMLHttpRequest();
    xhr.open('POST', FunctionUrl);
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        $("#AllContent").removeClass("AllContentBlur");
        $("#AllProgress").fadeOut(250, function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
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
        });
    }
}

function ShowMessage(Title, Content, Type, OkFunction) {
    if (OkFunction == null || OkFunction == undefined)
        OkFunction = function () { };
    $("#AllContent").addClass("AllContentBlur");
    $("#btnAlertLightBoxOk").off("click");
    $("#btnAlertLightBoxOk").click(function () {
        $("#AllLightBoxContetn").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxAlert").fadeOut(150);
        $("#AllContent").removeClass("AllContentBlur");
        OkFunction();
    });
    $("#imgAlertLightBoxCancel").off("click");
    $("#imgAlertLightBoxCancel").click(function () {
        $("#AllLightBoxContetn").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxAlert").fadeOut(150);
        $("#AllContent").removeClass("AllContentBlur");
        OkFunction();
    });

    $("#spnAlertLightBoxHeader").html(Title);
    $("#spnAlertLightBoxContent").html(Content);
    $("#LightBoxAlertContent").addClass("AllLightBoxContetnShow");
    $(".AllLightBoxHeader").css("background", Type);
    $("#AllLightBoxAlert").fadeIn(150);
}

function ShowConfirm(Title, Content, Type, OkFunction, NoFunction) {
    if (OkFunction == null || OkFunction == undefined)
        OkFunction = function () { };
    if (NoFunction == null || NoFunction == undefined)
        NoFunction = function () { };

    $("#AllContent").addClass("AllContentBlur");

    $("#btnConfirmLightBoxOk").off("click");
    $("#btnConfirmLightBoxOk").click(function () {
        $("#AllLightBoxContetn").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        $("#AllContent").removeClass("AllContentBlur");
        OkFunction();
    });
    $("#btnConfirmLightBoxNo").off("click");
    $("#btnConfirmLightBoxNo").click(function () {
        $("#AllLightBoxContetn").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        $("#AllContent").removeClass("AllContentBlur");
        NoFunction();
    });
    $("#imgConfirmLightBoxCancel").off("click");
    $("#imgConfirmLightBoxCancel").click(function () {
        $("#AllLightBoxContetn").removeClass("AllLightBoxContetnShow");
        $("#AllLightBoxConfirm").fadeOut(150);
        $("#AllContent").removeClass("AllContentBlur");
        NoFunction();
    });

    $("#spnConfirmLightBoxHeader").html(Title);
    $("#spnConfirmLightBoxContent").html(Content);
    $("#LightBoxConfirmContent").addClass("AllLightBoxContetnShow");
    $(".AllLightBoxHeader").css("background", Type);
    $("#AllLightBoxConfirm").fadeIn(150);

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

$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
}

$.fn.selectText = function () {
    this.find('input').each(function () {
        if ($(this).prev().length == 0 || !$(this).prev().hasClass('p_copy')) {
            $('<p class="p_copy" style="position: absolute; z-index: -1;"></p>').insertBefore($(this));
        }
        $(this).prev().html($(this).val());
    });
    var doc = document;
    var element = this[0];
    if (doc.body.createTextRange) {
        var range = document.body.createTextRange();
        range.moveToElementText(element);
        range.select();
    } else if (window.getSelection) {
        var selection = window.getSelection();
        var range = document.createRange();
        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);
    }
};

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

// ~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/ =| GetDatasFunctions |= \~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/.\~/