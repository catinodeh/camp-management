var this_js_script = $('script[src*=registration]'); // or better regexp to get the file name..
var registrationid = this_js_script.attr('data-registrationid');

function doSearch() {
    $("#searchResult").empty();    

    $.ajax({
        url: '/Registrations/Search?criteria=' + $("#txtCriteria").val(),
        method: 'GET',
        cache: false,
        success: function (data, status, xhr, dataType) {
            if (typeof (data) == "object") {
                alert(data.Message);
                return;
            }
            $("#searchResult").html(data);
        }
    });
}

function addGuardian(guardianid, callback) {
    $("#setupContent").loading({ message : "Adding Guardian..." });

    $.ajax({
        url: '/Registrations/' + registrationid + '/AddGuardian/' + guardianid,
        method: 'POST',
        cache: false,
        success: function (data, status, xhr, dataType) {
            $("#setupContent").loading('toggle');
            if (typeof(data) == "object") {
                alert(data.Message);
                return;
            }
            $("#setupContent").empty().html(data);
            if (callback != undefined)
                callback(guardianid);
        },
        error: function() {
            $("#setupContent").loading('toggle');
        }
    });
}

function addCamper(camperid) {
    $("#setupContent").loading({ message: "Adding Camper..." });

    $.ajax({
        url: '/Registrations/' + registrationid + '/AddCamper/' + camperid,
        method: 'POST',
        cache: false,
        async: false,
        success: function (data, status, xhr, dataType) {
            $("#setupContent").loading('toggle');
            if (typeof (data) == "object") {
                alert(data.Message);
                return;
            }
            // not valid JSON
            $("#setupContent").html(data);
        },
        error: function() {
            $("#setupContent").loading('toggle');
        }
    });
}

function cancelCamper(camperid, url) {
    var result = confirm("Are you sure you want to cancel this camper's registration?");
    if (result == null || !result) return;

    $("#cancelRegistrationId").val(registrationid);
    $("#cancelCamperId").val(camperid);
    $("#redirectToUrl").val(url);

    $('#cancelRegistration').modal('show');
}

function removeCamper(camperid) {
    var result = confirm("Are you sure you want to remove this camper?");
    if (result == null || !result) return;

    $.ajax({
        url: '/Registrations/' + registrationid + '/RemoveCamper/' + camperid,
        method: 'DELETE',
        cache: false,
        success: function (data, status, xhr, dataType) {
            if (typeof (data) == "object") {
                alert(data.Message);
                return;
            }
            // not valid JSON
            $("#setupContent").html(data);
        },
        error: function() {
            alert('Error removing Guardian.');
        }
    });
}

function updateCamper(registrationCamperId) {
    var result = confirm("Are you sure you want to update this camper's info?");
    if (result == null || !result) return;

    var obj = {
        Grade: $("#Grade_" + registrationCamperId).val(),
        CampSetupId: $("#CampSetupId_" + registrationCamperId).val(),
        Price: $("#Price_" + registrationCamperId).val()
    }

    if (obj.Grade == "") {
        alert("Fill the Grade correctly");
        return;
    }

    if (obj.Price == "") {
        alert("Fill the Price correctly");
        return;
    }

    $.ajax({
        url: '/Registrations/' + registrationid + '/UpdateCamperSetup/' + registrationCamperId,
        method: 'PUT',
        cache: false,
        data: obj,
        success: function (data, status, xhr, dataType) {
            if (typeof (data) == "object") {
                alert(data.Message);
                return;
            }
            // not valid JSON
            $("#setupContent").html(data);
        }
    });
}

function removeGuardian(guardianid) {
    var result = confirm("Are you sure you want to remove this registration's guardian?");
    if (result == null || !result) return;

    $.ajax({
        url: '/Registrations/' + registrationid + '/RemoveGuardian/' + guardianid,
        method: 'DELETE',
        cache: false,
        success: function (data, status, xhr, dataType) {
            if (typeof (data) == "object") {
                alert(data.Message);
                return;
            }
            // not valid JSON
            $("#setupContent").html(data);
        }
    });
}

$("#txtCriteria").focus();

$(document).on("keypress", "#txtCriteria", function (e) {
    if (e.which == 13) {
        doSearch();
    }
});
$("#btnSearch")
    .click(function () {
        doSearch();
    });

$("#pnSearch")
    .on("click",
        "a[rel='addguardian']",
        function (e) {
            e.preventDefault();
            //First adding the guardian...
            addGuardian($(this).data("guardianid"), function(guardianid) {
                //Then adding the campers...
                $("a[rel='addcamper'][data-guardianid='" + guardianid + "']")
                    .each(function(i, item) {
                        addCamper($(item).data("camperid"));
                    });
            });
        });

$("#pnSearch")
    .on("click",
        "a[rel='addguardianonly']",
        function (e) {
            e.preventDefault();
            addGuardian($(this).data("guardianid"));
        });
$("#pnSearch")
    .on("click",
        "a[rel='addcamper']",
        function (e) {
            e.preventDefault();
            addCamper($(this).data("camperid"));
        });

function openExtraActivities(url, id) {

    $.ajax({
        url: url,
        method: "GET",
        cache: false,
        success: function(data) {
            $('#ea_' + id + ' .modal-body').html(data);
            $('#ea_' + id).modal('show');
            $("#ea_" + id + " button.btn-success").unbind("click").on("click", function () {
                saveExtraActivities(id);
            });
        }
    });
}

function saveExtraActivities(id) {
    if ($("#ea_" + id + " input[type='checkbox']").length === 0) {
        $('#ea_' + id).modal('hide');
        return;
    }
    console.log('saveExtraActivities');
    var ids = $("#ea_" + id + " input[type='checkbox']:checked").map(function () { return $(this).val(); }).get().join();
    console.log(ids);

    $.ajax({
        url: "/ExtraActivities/Update/" + id + "?activityIds=" + ids,
        data: ids,
        method: "PUT",
        cache: false,
        success: function (data) {
            if (data.Success) {
                $.get("/Registrations/CurrentSetup/" + registrationid,
                    function(data) {
                        $("#setupContent").html(data);
                    });
            } else {
                alert(data.Error);
            }
            $('#ea_' + id).modal('hide');
        }
    });
}