var map, layer, select, hover, control, gIcon, markers;

function init()
    {
    map=new OpenLayers.Map('map');
    var a =
        {
            numZoomLevels: 7, isBaseLayer: true
        };
        var b = new OpenLayers.Bounds(-180, -90, 180, 90);
    //change testmap.jpg to any image you might want
    var c=new OpenLayers.Layer.Image('SchoolMap!','testmap.jpg',b,new OpenLayers.Size(640,480),a);
    markers=new OpenLayers.Layer.Markers("markers");
    var d = new OpenLayers.Layer.WMS("NASA Global Mosaic", "http://t1.hypercube.telascience.org/cgi-bin/landsat7",
        {
            layers: "landsat7"
        }, a);
    var e=new OpenLayers.Size(32,32);
    var f=new OpenLayers.Pixel(-(e.w/2),-e.h);
    var gIcon = new OpenLayers.Icon('http://maps.google.com/mapfiles/kml/pushpin/red-pushpin.png', e, f);
    textlayer = new OpenLayers.Layer.Text("Referencias",
                    { location: GetValorFromObj("reffile"),
                        projection: map.displayProjection
                    });
    map.addLayers([c, d, markers, textlayer]);
    map.addControl(new OpenLayers.Control.LayerSwitcher());
    map.zoomToMaxExtent();
    map.addControl(new OpenLayers.Control.MousePosition())
}

function move_map(a,b)
    {
    map.panTo(new OpenLayers.LonLat(a,b));
    return false
}

function addmarker(a, b) {
    markers.addMarker(new OpenLayers.Marker(new OpenLayers.LonLat(a, b), gIcon));
}
