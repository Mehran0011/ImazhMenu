var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {

    try {
        if (dataTable != null) {
            dataTable.destroy();
            $('#tblGalleryData').empty();
        }
    } catch (ex) { }

    dataTable = $("#tblGalleryData").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 1000000,

        "ajax": {
            "method": "post",
            "url": "/Account/GetAllGalleryPictures",
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
                "data": "galleryPicUrl",
                "name": "galleryPicUrl",
                "className": "text-center",
                "title": " تصویر گالری",
                "render": function (data) {
                    return `<img src="/${data}" width="200px;" height="200px;"/>`
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="w-75 btn-group" role="group">
                            <a href="/Account/UpdateGalleryPicture?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square">ویرایش</i>
                            </a>
                            <a onclick=Delete('/Account/DeleteGalleryPicture/${data}') class="btn btn-danger mx-2">
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
        title: 'آیا از حذف تصویر گالری اطمینان دارید؟',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText:'خیر',
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

