
$(document).ready(function () {

    $('#datatable-inventory').DataTable();
    $('#datatable-inventory2').DataTable();
    $('#datatable-sales').DataTable();
    $('#datatable-customers').DataTable();
 
    $( "#slider-range" ).slider({
        range: true,
        min: 0,
        max: 500,
        values: [ 75, 300 ],
        slide: function( event, ui ) {
            $( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
        }
    });
    $( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
      " - $" + $("#slider-range").slider("values", 1));


    console.log("hello");

    function getPeriodByMonth(start, choose, end){
        
        var period;

        if (start < choose)
            period = choose - start + 1;
        else if(choose == start)
            period = 1;
        else if(choose == end)
            period = 12;
        else period = (12 - start) + 1 + choose;
 

        return period;
    }

    $("#search-button").click(function () {
        var start_month_choosed = parseInt($("#start").attr('name')); //month choosed
        var end_month_choosed = parseInt($("#end").attr('name')); //month choosed
        var start = parseInt($("#saft-start-month").text()); 
        var end = parseInt($("#saft-end-month").text());

        var period1 = getPeriodByMonth(start, start_month_choosed);
        var period2 = getPeriodByMonth(start, end_month_choosed);

        if (period1 < period2)
            window.location = '/Sales/Index/' + period1 + "/" + period2;
        else window.location = '/Sales/Index/' + period2 + "/" + period1;
    });

    $(".startDate-d .dropdown-item").click(function () {
        $("#start").html($(this).text());
        $("#start").attr('name', $(this).attr('name'));
    });

    $(".endDate-d .dropdown-item").click(function () {
        $("#end").html($(this).text());
        $("#end").attr('name', $(this).attr('name'));
    });
} );



