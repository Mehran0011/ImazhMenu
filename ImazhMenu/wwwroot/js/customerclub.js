var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    try {
        if (dataTable != null) {
            dataTable.destroy();
            $('#tblCustomerClubData').empty();
        }
    } catch (ex) { }

    dataTable = $("#tblCustomerClubData").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 1000000,

        "ajax": {
            "method": "Post",
            "url": "/Account/AllCustomerClubInfo",
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
                "data": "cutomreName", "name": "cutomreName", "title": "نام مشتری", "className": "text-center"
            },
            {
                "data": "subject", "name": "subject", "title": "موضوع", "className": "text-center"
            },
            {
                "data": "phoneNumber", "name": "phoneNumber", "title": "شماره تلفن", "className": "text-center"
            },
            {
                "data": "message", "name": "message", "title": "متن پیام", "className": "text-center"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="w-75 btn-group" role="group">
                            <a onclick=Delete('/Account/DeleteCustomerClub/${data}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill">حذف</i>
                            </a>
                        </div>`
                }
                , "width": "50%"
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
        title: 'آیا از حذف پیام اطمینان دارید؟',
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

