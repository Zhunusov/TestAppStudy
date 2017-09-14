function open_sidebar() {
    document.getElementById("mySidebar").style.display = "block";
    document.getElementById("myOverlay").style.display = "block";
}

function close_sidebar() {
    document.getElementById("mySidebar").style.display = "none";
    document.getElementById("myOverlay").style.display = "none";
}

function show_nav(obj,name) {    
    $("#menuTeach").css("display", "none");
    $("#menuStu").css("display", "none");
    $("#menuSubj").css("display", "none");
    $("#"+name).css("display", "block");
    $('#data_result').empty();
    $('#title_main_cont_level_2').text("");
    $('#title_main_cont_level_1').text(obj.text);
    $("#assigning_teacher_container").css("display", "none");
    $("#assigning_teacher_info_container").css("display", "none");
    $("#assign_teach_to_stu_info_container").css("display", "none");
    $("#assign_teach_to_stu_container").css("display", "none");
}

function change_register_form() {
    var role = document.getElementById('selected_role').value;
    switch (role) {
        case "Студент":
            document.getElementById('id03').style.display = 'block';
            document.getElementById('studystart').setAttribute('required', 'required');

            break;

        case "Преподаватель":
            document.getElementById('id03').style.display = 'none';
            document.getElementById('studystart').removeAttribute("required");
            break;

    }
}

function compare_password() {
    if (document.getElementById('psw').value !== document.getElementById('psw_confirm').value) {
        alert("Ошибка: Поля 'Пароль' и 'Подтверждение пароля' не совпадают!");
        document.getElementById('psw_confirm').focus();
        return false;
    }
}

function change_to_register_modal() {
    document.getElementById('id02').style.display = 'none';
    document.getElementById('id01').style.display = 'block';
}

function change_to_logon_modal() {
    document.getElementById('id01').style.display = 'none';
    document.getElementById('id02').style.display = 'block';
}

function edit_personal_info() {
    $.ajax({
        url: '/api/account/getuserinfo',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            document.getElementById('modal_editing_per_info').style.display = 'block';
            document.getElementById('mySidebar').style.zIndex = '0';
            switch (data.Role) {
                case "Администратор":
                    WriteResponseAdmin(data);
                    document.getElementById('editing_per_info_admin_form').style.display = 'block';
                    break;

                case "Студент":
                    WriteResponseStudent(data);
                    document.getElementById('editing_per_info_stu_form').style.display = 'block';
                    break;

                case "Преподаватель":
                    WriteResponseAdmin(data);
                    document.getElementById('editing_per_info_admin_form').style.display = 'block';
                    break;
            }

        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function Close_modal_editing() {
    document.getElementById('modal_editing_per_info').style.display = 'none';
    document.getElementById('editing_per_info_admin_form').style.display = 'none';
    document.getElementById('editing_per_info_stu_form').style.display = 'none';
    document.getElementById('role_status').innerHTML = '';
    document.getElementById('mySidebar').style.zIndex = '4';
}

function WriteResponseAdmin(user) {
    document.getElementById('username_admin_form').value = user.UserName;
    document.getElementById('firstname_admin_form').value = user.FirstName;
    document.getElementById('lastname_admin_form').value = user.LastName;
    document.getElementById('patronymic_admin_form').value = user.Patronymic;
    document.getElementById('role_status').innerHTML = user.Role;
}
function WriteResponseStudent(user) {
    document.getElementById('username_stu_form').value = user.UserName;
    document.getElementById('firstname_stu_form').value = user.FirstName;
    document.getElementById('lastname_stu_form').value = user.LastName;
    document.getElementById('patronymic_stu_form').value = user.Patronymic;
    document.getElementById('studystart_stu_form').value = user.StudyStart_Short;
    document.getElementById('role_status').innerHTML = user.Role;
}


$(function () {
    $('#login_form').submit(function (e) {
        e.preventDefault();
        var data = {
            Email: $('#username_login').val(),
            Password: $('#psw_login').val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/account/login',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']['Account.login']) {
                    $.each(response['ModelState']['Account.login'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $('#editing_per_info_stu_form').submit(function (e) {
        e.preventDefault();
        var data = {
            Email: $('#username_stu_form').val(),
            FirstName: $('#firstname_stu_form').val(),
            LastName: $('#lastname_stu_form').val(),
            Patronymic: $('#patronymic_stu_form').val(),
            StudyStart: new Date(($('#studystart_stu_form').val()).replace(/(\d+).(\d+).(\d+)/, '$3/$2/$1')),
            Role: document.getElementById('role_status').innerHTML
        };
        $.ajax({
            type: 'POST',
            url: '/api/account/save',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $('#editing_per_info_admin_form').submit(function (e) {
        e.preventDefault();
        var data = {
            Email: $('#username_admin_form').val(),
            FirstName: $('#firstname_admin_form').val(),
            LastName: $('#lastname_admin_form').val(),
            Patronymic: $('#patronymic_admin_form').val(),
            Role: document.getElementById('role_status').innerHTML
        };
        $.ajax({
            type: 'POST',
            url: '/api/account/save',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $('#register_form').submit(function (e) {
        e.preventDefault();
        var data = {
            Email: $('#email').val(),
            Password: $('#psw').val(),
            ConfirmPassword: $('#psw_confirm').val(),
            FirstName: $('#firstname').val(),
            LastName: $('#lastname').val(),
            Patronymic: $('#patronymic').val(),
            StudyStart: new Date(($('#studystart').val()).replace(/(\d+).(\d+).(\d+)/, '$3/$2/$1')),
            Role: $('#selected_role').val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/account/register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $('#logoff').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: '/api/account/logout',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function () {
                location.reload(true);
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    });
});

$(function () {
    $('#lang_ru').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: '/api/culture/changeculture',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify("ru"),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $('#lang_en').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: '/api/culture/changeculture',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify("en"),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $("#studystart_stu_form").datepicker({ maxDate: 0, dateFormat: "dd.mm.yy" });
    $("#studystart").datepicker({ maxDate: 0, dateFormat: "dd.mm.yy" });
    $("#studystart_add_user_form").datepicker({ maxDate: 0, dateFormat: "dd.mm.yy" });
});

function edit_user(obj) {    
    var username = obj.id;
    $.ajax({
        url: '/api/admin/getuserinfo',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(username),
        success: function (data) {
            document.getElementById('modal_editing_per_info').style.display = 'block';
            document.getElementById('mySidebar').style.zIndex = '0';
            switch (data.Role) {
                case "Администратор":
                    WriteResponseAdmin(data);
                    document.getElementById('editing_per_info_admin_form').style.display = 'block';
                    break;

                case "Студент":
                    WriteResponseStudent(data);
                    document.getElementById('editing_per_info_stu_form').style.display = 'block';
                    break;

                case "Преподаватель":
                    WriteResponseAdmin(data);
                    document.getElementById('editing_per_info_admin_form').style.display = 'block';
                    break;
            }
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

$(function () {
    $('.my-nav').click(function (e) {
        e.preventDefault();        
        $('#title_main_cont_level_2').text($(this).text());        
    });
});

$(function () {
    $('#nav_add_teacher').click(function (e) {
        e.preventDefault();        
        $("#add_user_form").css("display", "block");
        $("#role_add_user_form").val("Преподаватель");
        $("#block_study_start_add_user_form").css("display", "none");
        $("#studystart_add_user_form").removeAttr("required");
        $("#mySidebar").css("zIndex", "0");              
    });
});

$(function () {
    $(".close-modal").click(function (e) {
        e.preventDefault();
        $("#add_user_form").css("display", "none");
        $("#add_subject_form").css("display", "none");
        $("#edit_subject_form").css("display", "none");
        $("#add_comment_form").css("display", "none");
        $("#mySidebar").css("zIndex", "4");
    });
});

$(function () {
    $("#role_add_user_form").change(function (e) {
        e.preventDefault();
        switch ($(this).val()) {
            case "Студент":
                $("#block_study_start_add_user_form").css("display", "block");
                $("#studystart_add_user_form").attr("required", "required");
                break;

            case "Преподаватель":
                $("#block_study_start_add_user_form").css("display", "none");
                $("#studystart_add_user_form").removeAttr("required");
                break;
        }
    });
});

$(function () {
    $("#psw_confirm_add_user_form").change(function (e) {
        e.preventDefault();
        if ($(this).val() !== $("#psw_add_user_form").val()) {
            alert("Ошибка: поля 'Пароль' и 'Подтверждение пароля' не совпадают");
            $("#psw_confirm_add_user_form").focus();            
        }        
    });
});

$(function () {
    $("#add_user_form_data").submit(function (e) {
        e.preventDefault();
        var data = {
            Email: $("#email_add_user_form").val(),
            Password: $("#psw_add_user_form").val(),
            ConfirmPassword: $("#psw_confirm_add_user_form").val(),
            FirstName: $("#firstname_add_user_form").val(),
            LastName: $("#lastname_add_user_form").val(),
            Patronymic: $("#patronymic_add_user_form").val(),
            Role: $("#role_add_user_form").val(),
            UserName: $("#email_add_user_form").val(),
            StudyStart: new Date(($("#studystart_add_user_form").val()).replace(/(\d+).(\d+).(\d+)/, '$3/$2/$1'))
        };
        $.ajax({
            type: 'POST',
            url: '/api/admin/register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

function remove_user(obj) {
    var username = obj.id;
    $("#modal_ask_confirm").css("display", "block");
    $("#mySidebar").css("zIndex", "0");
    $("#ask_content").text("Удалить " + username + " ?");
    $(function () {
        $("#remove_data").click(function (e) {
            e.preventDefault();            
            $.ajax({
                type: 'POST',
                url: '/api/admin/removeuser',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(username),
                success: function () {
                    location.reload(true);
                },
                error: function (obj, error, status) {
                    var response = jQuery.parseJSON(obj.responseText);
                    if (response['ModelState']) {
                        $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                    }
                }
            });
        });
    });
}

$(function () {
    $(".close-modal-conf-ask").click(function (e) {
        e.preventDefault();
        $("#modal_ask_confirm").css("display", "none");       
        $("#ask_content").text();
    });
});

$(function () {
    $('#nav_add_student').click(function (e) {
        e.preventDefault();
        $("#add_user_form").css("display", "block");
        $("#role_add_user_form").val("Студент");
        $("#block_study_start_add_user_form").css("display", "block");
        $("#studystart_add_user_form").attr("required", "required");
        $("#mySidebar").css("zIndex", "0");
    });
});

$(function () {
    $("#nav_add_subject").click(function (e) {
        e.preventDefault();
        $("#add_subject_form").css("display", "block");
        $("#mySidebar").css("zIndex", "0");       
    });
});

$(function () {
    $("#add_subject_form_data").submit(function (e) {
        e.preventDefault();
        var data = {
            Title: $("#add_subject_title").val(),
            Description: $("#add_subject_description").val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/admin/addsubject',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

function edit_subject(obj) {
    var subject_title = obj.id;
    $.ajax({
        url: '/api/admin/getsubject',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(subject_title),
        success: function (data) {
            $("#edit_subject_form").css("display", "block");
            $("#mySidebar").css("zIndex", "0");
            $("#edit_subject_title").val(data.Title);
            $("#edit_subject_description").val(data.Description);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

$(function () {
    $('#edit_subject_form_data').submit(function (e) {
        e.preventDefault();
        var data = {
            Title: $("#edit_subject_title").val(),
            Description: $("#edit_subject_description").val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/admin/updatesubject',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

function remove_subject(obj) {
    var subject_title = obj.id;
    $("#modal_ask_confirm").css("display", "block");
    $("#mySidebar").css("zIndex", "0");
    $("#ask_content").text("Удалить " + subject_title + " ?");
    $(function () {
        $("#remove_data").click(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: '/api/admin/removesubject',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(subject_title),
                success: function () {
                    location.reload(true);
                },
                error: function (obj, error, status) {
                    var response = jQuery.parseJSON(obj.responseText);
                    if (response['ModelState']) {
                        $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                    }
                }
            });
        });
    });
}

$(function () {
    $("#assigning_teacher").change(function (e) {
        e.preventDefault();
        var username = $(this).val();
        get_teacher_assign_info(username);
    });
});

$(function () {
    $("#btn_assign_subj").click(function (e) {
        e.preventDefault();
        var data = {
            Title: $("#all_subj_list").val(),
            TeacherName: $("#assigning_teacher_username").text()
        };
        $.ajax({
            type: 'POST',
            url: '/api/admin/addsubjecttoteacher',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                get_teacher_assign_info(data.TeacherName);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }           
        });
    });
});

$(function () {
    $("#btn_cancel_subj").click(function (e) {
        e.preventDefault();
        var data = {
            Title: $("#taught_subj_list").val(),
            TeacherName: $("#assigning_teacher_username").text()
        };
        $.ajax({
            type: 'POST',
            url: '/api/admin/cancelassignsubject',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                get_teacher_assign_info(data.TeacherName);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }            
        });
    });
});

$(function () {
    $("#assign_teach_students").change(function (e) {
        e.preventDefault();
        var username = $(this).val();
        get_student_assign_info(username);
    });
});

$(function () {
    $("#btn_assign_teacher").click(function (e) {
        e.preventDefault();
        var data = {
            SubjectTitle: $("#stu_all_subj_list").val(),
            TeacherUserName: $("#teacher_subj_list").val(),
            StudentUserName: $("#assign_teach_students").val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/admin/assignteachertostudent',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                get_student_assign_info(data.StudentUserName);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

function cancel_teacher(btn) {
    var data = {
        SubjectTitle: $(btn).closest("div.w3-row").find("a.subject").text(),
        TeacherUserName: $(btn).closest("div.w3-row").find("a.teacher").attr("id"),
        StudentUserName: $("#assign_teach_students").val()
    };
    $.ajax({
        type: 'POST',
        url: '/api/admin/cancelteachertostudent',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function () {
            get_student_assign_info(data.StudentUserName);
        },
        error: function (obj, error, status) {
            var response = jQuery.parseJSON(obj.responseText);
            if (response['ModelState']) {
                $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
            }
        }
    });
}

function add_comment(item) {
    $("#add_comment_form").css("display", "block");
    $("#mySidebar").css("zIndex", "0");
    $("#add_comment_subject_title").val($(item).closest("div.w3-panel").find("div.title").text());

}

$(function () {
    $("#add_comment_form_data").submit(function (e) {
        e.preventDefault();
        var data = {
            SubjectTitle: $("#add_comment_subject_title").val(),
            Description: $("#add_comment_description").val(),
            IsPublic: $("input[name=visibility]:checked").val(),
            UserName: $("#current_user").text()
        };
        $.ajax({
            type: 'POST',
            url: '/api/account/addcomment',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                location.reload(true);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $("#assign_subject").change(function (e) {
        e.preventDefault();
        var subjectTitle = $(this).val();
        get_students_assign_info(subjectTitle);
    });
});

$(function () {
    $("#btn_assign_subj").click(function (e) {
        e.preventDefault();
        var data = {
            SubjectTitle: $("#assign_subject").val(),
            TeacherUserName: $("#current_user").text(),
            StudentUserName: $("#all_student_list").val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/teacher/assignsubjecttostudent',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                get_students_assign_info(data.SubjectTitle);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

$(function () {
    $("#btn_cancel_student").click(function (e) {
        e.preventDefault();
        var data = {
            SubjectTitle: $("#assign_subject").val(),
            TeacherUserName: $("#current_user").text(),
            StudentUserName: $("#assigned_student_list").val()
        };
        $.ajax({
            type: 'POST',
            url: '/api/teacher/cancelsubjectfromstudent',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                get_students_assign_info(data.SubjectTitle);
            },
            error: function (obj, error, status) {
                var response = jQuery.parseJSON(obj.responseText);
                if (response['ModelState']) {
                    $.each(response['ModelState'], function (index, item) { alert(item + '\n'); });
                }
            }
        });
    });
});

