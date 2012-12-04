(function(document) {
    'use strict';

    function highlightExceptionRows() {
        var state = $('#StateSelector').val();

        $('tr').removeClass('exception');
        $('tbody').children("[data-state='" + state + "']").addClass('exception');
    }

    function getBuildsForDate() {
        var dateFilter = $('#dateSelector').val();

        $.ajax(
            {
                type: 'POST',
                url: 'Release/Builds/',
                data: 'date=' + dateFilter,
                beforeSend: prepareToFetchBuilds,
                success: populateBuildList,
                error: buildListError
            });
    }

    function prepareToFetchBuilds() {
        $('.wide-selector').attr('disabled', 'disabled');
        $('#loadingAnimation').fadeIn();
        $('#messageBar').slideUp();
    }

    function populateBuildList(results) {
        var buildSelect = $('#buildSelector');
        buildSelect.find('option').remove();

        $.each(results, function(index, value) {
            buildSelect.append($('<option>', {
                value: value,
                text: value
            }));
        });

        $('#loadingAnimation').fadeOut();
        $('.wide-selector').removeAttr('disabled');
    }

    function buildListError() {
        $('#messageBar')
            .html("<p>An error has occured while loading the builds. Try again soon</p>")
            .removeClass('hidden')
            .addClass('error', 'slow', 'swing');
    }

    function getWorkItems(e) {
        var previousBuild = $('#buildSelector').val();
        var currentBuild = $('#CurrentRelease').val();

        $('#previousReleaseName').html(previousBuild);
        $('#currentReleaseName').html(currentBuild);

        var hrefValue = 'Release/WorkItems?previousRelease={0}&currentRelease={1}';
        $('#csvLink').attr('href', hrefValue.format(previousBuild, currentBuild));

        $.ajax(
            {
                type: 'POST',
                url: 'Release/WorkItems/',
                data: { previousRelease: previousBuild, currentRelease: currentBuild },
                beforeSend: prepareToFetchWorkItems,
                success: populateReleaseNotes,
                error: releaseNotesError
            });

        e.preventDefault();
    }

    function prepareToFetchWorkItems() {
        $('#loadingAnimation').fadeIn();
        $('#messageBar').slideUp();
    }

    function populateReleaseNotes(results) {
        populateStateSelector(results);
        populateWorkItems(results);

        if (results.count > 0) {
            $('#noRows').hide();
        }

        $('#loadingAnimation').fadeOut();
        $('#buildSelection').fadeOut('fast');
        $('#buildDetails').fadeIn('slow');
        $('#releaseNotes').fadeIn('slow');
    }

    function populateWorkItems(results) {
        var workItemSource = $("#workitem-template").html();
        var workItemTemplate = Handlebars.compile(workItemSource);

        $('#workitemList').find('tr').remove();
        $('#workitemList').html(workItemTemplate(results));
    }

    function populateStateSelector(results) {
        var stateSource = $("#state-template").html();
        var stateTemplate = Handlebars.compile(stateSource);

        $('#StateSelector').find('option').remove();
        $('#StateSelector').html(stateTemplate(results));
    }

    function releaseNotesError() {
        $('#messageBar')
            .html("<p>An error has occured while loading the work items. Try again soon</p>")
            .addClass('error')
            .slideDown('slow', 'swing');
        $('#loadingAnimation').fadeOut();
    }

    $('#dateSelector').datepicker({ dateFormat: "yy-mm-dd" });
    $('#loadingAnimation').fadeOut();

    $(document).on('change', '#StateSelector', highlightExceptionRows);
    $(document).on('change', '#dateSelector', getBuildsForDate);
    $(document).on('submit', 'form', getWorkItems);
    
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
})(document);