
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

    $("#financial-search-button").click(function () {
        var start_month_choosed = parseInt($("#start").attr('name')); //month choosed
        var end_month_choosed = parseInt($("#end").attr('name')); //month choosed
        var start = parseInt($("#saft-start-month").text());
        var end = parseInt($("#saft-end-month").text());

        var period1 = start_month_choosed;
        var period2 = end_month_choosed;

        if (period1 < period2)
            window.location = '/Financial/Index/' + period1 + "/" + period2;
        else window.location = '/Financial/Index/' + period2 + "/" + period1;
    });

    $(".startDate-d .dropdown-item").click(function () {
        var start = parseInt($("#saft-start-month").text());


        $("#start").html($(this).text());
        $("#start").attr('name', $(this).attr('name'));


        var month = parseInt($(this).attr('name'));
        var end = parseInt($("#saft-end-month").text());
        console.log(month);

        newM = month;

        do {
            var newM = newM + 1;
            var month = getMonth(newM);
            if (newM <= 12)
                $(".endDate-d .dropdown-menu").append(' <a class="dropdown-item" name=' + newM + ' href="#">' + month + '</a>')
            else {
                newM = 1;
                month = getMonth(newM);
                $(".endDate-d .dropdown-menu").append(' <a class="dropdown-item" name=' + newM + ' href="#">' + month + '</a>')
            }
        }while (newM != end) 
       

        

    });

    function getMonth(month) {
        var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        return months[month - 1];
    }

    $(".endDate-d .dropdown-item").click(function () {
        $("#end").html($(this).text());
        $("#end").attr('name', $(this).attr('name'));
    });
} );



