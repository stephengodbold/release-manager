function highlightExceptionRows() {
    var state = $('#StateSelector').val();

    $('tr').removeClass('exception');
    $('tbody').children(":not([data-state='" + state + "'])").addClass('exception');
}

function getBuildsForDate() {
    var dateFilter = $('#dateSelector').val();

    $.ajax(
        {
            type: 'POST',
            url: 'Release/Builds/',
            data: 'date=' + dateFilter,
            beforeSend: function () { disableDateSelector(); },
            success: function (result) { populateBuildList(result); },
            error: function () { buildListError(); }
        });
}

function disableDateSelector() {
    $('.build-selector').attr('disabled', 'disabled');
}

function populateBuildList(results) {
    var buildSelect = $('#buildSelector');
    buildSelect.find('option').remove();

    $.each(results, function (index, value) {
        buildSelect.append($('<option>', {
            value: value,
            text: value
        }));
    });

    $('.build-selector').removeAttr('disabled');
}

function buildListError() {
    $('#messageBar')
        .html("<p>An error has occured while loading the builds. Try again soon</p>")
        .removeClass('hidden')
        .addClass('error', 'slow', 'swing');
}

function getWorkItems() {
    var previousBuild = $('#buildSelector').val();
    var currentBuild = $('#CurrentRelease').val();

    $('#previousReleaseName').html(previousBuild);
    $('#currentReleaseName').html(currentBuild);

    $.ajax(
        {
            type: 'POST',
            url: 'Release/WorkItems/',
            data: 'previousRelease=' + previousBuild +'&currentRelease=' + currentBuild,
            success: function (result) { populateReleaseNotes(result); },
            error: function () { releaseNotesError(); }
        });
}

function populateReleaseNotes(results) {
    var source = $("#workitem-template").html();
    var template = Handlebars.compile(source);

    $('#workitemList').html(template(results));
    $('#buildSelection').fadeOut('fast');
    $('#buildDetails').fadeIn('slow');
}

function releaseNotesError() {
    $('#messageBar')
        .html("<p>An error has occured while loading the work items. Try again soon</p>")
        .addClass('error')
        .slideDown('slow', 'swing');
}

$('#dateSelector').datepicker({ dateFormat: "yy-mm-dd" });