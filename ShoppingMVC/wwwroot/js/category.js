﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#catTable').DataTable({
        "ajax": { url: '/admin/category/getall'},
        "columns": [
            { data: 'name', "width": "30%" },
            { data: 'displayOrder', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                            <a href="/admin/category/edit?id=${data}" class="btn btn-primary mx-2" ><i class="bi bi-pencil-square"></i>Edit</a >
                            <a onClick=Delete('/admin/category/delete?id=${data}') class="btn btn-danger mx-2" ><i class="bi bi-trash-fill"></i>Delete</a ></div>`
                },
                "width": "50%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data){
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}
