var dataTable;

$(document).ready(function () {
    loadDataTable();
});
$("#createCategorydrp1").change(function () {
    var selectedCategoryId = $(this).val();
    dataTable.ajax.url(`/Account/GetAllSubCategories?categoryId=${selectedCategoryId}`).load();
});

$.ajax({
    method: "Post",
    url: "/Account/GetAllDrpCategories"

}).done(function (response) {
    var _html = "<option value='-1' selected>دسته بندی محصولات را انتخاب کنید...</option>";
    var allhtml = "";
    var totalHtml = "";
    for (var i = 0; i < response.length; i++) {
        allhtml += `<option value="${response[i].id}">${response[i].cactegoryName}</option>`;
    }

    totalHtml = _html + allhtml;

    $("#createCategorydrp").html(totalHtml);
    $("#createCategorydrp1").html(totalHtml);
});


//function loadDataTable() {
//    try {
//        if (dataTable != null) {
//            dataTable.destroy();
//            $('#tblSubCategoryData').empty();
//        }
//    } catch (ex) { }

//    dataTable = $("#tblSubCategoryData").DataTable({
//        "processing": true, // for show progress bar  
//        "serverSide": true, // for process server side  
//        "filter": true, // this is for disable filter (search box)  
//        "orderMulti": false, // for disable multiple column at once  
//        "pageLength": 1000000,
//        "scrollCollapse": true,
//        "scrollY": '400px',

//        "ajax": {
//            "method": "post",
//            "url": "/Account/GetAllSubCategories",
//            "type": "POST",
//            "datatype": "json"
//        },

//        "columnDefs": [
//            {
//                "targets": [0],
//                "searchable": false,
//                "visible": false,
//            },
//        ],
//        "aoColumns": [
//            {
//                'data': "id", "title": "#", "className": "text-center", 'render': function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
//            },
//            {
//                "data": "subCatImage",
//                "name": "subCatImage",
//                "title": " تصویر محصول",
//                "render": function (data) {
//                    return `<img src="/${data}" width="200px;" height="200px;"/>`
//                }
//            },
//            {
//                "data": "subCategoryName", "name": "subCategoryName", "title": " نام محصول", "className": "text-center"
//            },
//            {
//                "data": "price", "name": "price", "title": " قیمت محصول", "className": "text-center"
//            },
//            {
//                "data": "subCatDesc", "name": "subCatDesc", "title": "توضیحات", "className": "text-center"
//            },
//            {
//                "data": "isActive",
//                "title": "فعال/غیرفعال",
//                "className": "text-center",
//                "render": function (data, type, row) {
//                    console.log(data)
//                    return `
//            <label class="switch">
//                <input type="checkbox" ${data==true ? "checked" : ""} onclick="toggleProductStatus(${row.id}, this.checked)">
//                <span class="slider round"></span>
//            </label>`;
//                }
//            },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `
//                            <div class="w-75 btn-group" style="justify-content: center;color: white;" role="group">
//                            <a href="/Account/UpdateSubCategory?id=${data}" class="btn btn-primary mx-2">
//                                <i class="bi bi-pencil-square">ویرایش</i>
//                            </a>
//                            <a onclick=Delete('/Account/DeleteSubCategory/${data}') class="btn btn-danger mx-2">
//                                <i class="bi bi-trash-fill">حذف</i>
//                            </a>
//                        </div>`;
//                }
//            }
//        ],

//        responsive: true,
//        paging: !0,
//        drawCallback: function () { },
//        "initComplete": function (settings, json) { },
//        rowCallback: function (row, data) {
//            $(row).find('td').addClass('center-items');
//        }
//    });
//}


function loadDataTable() {
    try {
        if (dataTable != null) {
            dataTable.destroy();
            $('#tblSubCategoryData').empty();
        }
    } catch (ex) { }

    dataTable = $("#tblSubCategoryData").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 1000000,
        "scrollCollapse": true,
        "scrollY": '400px',

        "ajax": {
            "method": "post",
            "url": "/Account/GetAllSubCategories",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs": [
            {
                "targets": [0],
                "searchable": false,
                "visible": false,
            },
        ],
        "aoColumns": [
            {
                'data': "id", "title": "#", "className": "text-center", 'render': function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
            },
            {
                "data": "subCatImage",
                "name": "subCatImage",
                "title": " تصویر محصول",
                "render": function (data) {
                    return `<img src="/${data}" width="200px;" height="200px;"/>`
                }
            },
            {
                "data": "subCategoryName", "name": "subCategoryName", "title": " نام محصول", "className": "text-center"
            },
            {
                "data": "price", "name": "price", "title": " قیمت محصول", "className": "text-center"
            },
            {
                "data": "subCatDesc", "name": "subCatDesc", "title": "توضیحات", "className": "text-center"
            },
            {
                "data": "isActive",
                "title": "فعال/غیرفعال",
                "className": "text-center",
                "render": function (data, type, row) {
                    console.log(data)
                    return `
            <label class="switch">
                <input type="checkbox" ${data == true ? "checked" : ""} onclick="toggleProductStatus(${row.id}, this.checked)">
                <span class="slider round"></span>
            </label>`;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="w-75 btn-group" style="justify-content: center;color: white;" role="group">
                            <a href="/Account/UpdateSubCategory?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square">ویرایش</i>
                            </a>
                            <a onclick=Delete('/Account/DeleteSubCategory/${data}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill">حذف</i>
                            </a>
                        </div>`;
                }
            }
        ],

        responsive: true,
        paging: !0,
        drawCallback: function () { },
        "initComplete": function (settings, json) { },
        rowCallback: function (row, data) {
            $(row).find('td').addClass('center-items');
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: 'آیا از حذف محصول اطمینان دارید؟',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'خیر',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}


function toggleProductStatus(id, isActive) {
    $.ajax({
        url: "/Account/ToggleProductStatus",
        type: "POST",
        data: { id: id, isActive: isActive },
        success: function (response) {
            if (response.success) {
                alert("وضعیت محصول تغییر کرد");
            } else {
                alert("خطا در تغییر وضعیت");
            }
        },
        error: function () {
            alert("خطا در برقراری ارتباط با سرور");
        }
    });
}


