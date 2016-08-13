var getFirstPoint = false;
var getSecondPoint = false;

var g_lon = -57.57777;
var g_lat = -25.30900;

var lonbus = -57.55436;
var latbus = -25.22482;

var g_zoom = 12;
var zoombus = 11;
var act_secs_top = 15 * 1000;

var mapbus_ida = null, mapbus_vuelta = null;
var map = null, layer = null;
var geofence_features = null;

var targetLayer = new OpenLayers.Layer.Markers("TargetLayer");

OpenLayers.IMAGE_RELOAD_ATTEMPTS = 3;
OpenLayers.Util.onImageLoadErrorColor = "transparent";

OpenLayers.Control.Click = OpenLayers.Class(OpenLayers.Control, {
    defaultHandlerOptions: {
        'single': true,
        'double': false,
        'pixelTolerance': 0,
        'stopSingle': false,
        'stopDouble': false
    },

    initialize: function (options) {
        this.handlerOptions = OpenLayers.Util.extend(
                        {}, this.defaultHandlerOptions
                    );
        OpenLayers.Control.prototype.initialize.apply(
                        this, arguments
                    );
        this.handler = new OpenLayers.Handler.Click(
                        this, {
                            'click': this.trigger
                        }, this.handlerOptions
                    );
    },

    trigger: function (e) {
        var lonlat = map.getLonLatFromViewPortPx(e.xy);
        lonlat.transform(map.projection, map.displayProjection);
        if (document.getElementById("pto_txtLAT") != null &&
                        document.getElementById("pto_txtLON") != null) {
            var txtLAT = document.getElementById("pto_txtLAT");
            var txtLON = document.getElementById("pto_txtLON");
            if (txtLAT != null && txtLON != null) {
                txtLAT.value = lonlat.lat;
                txtLON.value = lonlat.lon;
            }
        }
    }
});

var gmap = null;
var ghyb = null;
var mapnik = null;
var osmarender = null;
var options = null;

function InicializarLayers() {
    try {
        options = {
            controls: [],
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326"),
            units: "m",
            numZoomLevels: 18,
            maxResolution: 156543.0339,
            maxExtent: new OpenLayers.Bounds(-20037508, -20037508,
                                     20037508, 20037508.34)
        };

        //if (G_HYBRID_MAP != null) {
        // create Google Mercator layers
        gmap = new OpenLayers.Layer.Google(
                    "Google - Mapa callejero",
                    { numZoomLevels: 20, 'sphericalMercator': true });
        // create Google Hybrid Layer
        ghyb = new OpenLayers.Layer.Google(
                    "Google - Satelital hibrido",
                    { type: google.maps.MapTypeId.HYBRID, numZoomLevels: 20, 'sphericalMercator': true });
        // create Google Hybrid Layer

        //}
        // create OSM layer
        mapnik = new OpenLayers.Layer.TMS(
                "OpenStreetMap - Mapa callejero alternativo 1",
                  "http://a.tile.openstreetmap.org/",
                {
                    type: 'png', getURL: osm_getTileURL,
                    displayOutsideMaxExtent: true
                });
        // create OSM layer
        osmarender = new OpenLayers.Layer.TMS(
                "OpenStreetMap - Mapa callejero alternativo 2",
                "http://tah.openstreetmap.org/Tiles/tile/",
                {
                    type: 'png', getURL: osm_getTileURL,
                    displayOutsideMaxExtent: true
                });
    }
    catch (err) {
        alert('Ha ocurrido un error, por favor refresque la página (F5).' + err);
    }
}

function MainGPS_init_map() {
    InicializarLayers();

    map = document.getElementById('map');
    if (map == null) return;

    map = new OpenLayers.Map('map', options);

    map.addControl(new OpenLayers.Control.MouseDefaults());
    map.addControl(new OpenLayers.Control.KeyboardDefaults());
    map.addControl(new OpenLayers.Control.PanZoomBar());
    map.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
    map.addControl(new OpenLayers.Control.MousePosition());

    map.addLayer(targetLayer);

    map.addLayers([gmap, ghyb, mapnik, osmarender]);

    map.raiseLayer(targetLayer, 0);

    textlayer = new OpenLayers.Layer.Text("Posiciones",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("txtfile"),
                projection: map.displayProjection
            });

    map.addLayer(textlayer);

    ref_layer = new OpenLayers.Layer.Text("Referencias",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("reffile"),
                projection: map.displayProjection
            });

    map.addLayer(ref_layer);

    map.setCenter(new OpenLayers.LonLat(g_lon, g_lat).transform(map.displayProjection, map.projection),
                             g_zoom);

    return true;
}

//////////////////////////////////////////////////
/////////////////// SECCION BUS //////////////////
//////////////////////////////////////////////////
var gmap_ida = null, gmap_vuelta = null;
var ghyb_ida = null, ghyb_vuelta = null;
var options_ida = null, options_vuelta = null;
function bus_InicializarLayers() {
    try {
        options_ida = {
            controls: [],
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326"),
            units: "m",
            numZoomLevels: 18,
            maxResolution: 156543.0339,
            maxExtent: new OpenLayers.Bounds(-20037508, -20037508,
                                     20037508, 20037508.34)
        };

        options_vuelta = {
            controls: [],
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326"),
            units: "m",
            numZoomLevels: 18,
            maxResolution: 156543.0339,
            maxExtent: new OpenLayers.Bounds(-20037508, -20037508,
                                     20037508, 20037508.34)
        };

        if (G_HYBRID_MAP != null) {
            // create Google Mercator layers
            gmap_ida = new OpenLayers.Layer.Google(
                    "Google - Mapa callejero",
                    { 'sphericalMercator': true });
            // create Google Hybrid Layer
            ghyb_ida = new OpenLayers.Layer.Google(
                    "Google - Satelital hibrido",
                    { type: G_HYBRID_MAP, 'sphericalMercator': true });
            // create Google Mercator layers
            gmap_vuelta = new OpenLayers.Layer.Google(
                    "Google - Mapa callejero",
                    { 'sphericalMercator': true });
            // create Google Hybrid Layer
            ghyb_vuelta = new OpenLayers.Layer.Google(
                    "Google - Satelital hibrido",
                    { type: G_HYBRID_MAP, 'sphericalMercator': true });
            // create OSM layer
            mapnik = new OpenLayers.Layer.TMS(
                "OpenStreetMap - Mapa callejero alternativo 1",
                  "http://a.tile.openstreetmap.org/",
                {
                    type: 'png', getURL: osm_getTileURL,
                    displayOutsideMaxExtent: true
                });
        }
    }
    catch (exception) {
        alert('Ha ocurrido un error, por favor refresque la página (F5). B7346ERR');
    }
}

function gbus_UpdateRef(queue) {
    if (mapbus_ida != null) {
        var idaoldtextlayer = mapbus_ida.getLayersByName("Referencias");
        if (idaoldtextlayer.length > 0) {
            mapbus_ida.removeLayer(idaoldtextlayer[0]);
            idaoldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Referencias",
                    { location: GetValorFromObj("reffile"),
                        projection: mapbus_ida.displayProjection
                    });
            mapbus_ida.addLayer(textlayer);
        }
    }
    if (mapbus_vuelta != null) {
        var vueltaoldtextlayer = mapbus_vuelta.getLayersByName("Referencias");
        if (vueltaoldtextlayer.length > 0) {
            mapbus_vuelta.removeLayer(vueltaoldtextlayer[0]);
            vueltaoldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Referencias",
                { location: GetValorFromObj("reffile"),
                    projection: mapbus_vuelta.displayProjection
                });
            mapbus_vuelta.addLayer(textlayer);
        }
    }
}

function gbus_UpdateMap(queue) {
    if (mapbus_ida != null) {
        var idaoldtextlayer = mapbus_ida.getLayersByName("Posiciones");
        if (idaoldtextlayer.length > 0) {
            mapbus_ida.removeLayer(idaoldtextlayer[0]);
            idaoldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Posiciones",
                    { location: GetValorFromObj("txtfile_ida"),
                        projection: mapbus_ida.displayProjection
                    });
            mapbus_ida.addLayer(textlayer);
        }
    }
    if (mapbus_vuelta != null) {
        var vueltaoldtextlayer = mapbus_vuelta.getLayersByName("Posiciones");
        if (vueltaoldtextlayer.length > 0) {
            mapbus_vuelta.removeLayer(vueltaoldtextlayer[0]);
            vueltaoldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Posiciones",
                { location: GetValorFromObj("txtfile_vuelta"),
                    projection: mapbus_vuelta.displayProjection
                });
            mapbus_vuelta.addLayer(textlayer);
        }
    }
    if (queue == true) setTimeout("gbus_UpdateMap(true)", act_secs_top);
}

function busida_map_panTo(lon, lat) {
    if (mapbus_ida != null) {
        mapbus_ida.panTo(new OpenLayers.LonLat(lon, lat).transform(mapbus_ida.displayProjection,
                                                                    mapbus_ida.projection));
    }
}
function busvuelta_map_panTo(lon, lat) {
    if (mapbus_vuelta != null) {
        mapbus_vuelta.panTo(new OpenLayers.LonLat(lon, lat).transform(mapbus_vuelta.displayProjection,
                                                                      mapbus_vuelta.projection));
    }
}

function busmain_init_map() {
    try {
        bus_InicializarLayers();

        mapbus_ida = document.getElementById('mapbus_ida');
        mapbus_vuelta = document.getElementById('mapbus_vuelta');
        if (mapbus_ida == null || mapbus_vuelta == null) {
            return;
        }

        mapbus_ida = new OpenLayers.Map('mapbus_ida', options_ida);
        mapbus_vuelta = new OpenLayers.Map('mapbus_vuelta', options_vuelta);

        mapbus_ida.addControl(new OpenLayers.Control.MouseDefaults());
        //mapbus_ida.addControl(new OpenLayers.Control.KeyboardDefaults());
        mapbus_ida.addControl(new OpenLayers.Control.PanZoom());
        mapbus_ida.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
        mapbus_ida.addControl(new OpenLayers.Control.MousePosition());
        mapbus_ida.addLayers([gmap_ida, ghyb_ida]);
        mapbus_ida.raiseLayer(gmap_ida, 1);

        mapbus_vuelta.addControl(new OpenLayers.Control.MouseDefaults());
        //mapbus_vuelta.addControl(new OpenLayers.Control.KeyboardDefaults());
        mapbus_vuelta.addControl(new OpenLayers.Control.PanZoom());
        mapbus_vuelta.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
        mapbus_vuelta.addControl(new OpenLayers.Control.MousePosition());
        mapbus_vuelta.addLayers([gmap_vuelta, ghyb_vuelta]);
        mapbus_vuelta.raiseLayer(gmap_vuelta, 1);

        textlayer_ida = new OpenLayers.Layer.Text("Posiciones",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("txtfile_ida"),
                projection: mapbus_ida.displayProjection
            });

        textlayer_vuelta = new OpenLayers.Layer.Text("Posiciones",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("txtfile_vuelta"),
                projection: mapbus_vuelta.displayProjection
            });

        mapbus_ida.addLayer(textlayer_ida);
        mapbus_vuelta.addLayer(textlayer_vuelta);

        ref_layer_ida = new OpenLayers.Layer.Text("Referencias",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("reffile"),
                projection: mapbus_ida.displayProjection
            });
        ref_layer_vuelta = new OpenLayers.Layer.Text("Referencias",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("reffile"),
                projection: mapbus_vuelta.displayProjection
            });

        mapbus_ida.addLayer(ref_layer_ida);
        mapbus_vuelta.addLayer(ref_layer_vuelta);

        mapbus_ida.setCenter(new OpenLayers.LonLat(lonbus, latbus).transform(mapbus_ida.displayProjection,
        mapbus_ida.projection),
        zoombus);
        mapbus_vuelta.setCenter(new OpenLayers.LonLat(lonbus, latbus).transform(mapbus_ida.displayProjection,
        mapbus_ida.projection),
        zoombus);
        return true;
    }
    catch (exception) {
        //busmain_init_map();
        //alert(exception);
    }
}

//////////////////////////////////////////////////
/////////////////// SECCION BUS //////////////////
//////////////////////////////////////////////////

function Historico_init_map() {
    InicializarLayers();

    map = document.getElementById('map');
    if (map == null) return;

    map = new OpenLayers.Map('map', options);

    map.addControl(new OpenLayers.Control.MouseDefaults());
    map.addControl(new OpenLayers.Control.KeyboardDefaults());
    map.addControl(new OpenLayers.Control.PanZoomBar());
    map.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
    map.addControl(new OpenLayers.Control.MousePosition());

    map.addLayer(targetLayer);

    map.addLayers([gmap, ghyb, mapnik, osmarender]);

    map.raiseLayer(ghyb, 1);

    textlayer = new OpenLayers.Layer.Text("Posiciones",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("txtfile"),
                projection: map.displayProjection
            });

    map.addLayer(textlayer);

    map.setCenter(new OpenLayers.LonLat(g_lon, g_lat).transform(map.displayProjection, map.projection),
                             g_zoom);

    return true;
}

function GPSadmin_init_map() {
    InicializarLayers();

    map = document.getElementById('map');

    if (map == null) return;

    map = new OpenLayers.Map('map', options);

    map.addControl(new OpenLayers.Control.MouseDefaults());
    map.addControl(new OpenLayers.Control.KeyboardDefaults());
    map.addControl(new OpenLayers.Control.PanZoomBar());
    map.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
    map.addControl(new OpenLayers.Control.MousePosition());

    map.addLayer(targetLayer);

    textlayer = new OpenLayers.Layer.Text("Posiciones",
                    { 'calculateInRange': function () { return true; },
                        location: GetValorFromObj("txtfile"),
                        projection: map.displayProjection
                    });

    map.addLayer(textlayer);

    ref_layer = new OpenLayers.Layer.Text("Referencias",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("reffile"),
                projection: map.displayProjection
            });

    map.addLayer(ref_layer);

    map.addLayers([gmap, ghyb, mapnik, osmarender]);
    map.raiseLayer(ghyb, 1);

    map.setCenter(new OpenLayers.LonLat(g_lon, g_lat).transform(map.displayProjection, map.projection),
                             g_zoom);

    //setTimeout("g_UpdateMap(false)", act_secs_top);

    return true;
}

function hojaderuta_init_map() {
    InicializarLayers();

    map = document.getElementById('map');
    if (map == null) return;

    map = new OpenLayers.Map('map', options);

    map.addLayer(targetLayer);

    map.addLayers([ghyb, gmap, mapnik, osmarender]);
    map.raiseLayer(ghyb, 1);

    var click = new OpenLayers.Control.Click();
    map.addControl(click);
    click.activate();

    map.addControl(new OpenLayers.Control.MouseDefaults());
    map.addControl(new OpenLayers.Control.KeyboardDefaults());
    map.addControl(new OpenLayers.Control.PanZoomBar());
    map.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
    map.addControl(new OpenLayers.Control.MousePosition());

    textlayer = new OpenLayers.Layer.Text("Posiciones",
                    { 'calculateInRange': function () { return true; },
                        location: GetValorFromObj("txtfile"),
                        projection: map.displayProjection
                    });

    map.addLayer(textlayer);

    map.setCenter(new OpenLayers.LonLat(g_lon, g_lat).transform(map.displayProjection, map.projection),
                             g_zoom);

    return true;
}

geocercas = new OpenLayers.Layer.Vector("Geocercas");
var v_options_geocerca = {
    hover: true,
    onSelect: my_serialize_geocerca
};
var select = new OpenLayers.Control.SelectFeature(geocercas, v_options_geocerca);

function geocerca_init_map() {
    InicializarLayers();

    map = document.getElementById('map');
    if (map == null) return;

    map = new OpenLayers.Map('map', options);

    map.addLayer(targetLayer);

    map.addLayers([gmap, ghyb, mapnik, osmarender, geocercas]);

    map.addControl(new OpenLayers.Control.MouseDefaults());
    map.addControl(new OpenLayers.Control.KeyboardDefaults());
    map.addControl(new OpenLayers.Control.PanZoomBar());
    map.addControl(new OpenLayers.Control.LayerSwitcher({ 'ascending': false }));
    map.addControl(new OpenLayers.Control.MousePosition());
    map.addControl(new OpenLayers.Control.EditingToolbar(geocercas));
    map.addControl(select);
    select.activate();

    textlayer = new OpenLayers.Layer.Text("Posiciones",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("txtfile"),
                projection: map.displayProjection
            });

    map.addLayer(textlayer);

    ref_layer = new OpenLayers.Layer.Text("Referencias",
            { 'calculateInRange': function () { return true; },
                location: GetValorFromObj("reffile"),
                projection: map.displayProjection
            });

    map.addLayer(ref_layer);

    map.setCenter(new OpenLayers.LonLat(g_lon, g_lat).transform(map.displayProjection, map.projection),
                             g_zoom);

    return true;
}

function my_serialize_geocerca(feature) {
    if (map != null) {
        var type = "wkt";
        var out_options = {
            'internalProjection': map.baseLayer.projection,
            'externalProjection': new OpenLayers.Projection("EPSG:4326")
        };
        var str = new OpenLayers.Format.WKT(out_options).write(feature, false);
        //alert(str);
        if (str != "") {
            var currentTime = new Date();
            var hh = currentTime.getHours();
            var mm = currentTime.getMinutes();
            var ss = currentTime.getSeconds();
            var msjgeo = document.getElementById('mensajegeocerca');
            if (msjgeo != null) {
                msjgeo.value = "Geocerca seleccionada a las " + hh + ":" + mm + ":" + ss;
                document.getElementById('insGeocercaPuntos').value = str;
            }
        }
    }
}

function my_deserialize_geocerca() {
    if (geofence_features != null)
        geocercas.removeFeatures(geofence_features);
    var element = document.getElementById('geocerca_puntos');
    var type = "wkt";
    var in_options = {
        'internalProjection': map.baseLayer.projection,
        'externalProjection': new OpenLayers.Projection("EPSG:4326")
    };
    geofence_features = new OpenLayers.Format.WKT(in_options).read(element.value);
    var bounds;
    if (geofence_features) {
        if (geofence_features.constructor != Array) {
            geofence_features = [geofence_features];
        }
        for (var i = 0; i < geofence_features.length; ++i) {
            if (!bounds) {
                bounds = geofence_features[i].geometry.getBounds();
            } else {
                bounds.extend(geofence_features[i].geometry.getBounds());
            }
        }
        geocercas.addFeatures(geofence_features);
        map.zoomToExtent(bounds);
    }
}

function g_UpdateMap(queue) {
    if (map != null) {
        var oldtextlayer = map.getLayersByName("Posiciones");
        if (oldtextlayer.length > 0) {
            map.removeLayer(oldtextlayer[0]);
            oldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Posiciones",
                    { location: GetValorFromObj("txtfile"),
                        projection: map.displayProjection
                    });
            map.addLayer(textlayer);
            if (queue == true) setTimeout("g_UpdateMap(true)", act_secs_top);
        }
    }
}
function g_UpdateRef(queue) {
    if (map != null) {
        var oldtextlayer = map.getLayersByName("Referencias");
        if (oldtextlayer.length > 0) {
            map.removeLayer(oldtextlayer[0]);
            oldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Referencias",
                    { location: GetValorFromObj("reffile"),
                        projection: map.displayProjection
                    });
            map.addLayer(textlayer);
            if (queue == true) setTimeout("g_UpdateRef(true)", act_secs_top);
        }
    }
}
function hojaderuta_UpdateMap(queue, varlocation) {
    if (map != null) {
        var oldtextlayer = map.getLayersByName("Posiciones");
        if (oldtextlayer.length > 0) {
            map.removeLayer(oldtextlayer[0]);
            oldtextlayer[0].destroy();
            textlayer = new OpenLayers.Layer.Text("Posiciones",
                    { location: varlocation,
                        projection: map.displayProjection
                    });
            map.addLayer(textlayer);
            if (queue == true) setTimeout("hojaderuta_UpdateMap(true, " + varlocation + ")", act_secs_top);
        }
    }
}

function hojaderuta_UpdateMark(zlon, zlat) {
    if (map != null) {
        var markers = map.getLayersByName("Marcas");
        if (markers.length > 0) {
            map.removeLayer(markers[0]);
        }
        markers = new OpenLayers.Layer.Markers("Marcas");
        var _size = new OpenLayers.Size(15, 15);
        var _icon = new OpenLayers.Icon('./img/center_normal.gif', _size);
        var mi_marca = new OpenLayers.Marker(new OpenLayers.LonLat(zlon, zlat).transform(map.displayProjection, map.projection), _icon);
        markers.addMarker(mi_marca);
        map.addLayer(markers);
    }
}

function osm_getTileURL(bounds) {
    if (map != null) {
        if (bounds != null) {
            var res = this.map.getResolution();
            var x = Math.round((bounds.left - map.maxExtent.left) / (res * map.tileSize.w));
            var y = Math.round((map.maxExtent.top - bounds.top) / (res * map.tileSize.h));
            var z = this.map.getZoom();
            var limit = Math.pow(2, z);

            if (y < 0 || y >= limit) {
                return OpenLayers.Util.getImagesLocation() + "404.png";
            } else {
                x = ((x % limit) + limit) % limit;
                return this.url + z + "/" + x + "/" + y + "." + this.type;
            }
        }
    }
}

//Especificamente funciona paginas que contengan los controles llamados txtLAN y txtLON
function setLONLAT_Historico(llon, llat) {
    if (document.getElementById("txtLON") != null &&
                        document.getElementById("txtLAT") != null) {
        var txtLAT = document.getElementById("txtLAT");
        var txtLON = document.getElementById("txtLON");
        if (txtLAT != null && txtLON != null) {
            txtLAT.value = llat;
            txtLON.value = llon;
        }
    }
}

//Especificamente funciona paginas que contengan los controles llamados txtLAN y txtLON
function setINFO_ForMail(idrastreo, lon, lat, velocidad, rumbo) {
    if (document.getElementById("mailID") != null &&
        document.getElementById("mailLAT") != null &&
        document.getElementById("mailLON") != null) {
        var mailID = document.getElementById("mailID");
        var mailLAT = document.getElementById("mailLON");
        var mailLON = document.getElementById("mailLON");
        if (mailID != null) {
            mailID.value = idrastreo;
        }
        if (mailLAT != null && mailLON != null) {
            mailLAT.value = llat;
            mailLON.value = llon;
        }
        if (document.getElementById("mailSPD") != null) {
            var mailSPD = document.getElementById("mailSPD");
            if (mailSPD != null) {
                mailSPD.value = velocidad;
            }
        }
        if (document.getElementById("mailRUMBO") != null) {
            var mailRUMBO = document.getElementById("mailRUMBO");
            if (mailRUMBO != null) {
                mailRUMBO.value = rumbo;
            }
        }
    }
}

function get_g_zoom() {
    if (document.getElementById("g_zoom_value") != null) {
        var g_zoom_value = document.getElementById("g_zoom_value");
        if (map.getZoom() == 12)
            g_zoom_value.value = 15;
        else
            g_zoom_value.value = map.getZoom();
    }
}

function googleFindAddress(address) {
    // Create new geocoding object
    geocoder = new GClientGeocoder();

    // Retrieve location information, pass it to addToMap()
    //alert(address);
    geocoder.getLocations(address, AddToMap);
}

// This function adds the point to the map

function AddToMap(response) {
    // Retrieve the object
    var found = false;
    if (response != null) {
        if (response.Placemark != null) {
            if (response.Placemark.length > 0) {
                address = response.Placemark[0];
                // Retrieve the latitude and longitude AND
                // Center the map on this point
                map_panTo(address.Point.coordinates[0],
              address.Point.coordinates[1]);
                found = true;
            }
        }
    }
    if (!found) {
        alert('No puedo encontrar dicha direccion, intente de nuevo.');
    }
}

function map_panTo(lon, lat) {
    if (map != null) {
        targetLayer.clearMarkers();
        var target_size = new OpenLayers.Size(30, 30);
        //var target_offset = new OpenLayers.Pixel(0, 0);
        var target_icon = new OpenLayers.Icon('./App_themes/CENTRAL/Imagenes/target.png', target_size, new OpenLayers.Pixel(-15, -15));
        var targetLocked = new OpenLayers.Marker(new OpenLayers.LonLat(lon, lat).transform(map.displayProjection, map.projection), target_icon);
        targetLocked.setOpacity(0.7);
        targetLayer.addMarker(targetLocked);
        map.panTo(new OpenLayers.LonLat(lon, lat).transform(map.displayProjection, map.projection));
    }
}

//function map_panTo(lon, lat) {
//    if (map != null) {
//        targetLayer.clearMarkers();
//        var target_size = new OpenLayers.Size(30, 30);
//        //var target_offset = new OpenLayers.Pixel(0, 0);
//        var target_icon = new OpenLayers.Icon('./img/center_normal.gif', target_size, null);
//        var targetLocked = new OpenLayers.Marker(new OpenLayers.LonLat(lon, lat).transform(map.displayProjection, map.projection), target_icon);
//        targetLocked.setOpacity(0.7);
//        targetLayer.addMarker(targetLocked);
//        map.panTo(new OpenLayers.LonLat(lon, lat).transform(map.displayProjection, map.projection));
//    }
//}