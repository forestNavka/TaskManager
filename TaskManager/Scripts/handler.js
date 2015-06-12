(function ($) {
    //loads all categories
    var loadAllCategories = function () {
        $.ajax({
            type: "GET",
            url: "api/categories",
            contentType: "application/json"
        }).done(function (data) {
            var html = '<div class="panel-group">';
            $.each(data, function (key, value) {
                html += "<div class='panel panel-default category' data-id='" + value.Id + "'><div class='panel-heading panel-title'><a data-toggle='collapse' data-target='#category" + key + "' >" + value.Name + "</a><span class='glyphicon glyphicon-chevron-down'></span></div><div class='panel-body collapse' id='category" + key + "'></div></div>";
            });
            html += "<div class='panel panel-primary'><div class='panel-heading panel-title'><a data-toggle='collapse' data-target='#addCategoryForm' id='categoryAddButton'>Додати категорію</a><span class='glyphicon glyphicon-chevron-down'></span></div><div class='panel-body collapse' id='addCategoryForm'></div></div></div>";
            $("#main").html(html);
        });
    }

    //adds new category
    var addCategory = function (event) {
        event.preventDefault();
        var form = $("#addCategory");
        var category = { Name: form.find("input[name='name']").val(), Text: form.find("textarea[name='text']").val() };
        $.ajax({
            type: "POST",
            data: JSON.stringify(category),
            url: "api/categories",
            contentType: "application/json",
            error: function (xhr, status, error) {
                alert($.parseJSON(xhr.responseText).ExceptionMessage);
            }
        }).done(loadAllCategories);
    }

    //loads all tasks of category
    var loadTasks = function (categoryId, domObject) {
        $.ajax({
            type: "GET",
            url: "api/tasks",
            data: {
                query: "Category_Id = '" + categoryId + "'"
            },
            contentType: "application/json",
            error: function (xhr, status, error) {
                alert($.parseJSON(xhr.responseText).ExceptionMessage);
            }
        }).done(function (data) {
            var html = '<div class="panel-group">';
            $.each(data, function (key, value) {
                html += "<div class='panel panel-default task' data-id='" + value.Id + "'><div class='panel-heading panel-title'>";
                html += (value.IsDone) ? "<button class='done btn btn-default btn-xs' style='display:inline-block;'><span class='glyphicon glyphicon-ok'></span>Зроблено</button><button class='not-ready  btn btn-default btn-xs'><span class='glyphicon glyphicon-remove'></span>Не зроблено</button>" : "<button class='done  btn btn-default btn-xs'><span class='glyphicon glyphicon-ok'></span>Зроблено</button><button class='not-ready btn-xs btn btn-default' style='display:inline-block;'><span class='glyphicon glyphicon-remove'></span>Не зроблено</button>";
                html += "<a data-toggle='collapse' data-target='#task" + key + domObject.attr('id') + "' >" + value.Name + "</a><span class='glyphicon glyphicon-chevron-down'></span></div><div class='panel-body  collapse' id='task" + key + domObject.attr('id') + "'></div></div>";
            });
            html += "<div class='panel panel-info'><div class='panel-heading panel-title'><a data-toggle='collapse' data-target='#addTaskForm" + domObject.attr('id') + "' id='addTask" + domObject.attr('id') + "'>Додати задачу</a><span class='glyphicon glyphicon-chevron-down'></span></div><div class='panel-body collapse' id='addTaskForm" + domObject.attr('id') + "'></div></div></div>";
            domObject.html(html);
        });
    }

    //adds new task to category
    var addTask = function (event) {
        event.preventDefault();
        var element = $(this);
        var form = $(this).parents("form");
        var categoryId = $(this).parents(".category").data("id");
        var task = { Name: form.find("input[name='name']").val(), Text: form.find("textarea[name='text']").val(), Category_Id: categoryId };
        $.ajax({
            type: "POST",
            data: JSON.stringify(task),
            url: "api/tasks",
            contentType: "application/json",
            error: function (xhr, status, error) {
                alert($.parseJSON(xhr.responseText).ExceptionMessage);
            }
        }).done(function () { loadTasks(categoryId, element.parents("[id^='category']")); });
    }

    //loads all subtasks of task
    var loadSubtasks = function (taskId, domObject) {
        $.ajax({
            type: "GET",
            url: "api/subtasks",
            data: {
                query: "Task_Id = '" + taskId + "'"
            },
            contentType: "application/json",
            error: function (xhr, status, error) {
                alert($.parseJSON(xhr.responseText).ExceptionMessage);
            }
        }).done(function (data) {
            var html = '<div class="panel-group">';
            $.each(data, function (key, value) {
                html += "<div class='panel panel-default subtask' data-id='" + value.Id + "'><div class='panel-heading panel-title'>"
                html += (value.IsDone) ? "<button class='done btn btn-default btn-xs' style='display:inline-block;'><span class='glyphicon glyphicon-ok'></span>Зроблено</button><button class='not-ready btn btn-default btn-xs'><span class='glyphicon glyphicon-remove'></span>Не зроблено</button>" : "<button class='done btn btn-default btn-xs'><span class='glyphicon glyphicon-ok'></span>Зроблено</button><button class='not-ready btn btn-default btn-xs' style='display:inline-block;'><span class='glyphicon glyphicon-remove'></span>Не зроблено</button>";
                html += "<a" + key + domObject.attr('id') + "' >" + value.Name + "</a></div><div class='panel-body  collapse' id='subtask" + key + domObject.attr('id') + "'></div></div>";
            });
            html += "<div class='panel panel-success'><div class='panel-heading panel-title'><a data-toggle='collapse' data-target='#addSubtaskForm" + domObject.attr('id') + "' id='addSubtask" + domObject.attr('id') + "'>Додати підзадачу</a><span class='glyphicon glyphicon-chevron-down'></span></div><div class='panel-body collapse' id='addSubtaskForm" + domObject.attr('id') + "'></div></div></div>";
            domObject.html(html);
        });
    }

    //adds new subtask to task
    var addSubtask = function (event) {
        event.preventDefault();
        var form = $(this).parents("form");
        var element = $(this);
        var taskId = $(this).parents(".task").data("id");
        var subtask = { Name: form.find("input[name='name']").val(), Text: form.find("textarea[name='text']").val(), Task_Id: taskId };
        $.ajax({
            type: "POST",
            data: JSON.stringify(subtask),
            url: "api/subtasks",
            contentType: "application/json",
            error: function (xhr, status, error) {
                alert($.parseJSON(xhr.responseText).ExceptionMessage);
            }
        }).done(function () { loadSubtasks(taskId, element.parents("[id^='task']")); });
    }

    $(document).ready(function () {
        //loads all categories after button was clicked
        $("#categories").on("click", loadAllCategories);

        //loads all task of category after category's panel was expanded
        $(document).on("show.bs.collapse", ".category > .collapse", function (e) {
            $(this).parent().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            loadTasks($(this).parent(".panel").data("id"), $(this));
        });

        //loads all subtask of task after task's panel was expanded
        $(document).on("show.bs.collapse", ".task > .collapse", function (e) {
            if ($(e.target).is("[id^='task']")) {
                e.stopPropagation();
            }
            $(this).parent().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            loadSubtasks($(this).parent(".task").data("id"), $(this));
        });

        //changes glyphicon after panel was collapsed
        $(document).on("hide.bs.collapse", ".collapse", function (e) {
            if ($(e.target).is("[id^='addTaskForm']")) {
                e.stopPropagation();
            }
            if ($(e.target).is("[id^='task']")) {
                e.stopPropagation();
            }
            if ($(e.target).is("[id^='addSubtaskForm']")) {
                e.stopPropagation();
            }
            if ($(e.target).is("[id^='subtask']")) {
                e.stopPropagation();
            }
            $(this).parent().find("span.glyphicon-chevron-up").removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
        });

        //loads form for adding new category after addcategory panel was expanded
        $(document).on("show.bs.collapse", "#addCategoryForm", function (e) {
            $(this).parent().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            $("#addCategoryForm").html('<form role="form" id="addCategory"><div class="form-group"><label class="control-label" for="name" >Назва: </label><input type="text" name="name" class="form-control" /> </div> <div class="form-group"><label class="control-label" for="text" >Опис: </label> <textarea name="text" class="form-control" ></textarea></div><input type="submit" id="categorySubmit"/> </form>');
        });

        ////loads form for adding new task after addtask panel was expanded
        $(document).on("show.bs.collapse", "[id^='addTaskForm']", function (e) {
            if ($(e.target).is("[id^='addTaskForm']")) {
                e.stopPropagation();
            }
            $(this).parent().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            $(this).html('<form role="form"><div class="form-group"><label class="control-label" for="name" >Назва: </label><input type="text" name="name" class="form-control" /> </div> <div class="form-group"><label class="control-label" for="text" >Опис: </label> <textarea name="text" class="form-control" ></textarea></div><input type="submit" id="taskSubmit' + $(this).attr("id") + '"/> </form>');
        });

        ////loads form for adding new subtask after addsubtask panel was expanded
        $(document).on("show.bs.collapse", "[id^='addSubtaskForm']", function (e) {
            if ($(e.target).is("[id^='addSubtaskForm']")) {
                e.stopPropagation();
            }
            $(this).parent().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            $(this).html('<form role="form"><div class="form-group"><label class="control-label" for="name" >Назва: </label><input type="text" name="name" class="form-control" /> </div> <div class="form-group"><label class="control-label" for="text" >Опис: </label> <textarea name="text" class="form-control" ></textarea></div><input type="submit" id="subtaskSubmit' + $(this).attr("id") + '"/> </form>');
        });

        //submits category's form
        $(document).on("click", "#categorySubmit", addCategory);

        //submits task's form
        $(document).on("click", "[id^='taskSubmit']", addTask);

        //submits subtask's form
        $(document).on("click", "[id^='subtaskSubmit']", addSubtask);

        //changes suntask status after status button was clicked
        $(document).on("click", ".subtask button.done", function () {
            var element = $(this);
            $.ajax({
                type: "PUT",
                data: "=false",
                dataType: "text",
                url: "api/subtasks/" + $(this).parents(".subtask").data("id"),
                error: function (xhr, status, error) {
                    alert($.parseJSON(xhr.responseText).ExceptionMessage);
                }
            }).done(function () {
                element.hide();
                element.next().show();
                var task = element.parents(".task");
                var taskId = task.data("id");
                $.ajax({
                    type: "GET",
                    url: "api/tasks/" + taskId,
                    contentType: "application/json"

                }).done(function (data) {
                    if (data.IsDone == true) {
                        task.find("button.done").show();
                        task.find("button.not-ready").hide();
                    }
                    else {
                        task.find("button.done:eq(0)").hide();
                        task.find("button.not-ready:eq(0)").show();
                    }
                }).failed(function (xhr, status, error) {
                    alert(error);
                });

            });
        });

        //changes suntask status after status button was clicked
        $(document).on("click", ".subtask button.not-ready", function () {
            var element = $(this);
            $.ajax({
                type: "PUT",
                data: "=true",
                dataType: "text",
                url: "api/subtasks/" + $(this).parents(".subtask").data("id"),
                //contentType: "application/json",
                error: function (xhr, status, error) {
                    console.log(xhr);
                    console.log(status);
                    console.log(error);
                }
            }).done(function () {
                element.hide();
                element.prev().show();
                var task = element.parents(".task");
                var taskId = task.data("id");
                $.ajax({
                    type: "GET",
                    url: "api/tasks/" + taskId,
                    contentType: "application/json"

                }).done(function (data) {
                    if (data.IsDone == true) {
                        task.find("button.done").show();
                        task.find("button.not-ready").hide();
                    }
                    else {
                        task.find("button.done:eq(0)").hide();
                        task.find("button.not-ready:eq(0)").show();
                    }
                }).failed(function (xhr, status, error) {
                    alert(error);
                });
            });
        });

        $(document).on("click", ".task button.done:eq(0)", function () {
            var element = $(this);
            $.ajax({
                type: "PUT",
                data: "=false",
                dataType: "text",
                url: "api/tasks/" + $(this).parents(".task").data("id"),
                error: function (xhr, status, error) {
                    alert($.parseJSON(xhr.responseText).ExceptionMessage);
                }
            }).done(function () {
                element.hide();
                element.next().show();
            });
        });


        $(document).on("click", ".task button.not-ready:eq(0)", function () {
            var element = $(this);
            $.ajax({
                type: "PUT",
                data: "=true",
                dataType: "text",
                url: "api/tasks/" + $(this).parents(".task").data("id"),
                error: function (xhr, status, error) {
                    alert($.parseJSON(xhr.responseText).ExceptionMessage);
                }
            }).done(function () {
                element.hide();
                element.prev().show();
            });
        });
        //deletes all finished tasks
        $("#delete").on("click", function () {
            $.ajax({
                type: "DELETE",
                url: "api/tasks/delete"
            }).done(loadAllCategories);
        });
    });
})(jQuery)