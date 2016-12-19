function doSearch() {
    $("#pnSearch,#pnSetup").show();
    $.ajax({
        url: '/Registrations/Search?criteria=' + $("#txtCriteria").val(),
        method: 'GET',
        cache: false,
        success: function (data) {
            $("#searchResult").empty().html(data);
        }
    });
}

function addGuardian(guardianid) {
    $("#setupContent").find("#guardianInfo").remove();
    $.ajax({
        url: '/Registrations/AddGuardian/' + guardianid,
        method: 'POST',
        cache: false,
        success: function (data) {
            $("#setupContent").prepend(data);
        }
    });
}

function addCamper(camperid) {
    if ($("#camper" + camperid).length > 0) {
        alert('Camper already added.');
        return;
    }
    $.ajax({
        url: '/Registrations/AddCamper/' + camperid,
        method: 'POST',
        cache: false,
        success: function (data) {
            $("#setupContent").append(data);
        }
    });
}

function removeCamper(camperid) {
    $("#camper" + camperid).remove();
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
        function () {
            alert('addguardian');
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
            addCamper($(this).data("guardianid"));
        });