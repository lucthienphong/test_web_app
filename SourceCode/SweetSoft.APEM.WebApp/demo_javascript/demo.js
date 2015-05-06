$(document).ready(function () {
    $('#customername').typeahead({
        //co the load du lieu bang ajax
        source: [
            'Toronto',
            'Montreal',
            'New York',
            'Buffalo',
            'Boston',
            'Columbus',
            'Dallas',
            'Vancouver',
            'Seattle',
            'Los Angeles']
    });
});
