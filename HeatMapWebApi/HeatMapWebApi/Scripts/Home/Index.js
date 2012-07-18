$(document).ready(function () {
    $(".checkinSlider").slider({ 
            step : 1,
            max: 1000, 
            value : 100,
            slide: function () {
                $('#checkinCount').html($(".checkinSlider").slider("option", "value"));
            }
        }
    );
    $(".preloader").hide();
    $('#clearButton').hide();
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', showAnim: 'fade', prevText: '<<<', nextText: '>>>' });
    $("#logWrapper").hide();

    $('#checkinCount').html($(".checkinSlider").slider("option", "value"));
    

    $(".checkinSlider").mousedown(function () {
        var value = $(".checkinSlider").slider("option", "value");
        console.log(value);
    });

    $('#showMap').click(function () {
        if ($('#infovis').html != null)
            Clear(['infovis', 'log']);
        $("#logWrapper").hide();

        var dates = { dateFrom: $('#releaseDateFrom').val(), dateTo: $('#releaseDateTo').val() };
        var iteration = $("#iterationNo").val() != "" ? $("#iterationNo").val() : "Trunk";
        var path = $("#pathType").val() != "" ? $("#pathType").val() : "Reed";
        //var depth = $("#depth").val() != "" ? $("#depth").val() : 100;
        var checkins = $(".checkinSlider").slider("option", "value");
        jit.initHT('infovis', dates, iteration, path, checkins);
    });

    $('#clearButton').click(function () {
        $('#clearButton').hide('fade');
        $("#logWrapper").hide();
        Clear(['infovis']);
    });
});