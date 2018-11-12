$().ready(function() {
    gmapsInit();
});
function addScript(url, callback) {
    var script = document.createElement('script');
    if (callback) script.onload = callback;
    script.type = 'text/javascript';
    script.src = url;
    document.body.appendChild(script);
}
function gmapsInit() {

    loadMapsAPI();
}
function loadMapsAPI() {
    if ($('#gmap').length) {
        addScript('https://maps.googleapis.com/maps/api/js?key=' + $('#GoogleAPI').val() + '&sensor=false&callback=mapsApiReady');
    }
}

function mapsApiReady() {
    mapInit();
}


function mapInit() {
    var marker;
    var mapOptions = {
        zoom: parseInt($('#Zoom').val()),
        center: new google.maps.LatLng(parseFloat($('#Lat').val()), parseFloat($('#Lng').val())),
        
    };

    var map = new google.maps.Map(document.getElementById('gmap'), mapOptions);
    map.panTo(mapOptions.center);

        marker = new google.maps.Marker({
            position: mapOptions.center,
            map: map,
            draggable: false,
            title: $('#addr-cell b').text()
        });


}
