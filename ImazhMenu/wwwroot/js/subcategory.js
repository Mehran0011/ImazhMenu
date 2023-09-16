var dataTable;

$(document).ready(function () {
    loadDataTable();
});

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

        "columnDefs":
            [
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
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="w-75 btn-group" role="group">
                            <a href="/Account/UpdateSubCategory?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square">ویرایش</i>
                            </a>
                            <a onclick=Delete('/Account/DeleteSubCategory/${data}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill">حذف</i>
                            </a>
                        </div>`
                }
            }
        ],

        responsive: true,
        paging: !0,
        drawCallback: function () {

        },
        "initComplete": function (settings, json) {
        },
        rowCallback: function (row, data) {

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

