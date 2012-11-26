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
            success: populateBuildList(result),
            error: buildListError()
        });
}

function populateBuildList(results) {
    var buildList = $('#buildSelector');

    for (var result in results) {
        buildList.add('<option>' + result + '</option>');
    }
}

function buildListError() {
    $('#buildListError').show();
}

$('#buildFilterDate').datepicker();