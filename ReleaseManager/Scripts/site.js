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

    buildSelect.show('slow');
}

function buildListError() {
    $('#buildListError').show();
}

$('#buildFilterDate').datepicker({ dateFormat: "yy-mm-dd" });
$('#buildSelector').hide();
$('#buildListError').hide();