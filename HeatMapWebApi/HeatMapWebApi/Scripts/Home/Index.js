$(document).ready(function () {
    $('#clearButton').hide();
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', showAnim: 'fade', prevText: '<<<', nextText: '>>>' });

    $('#showMap').click(function () {
        if ($('#infovis').html != null)
            Clear(['infovis', 'log']);

        var dates = { dateFrom: $('#releaseDateFrom').val(), dateTo: $('#releaseDateTo').val() };
        var iteration = $("#iterationNo").val() != "" ? $("#iterationNo").val() : "Trunk";
        var path = $("#pathType").val() != "" ? $("#pathType").val() : "Reed";
        var depth = $("#depth").val() != "" ? $("#depth").val() : 100;
        //initHT('infovis', dates, iteration, path, depth);
        initHT('infovis', dates, iteration, path, depth);

        $('#clearButton').show('fade');
    });

    $('#clearButton').click(function () {
        $('#clearButton').hide('fade');
        Clear(['infovis']);
    });
});