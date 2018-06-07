$(document).ready(function () {
    //DisplayErrors();

    $('#formContainer').validate({
        focusInvalid : false,
        rules: {
            EventName: { required: true},
            Location: { required: true },
            EventStartDate: { required: true },
            EventStartTime: { required: true },
            //EventEndDate: { required: true },
            EventEndTime: { required: true }
        },
        messages: {
            EventName: { required: "<a href='#' onclick=buildError('EventName')>Please enter the Event Name</a>"},
            Location: { required: "<a href='#' onclick=buildError('Location')>Please enter the Location</a>" },
            EventStartDate: { required: "<a href='#' onclick=buildError('EventStartDate')>Please enter the Start Date</a>" },
            EventStartTime: { required: "<a href='#' onclick=buildError('EventStartTime')>Please enter the Start Time</a>" },
            //EventEndDate: { required: "<a href='#' onclick=buildError('EventEndDate')>Enter the End Date</a>" },
            EventEndTime: { required: "<a href='#' onclick=buildError('EventEndTime')>Please enter the End Time</a>" }
        },
                      
        debug: true,
        errorLabelContainer: "#errormessages",
        wrapper: "li",
        invalidHandler: function(event, validator) {
            var errors = validator.numberOfInvalids();
            if (errors) {
                var message = errors == 1
                  ? "<h4 id=\"errorHeader\" tabindex=\"0\" role=\"alert\">Please fix the following " + errors + " error in your meeting information:</h4>"
                  : "<h4 id=\"errorHeader\" tabindex=\"0\" role=\"alert\">Please fix the following " + errors + " errors in your meeting information:</h4>";
                var errDiv = $("div.error");
                errDiv.attr("class", "text-danger")
                    .html(message)
                    .show();
                 $('#errorHeader').focus();
            }
        },
        submitHandler: function (form) {          
            $.ajax({
                url: "/api/events/",
                datatype: "json",
                method: "post",
                data: $("form").serializeArray(),
                success: function (data) {
                    $('#formContainer')[0].reset();
                    $('#tblOrgs tr').remove();
                    $('#events').DataTable().ajax.url('/api/Events/GetEventByOrganizer').load();
                    $('#events').attr("Summary", "My Meetings");
                    $('.customCRUD').show();
                    $('#eventStatus').text("Successfully Created an Event").css("color", "green");
                    $('#eventStatus').attr("tabIndex", "0");
                    $('#eventStatus').focus();
                    $("div.error").hide();
                },
                error: function (data) {
                    if (data.responseJSON.Message != null) {
                        $('#eventStatus').text(data.responseJSON.Message).css("color", "Red");
                    } else { $('#eventStatus').text("Unable to Create an Event").css("color", "Red"); }
                    $('#eventStatus').attr("tabIndex", "0");
                    $('#eventStatus').focus();
                    $("div.error").hide();

                         
                }
            });
        }
    });

    $('input[name=Organizer]').click(function () {
        if (this.id == "organizerN") {
            $('#addAdmin').hide();
            $('#orgErr').empty();
            $("#tblOrgs tr").remove();
        } else {
            $('#addAdmin').show();
        }
    });
    $('input[name=attendees]').click(function () {
        if (this.id == "attendeesN") {
            $('#maxnoofAttendees').hide();
            $('#IsRegistration').val(false);
        } else {
            $('#maxnoofAttendees').show();
           $('#IsRegistration').val(true);
        }
    });

    //$('input[name=meetingType]').click(function () {
    //    if (this.id === "oneDay") {
    //        $('#meetingType').val("oneday");
    //        //prefill enddate with start date
    //        $('#EventStartDate').change(function () {
    //            var startDate = $('#EventStartDate').val();
    //            $('#EventEndDate').val(startDate).attr('disabled', true);
    //        });
    //        //prefill enddate with start date in Modal
    //        } else {
    //        $('#meetingType').val("recurring");
    //        }
    //    alert($('#meetingType').val());
    //});

    $('#showCreateMeeting2').click(function () {
        $('#meetingDetails').toggle('slow', function () { });
        $('#cards').hide();
        $('#note').hide();
        $('#recurringFunctionality').hide();
        $('#menu').show();
        $('#attendeesN').prop('checked', true);
        $('#organizerN').prop('checked', true);
        $('header#create').css("min-height", "85%");
           });
    $('#mainMenu').click(function () {
        $('#cards').show();
        $('#note').show();
        $('header#create').css("min-height", "70%");
        $('#meetingDetails').hide();
        $('#recurringFunctionality').hide();
        $('#errormessages').hide();
        $('#eventStatus').text("");
        $("label.error").hide();
        $('#menu').hide();
        $('#timepickerVali').text("");
        $('#datepickerVali').text("");
        $('#eventVali').text("");
        $("input[type=text]").val("");
        $("input[type=radio]").prop('checked',false);
        $('#addAdmin').hide();
        $('#maxnoofAttendees').hide();
        $('#orgErr').empty();
        $('#tblOrgs').empty();
        $('input[name=Organizers]').remove();
    });
    $('#showRecurrance').click(function () {
        $('#recurringFunctionality').toggle('slow', function () { });
        $('#cards').hide();
        $('#note').hide();
        $('#meetingDetails').hide();
        $('#menu').show();
    });
    $('#orgPIN').keydown(function(e) {
        if (e.keyCode == 13) {
            e.preventDefault();
               $("#addOrganizerButton").click();
             }
      });
    // Get organizers

    $("#addOrganizerButton").click(function () {
            $.ajax({
            url: "/api/peopleAPI/GetUserInfo",
            data: { pinEmail: $("#orgPIN").val() },
            datatype: "json",
            success: function (data) {
                var $tr = $("<tr>");
                var $btn = $("<a>").append("remove").attr({"href":"#", "data-id" : data.SSA_Pin}).click(function () {
                    var removePIN = $(this).closest("td").val();
                    $(this).closest('tr').remove();
                    removeOrg($(this).attr("data-id"));
                });
                var $tdAction = $("<td>").append($btn);
                var orgName = data.FirstName + " " + data.LastName;
                var loggedUser = $("#LoggedInUser").val();
                var $tdCntnt = $("<td>").html("<strong>Additional Organizer: </strong>" + orgName);
                if (loggedUser.localeCompare(orgName) == 0) {
                    $("#orgErr").html("You are already an organizer").css("color", "red");
                } else{
                    if ($('input[name=Organizers]').length > 0) {
                        $('input[name=Organizers]').each(function () {
                            var pin = $(this).val();
                            if (pin == data.SSA_Pin) {
                                $("#orgErr").html("You are already an organizer").css("color", "red");
                            } else {
                                addOrg(data.SSA_Pin);
                                $tr.append($tdCntnt).append($tdAction);
                                $("#organizers > table").append($tr);
                                $('#orgPIN').val("");
                                $("#orgErr").html(" ");
                            }
                        });
                    } else {
                        addOrg(data.SSA_Pin);
                        $tr.append($tdCntnt).append($tdAction);
                        $("#organizers > table").append($tr);
                        $('#orgPIN').val("");
                        $("#orgErr").html(" ");
                    }
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 404 || errorThrown == 'Not Found') {
                    console.log('There was a 404 error.');
                    $("#orgErr").html("PIN/Email doesn't exist");
                }
                if (jqXHR.status == 400 || errorThrown == 'Bad Request') {
                    $("#orgErr").html(jqXHR.responseJSON.Message);
                }
            }
        });
       });

    function addOrg(org) {
        var $thisForm = $('#formContainer');
        var $orgInput = $('<input>').attr({
            'value': org,
            'name': 'Organizers',
            'type': 'hidden'
        });
        $thisForm.prepend($orgInput);
          }

    /**
        Finds input matching a specific value and remove the element
    */
    function removeOrg(org) {
        $("input[name=Organizers").each(function () {
            if ($(this).val() == org)   
                $(this).remove();
        }
    );
    }
    $("#btnregistration").click(function () {
        $.ajax({
            url: "/api/peopleAPI/GetUserInfo",
            data: { pinEmail: $("#attPIN").val() },
            datatype: "json",
            success: function (data) {
                var $tr = $("<tr>");
                var $btn = $("<a>").append("remove").attr("href", "#").click(function () {
                    $(this).closest('tr').remove();
                    var removeatt = $('table#tblAtt tbody').find('td:first').val();
                    $('#attPIN').val("");
                    $("#Participants").remove(removeatt);
                });
                var $tdAction = $("<td>").append($btn);
                var $tdCntnt = $("<td>").html(data.FirstName + " " + data.LastName);
                $("#Participants").add(data.SSA_Pin);
                $tr.append($tdCntnt).append($tdAction);
                $("#attendees > table").append($tr);
                $('#attPIN').val("");
                alert($("#Participants").val());
                },
            error: function (data) {
                $("attErr").html(data);
            }
        });
    });
    //prefill enddate with start date
    $('#EventStartDate').change(function () {
        var startDate = $('#EventStartDate').val();
        $('#EventEndDate').val(startDate).attr('disabled', true);
    });
    //prefill enddate with start date in Modal

    var typeEvent = location.search.split('id=')[1] ? location.search.split('id=')[1] : '0';
    var table = $('#events').DataTable({
        columnDefs: [{
            targets: 0,
            searchable:false,
            orderable:false,
            className: 'dt-body-center',
            render: function (data, type, full, meta) {
                return '<input type="checkbox" name="select Row" aria-labelledby="eventName Location" value="' + $('<div/>').text(data).html() + '">';
            }
        }],
        select: {
            style: 'single',
            info:false,
            selector: 'td:first-child'
        },
        iDisplayLength: 25,
        fnDrawCallback: function (oSettings) {
            $("table#events").removeAttr("role");
            var columnIndex = String(this.fnSettings().aaSorting);
            if (!!columnIndex && columnIndex.length > 0) {
               $("table#events > thead > tr>th").eq(columnIndex.substring(0, 1)).focus();
            }
            var $paginate = this.siblings('.dataTables_paginate');
            if (this.api().data().length <= this.fnSettings()._iDisplayLength) {
                $paginate.hide();
            }
            else {
                $paginate.show();
            }
        },
        "language": {
            "lengthMenu": " Select Rows _MENU_ ",
            "zeroRecords": "No Data Available",
            "info": "View _PAGE_ of _PAGES_",
            "sInfoEmpty": "View 0 of 0",
        },
        ajax:{
            url: "/api/Events/GetEventByOrganizer/",
            type: "GET" ,
            dataType: "JSON",
            data: "",
        },
        columns: [
                      {"data":"Id","sWidth":"2%"},
                      { "data": "EventName", "sWidth": "37%","fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                          $(nTd).html("<a title='"+oData.EventName+"' href='/Event/SignIn?id=" + oData.Id + "'>" + oData.EventName + "</a>");
                      }},
                      { "data": "Location", "sWidth": "20%" },
                      { "data": "EventStartDate", "sWidth": "10%" },
                      { "data": "EventStartTime", "sWidth": "10%" },
                      { "data": "EventEndDate", "sWidth": "10%" },
                      { "data": "EventEndTime", "sWidth": "11%" }

        ],
        //order: [[3, 'desc']],
        "aaSorting": [],
        "processing":true,
        "lengthChange": true,
        "sDom": '<"top"f>rt<"bottom col-xs-12"<"customCRUD"><"openSignIn">lpi>'
    });

    $("div.customCRUD").html('<button type="button" class="btn btn-info" data-toggle="modal" id="edit" data-target="#edit-modal">EDIT</button>&nbsp;<button type="button" class="btn btn-info" id="delete" data-toggle="modal" data-target="#delete-Modal">DELETE</button>&nbsp;');
    $("div.openSignIn").html('<button type="button" class="btn btn-info" id="openEvent">OPEN</button>&nbsp;&nbsp;');
    $('#events').attr("Summary", "Meetings created by Organizers");

    //Row Selection --Begin

    $('#events tbody').on('change', 'input[type="checkbox"]', function () {
        $("#events tbody").find("input[type='checkbox']").attr('checked', false);
        this.checked = true;
    });

    $('#events tbody').on('click', 'tr', function () {
        $("#events tbody").find("input[type='checkbox']").attr('checked', false);
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $(this).find("input[type='checkbox']").prop('checked', 'checked');
        }
    });
    //Row Selection --End
    $('#delete').click(function () {
        var rowdata = table.row('.selected').data();
        if (rowdata == null) {
            alert("Please select a row to Delete");
        } else{
            var rowId = rowdata["Id"];
            $.ajax({
                url: "/Event/Delete/",
                type: "POST",
                dataType: "JSON",
                data: { 'Id': rowId },
                success: function () {
                    $('#events').DataTable().ajax.url('/api/Events/GetEventByOrganizer/').load();
                },
                error: function (data) {
                }
            });
        }
    });

    $('#edit-modal').on('shown.bs.modal', function () {
       var rowdata = table.row('.selected').data();
       if (rowdata == null) {
           alert("Please select a row to Edit");
        } else {
            $('#edit-id').val(rowdata["Id"]);
            $('#eventNameModel').val(rowdata["EventName"]);
            $('#locationModel').val(rowdata["Location"]);
            $('#startDateU').val(rowdata["EventStartDate"]);
            $('#startTimeU').val(rowdata["EventStartTime"]);
            $('#endDateU').val(rowdata["EventEndDate"]);
            $('#endDateU').attr("disabled", "disabled");
            $('#endTimeU').val(rowdata["EventEndTime"]);
        }
    })
    $('#edit-modal').on('hide.bs.modal', function (e) {
        $("input[type=text]").val("");
        $('#datepickerValiU').text("");
        $('#timepickerValiU').text("");
        $('#eventStatusUpdate').text("");
    })
    $('#startDateU').change(function () {
        var startDate = $('#startDateU').val();
        $('#endDateU').val(startDate);
    });
    // Redirect to SignIn Page
    $('#openEvent').click(function () {
        var rowdata = $('#events').DataTable().row('.selected').data();
        if (rowdata == null) {
            alert("Please select a row to Open");
        } else {
            var rowId = rowdata["Id"];
            window.location.href = window.location.protocol +"//"+ window.location.host + "/Event/SignIn?id=" + rowId;
        }
    });

    $("#edit-form").on("submit", function (event) {
        var eventId = $('#edit-id').val();
        var eventName = $('#eventNameModel').val();
        var location = $('#locationModel').val();
        var startDate = $('#startDateU').val();
        var endDate = $('#endDateU').val();
        var startTime = $('#startTimeU').val();
        var endTime = $('#endTimeU').val();
        var today = Date.parse(moment().format("MM/DD/YYYY"));
        var sdateTime = Date.parse(moment(startDate + ' ' + startTime).format("MM/DD/YYYY hh:mm A"));
        var edateTime = Date.parse(moment(startDate + ' ' + endTime).format("MM/DD/YYYY hh:mm A"));
        var currentDateTime = Date.parse(moment().format("MM/DD/YYYY hh:mm A"));
        event.preventDefault();
       
        if (Date.parse(startDate) < today || sdateTime <= currentDateTime) {
            $('#eventStatusUpdate').text("Cannot update meeting with a past date or time.").css("color", "red");
        } else if (sdateTime >= edateTime) {
            $('#eventStatusUpdate').text("Cannot update meeting with invalid end time.").css("color", "red");
        } else
            {
            $('#eventStatusUpdate').text("");
        $.ajax({
                url: "/Event/Edit/",
                type: "POST",
                dataType: "JSON",
                data: {
                'Id': eventId, 'EventName': eventName, 'Location': location, 'EventStartDate': startDate,
                'EventStartTime': startTime, 'EventEndDate': endDate, 'EventEndTime': endTime
        },
                success: function () {
                $('#edit-modal').modal('hide');
                 $('#eventStatusUpdate').text(" ");
                $('#events').DataTable().ajax.url('/api/Events/GetEventByOrganizer/').load();
        },
                error: function (data) {
              }
        });
    }
    });
      
    setTimeout(function () {
        if (typeEvent == 1) {
            $('#meetingSection').text("My Meetings");
            $('#events').DataTable().ajax.url('/api/Events/GetEventByOrganizer').load();
            $('#events').attr("Summary", "My Meetings");
            $('.customCRUD').show();
            //$('#myMeetings').css('outline','thin dotted');
            $('#myMeetings').css('color', '#ffff80');
        } else if (typeEvent == 2) {
            $('#meetingSection').text("View Past Meetings");
            $('#events').DataTable().ajax.url('/api/Events/PastEvents').load();
            $('#events').attr("Summary", "View Past Meetings");
            $('.customCRUD').hide();
            $('#past').css('color', '#ffff80');
            //$('#past').css('outline','thin dotted');
        } else if (typeEvent == 3) {
            $('#meetingSection').text("View Current & Upcoming Meetings");
            $('#events').DataTable().ajax.url('/api/Events/UpcomingMeetings').load();
            $('#upcoming').css('color', '#ffff80');
            $('.customCRUD').hide();
            //$('#upcoming').css('outline','thin dotted');
            $('#events').attr("Summary", "View Current & Upcoming Meetings");
        } else if (typeEvent == 4) {
            $('#meetingSection').text("Meeting Administration");
            $('#events').DataTable().ajax.url('/api/Events/PastEvents').load();
            $('.customCRUD').show();
            $('#allMeetings').css('color', '#ffff80');
            //$('#allMeetings').css('outline','thin dotted');
            $('#events').attr("Summary", "Meeting Administartion");
        }
    }, 200);
                     
    $('#myMeetings').click(function () {
        $('.nav li.active a').css('color', 'white');
        $('#meetingSection').text("My Meetings");
        $('#events').DataTable().ajax.url('/api/Events/GetEventByOrganizer').load();
        $('.customCRUD').show();
        $(this).css('color', '#ffff80');
        $('#events').attr("Summary", "My Meetings");
    });
    $('#past').click(function () {
        $('.nav li.active a').css('color', 'white');
        $('#meetingSection').text("View Past Meetings");
        $('#events').DataTable().ajax.url('/api/Events/PastEvents').load();
        $('.customCRUD').hide();
        $('#events').attr("Summary", "View Past Meetings");
        $(this).css('color', '#ffff80');

    });
    $('#upcoming').click(function () {
        $('.nav li.active a').css('color', 'white');
        $('#meetingSection').text("View Current & Upcoming Meetings");
        $('#events').DataTable().ajax.url('/api/Events/UpcomingMeetings').load();
        $('.customCRUD').hide();
        $(this).css('color', '#ffff80');
        $(this).attr("href", "#Events");
        $('#events').attr("Summary", "View Current & Upcoming Meetings");
    });
    $('#allMeetings').click(function () {
        $('.nav li.active a').css('color', 'white');
        $('#meetingSection').text("Meeting Administration");
        $('#events').DataTable().ajax.url('/api/Events/PastEvents').load();
        $('.customCRUD').show();
        $(this).css('color', '#ffff80');
        $(this).attr("href", "#Events");
        $('#events').attr("Summary", "Meeting Administration");
    });
    $('.nav li').click(function (e) {
        $('.nav li').removeClass('active');
        $(this).addClass('active');
        //$(this).focus();
        //alert('stop it!');
        $(this).attr('tabindex', '0');
        // $(this).focus();
    });
    // Client Side Validation for fields-- Begin
    // Time Validation ---  Begin
    $('#EventEndTime').change(function () {
        var dateBegin = $("#EventStartDate").val();
        var dateEnd = $("#EventEndDate").val();
        var timeBegin = $('#EventStartTime').val();
        var timeEnd = $('#EventEndTime').val();
        var agStart = Date.parse(dateBegin + ' ' + timeBegin);
        var agEnd = Date.parse(dateEnd + ' ' + timeEnd);
        if (agStart > agEnd) {
            $('#timepickerVali').text("End time cannot be earlier than Start time").css("color", "red").focus();
        } else if (agStart == agEnd) {
            $('#timepickerVali').text("End time cannot be same as Start time").css("color", "red").focus();
        } else {
            $('#timepickerVali').text("");
        }
    })
    $('#endTimeU').change(function () {
        var dateBegin = $("#startDateU").val();
        var dateEnd = $("#endDateU").val();
        var timeBegin = $('#startTimeU').val();
        var timeEnd = $('#endTimeU').val();
        var agStart = Date.parse(dateBegin + ' ' + timeBegin);
        var agEnd = Date.parse(dateEnd + ' ' + timeEnd);
        if (agStart > agEnd) {
            $('#timepickerValiU').text("End time cannot be earlier than Start time").css("color", "red").focus();
        } else if (agStart == agEnd) {
            $('#timepickerValiU').text("End time cannot be same as Start time").css("color", "red").focus();
        } else {
            $('#timepickerValiU').text("");
            $('#eventStatusUpdate').text("");
        }
    })
    // Time Validation ---  End

    // Date Validation --- Start
    $("#EventStartDate").change(function () {
        var today = Date.parse(moment().format("MM/DD/YYYY"));
        var startDate = Date.parse($(this).val());
        if (startDate < today) {
            $('#datepickerVali').text("Start Date cannot be earlier than today").css("color", "red").focus();
        } else $('#datepickerVali').text("");
       });
    // Date Validation --- End
});
$("#startDateU").change(function(){
    var today = Date.parse(moment().format("MM/DD/YYYY"));
    var startDate = Date.parse($(this).val());
    if (startDate < today) {
        $('#datepickerValiU').text("Start Date cannot be earlier than today").css("color", "red").focus();
    } else {
        $('#datepickerValiU').text("");
        $('#eventStatusUpdate').text("");
    }
});

    function buildError(fieldName) {
    $('#' + fieldName).focus();
}

/**
      overrides brownser's default behavior of removing placeholders onclick
  */
function stationaryPlaceholder() {
    $("input").each(function () {
        var $this = $(this);
        $this.attr("data-placeholder", $this.attr("placeholder"));
    });
    var $currInputElm = null; // temp variable for input elements on click

    $("input").click(function () {
        $currInputElm = $(this);
        var elm = $("#EventName");
        if ($currInputElm.val() == "")
            $currInputElm.val($currInputElm.attr("data-placeholder"));
    }).keypress(function () {
        if ($currInputElm.val() == $currInputElm.attr("data-placeholder"))
            $currInputElm.val('');
    });

}

