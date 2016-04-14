$(document).ready(function () {
    $('a[href^="matlab:"]').on('click', function (e) {
        e.preventDefault();
        var href = $(this).attr('href'),
            match = href.match(/matlab:(.*)/),
            matlabCommand = null;

        if (match) {
            matlabCommand = match[1];
        }

        if (matlabCommand) {
            $("#matlab-command-dialog #dialog-body #dialog-matlab-command").text(matlabCommand);
        } else {
            $("#matlab-command-dialog #dialog-body #dialog-matlab-command").hide();
        }
        $("#matlab-command-dialog").modal();

    });

	//processing for live script examples
    $('td[title="MATLAB Live Script"]').parent().find('a').off('click');
    $('td[title="MATLAB Live Script"]').parent().find('a').on('click', function (e) {
        e.preventDefault();
        var href = $(this).attr('href'),
            match = href.match(/matlab:(.*)/),
            matlabCommand = null;

        if (match) {
            matlabCommand = match[1];
        }

        var liveScriptFilename = matlabCommand.match(/edit\s+(.*)/)[1];
        var title = $(this).closest('tr').children('.example_element_desc').text();
		title = title.replace(/\n/, ' ');

        var livescript_template_url = "examples/livescripts.html";

        var documentUrl = $(location).attr('href');
        var example_url = documentUrl.substring(0, documentUrl.lastIndexOf("/") + 1) + "examples/livescripts/" + liveScriptFilename + ".mlx";

        window.location = livescript_template_url + "?title=" + title + "&example_url=" + example_url;
    })
});        