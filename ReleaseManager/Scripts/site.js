function highlightExceptionRows() {
    var state = $('#StateSelector').val();

    $('tr').removeClass('exception');
    $('tbody').children(":not([data-state='" + state + "'])").addClass('exception');
}

function getBuildsForDate() {
    var dateFilter = $('#buildFilterDate').val();

    $.ajax(
        {
            type: 'POST',
            url: 'Release/Builds/',
            data: 'date=' + dateFilter,
            success: function (result) { populateBuildList(result); },
            error: function () { buildListError(); }
        });
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

    $('#buildSelectionDisplay').show('slow');
}

function buildListError() {
    $('#buildListError').show();
}

function getWorkItems() {
    var currentBuild = $('#CurrentRelease').val();
    var previousBuild = $('#buildSelector').val();

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
    var template = handlebars.compile(source);
    var content = template(results);

    $('#workitemList').html(content);
}

function releaseNotesError() {
    
}

$('#buildFilterDate').datepicker({ dateFormat: "yy-mm-dd" });
$('#buildSelectionDisplay').hide();
$('#buildListError').hide();