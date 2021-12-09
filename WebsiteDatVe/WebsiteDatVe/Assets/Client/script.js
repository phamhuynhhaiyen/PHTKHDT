$(document).ready(function () {
    ////var target = ""
    //$("#txtDiemDi").click(function () {
    //    $("#cbbDiemDi").toggleClass("active");
    //    //target = '#txtDiemDi'
    //})
    //$("#txtDiemDen").click(function () {
    //    $("#cbbDiemDen").toggleClass("active");
    //    //target = '#txtDiemDen'
    //})

    $("#frmKhach").click(function () {
        $("#cbbKhach").toggleClass("active");
    })
    $("#frmLoaiGhe").click(function () {
        $("#cbbLoaiGhe").toggleClass("active");
    })

    //Xu li chon ngay

    flatpickr("#timeCheckIn", {
        minDate: "today",
/*        dateFormat: "d-m-Y",*/
        defaultDate: "today",
        onChange: function (selectedDates, dateStr, instance) {
            flatpickr("#timeCheckOut", {
                minDate: dateStr,
/*                dateFormat: "d-m-Y",*/
            });
        },

    });
    

    //window.onclick = function (e) {
    //    if (!e.target.matches(".form-group")) {
    //        var dropdowns = $(".combobox");
    //        var i;
    //        for (i = 0; i < dropdowns.length; i++) {
    //            var openDropdown = dropdowns[i];
    //            if (openDropdown.classList.contains('active')) {
    //                openDropdown.classList.remove('active');
    //            }
    //        }
    //    }
    //}

    //$('.main').click(function () {
    //    var dropdowns = $('.combobox')
    //    console.log(dropdowns)
    //    for (var i = 0; i < dropdowns.length; i++) {
    //        if (dropdowns[i].classList.contains('active')) {
    //            dropdowns[i].classList.remove('active');
    //        }
    //    }
    //})



})

