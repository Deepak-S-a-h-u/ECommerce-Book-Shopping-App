var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
        "ajax":
        {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "10%" },
            { "data": "discription", "width": "50%"},
            { "data": "author", "width": "10%" },
            { "data": "isbn", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text-center">
    <a class="btn btn-info" href="/Admin/Product/Upsert/${data}">
        <i class="fas fa-edit"></i>
    </a>
    <a class="btn btn-danger" onclick=Delete("/Admin/Product/Delete/${data}")>
        <i class="fas fa-trash-alt"></i>
    </a>
</div>
`;
                }
            }

        ]
    })
}


function Delete(url) {
    swal({
        title: "want to delete data?",
        text: "Delete information",
        buttons: true,
        icon: "warning",
        dangerModel: true,
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.messege);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.messege);
                    }
                }

            })
        }
    })
}