var dataTable;
$(document).ready(function (){
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/User/GetAll"
        },
        "columns": [       
                                { "data": "name", "width": "15%" },
                                { "data": "email", "width": "15%" },
                                { "data": "phoneNumber", "width": "15%" },
                                { "data": "company.name", "width": "15%" },
                                { "data": "role", "width": "15%" },
            {
                "data": {
                    id:"id",lockoutEnd:"lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //user locked
                        return `
                        <div class="text-center">
                            <a class="btn btn-danger" onclick=LockUnlock('${data.id}')>
                                    <i class="fas fa-lock"></i>Lock
                            </a>
                        </div>
                        `;
                    }
                    else
                    {
                        //user unlocked
                        return `
                            <div class="text-center">

                            <a class="btn btn-success" onclick=LockUnlock('${data.id}')>
                                <i class="fas fa-lock-open"></i>Unlock
                            </a>
                            </div>
                            `;
                    }
                }
            }
        ]
    })
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
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