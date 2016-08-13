using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace KMLCreator
{

    /// <summary>
    /// http://earth.google.com/kml/kml_tags_21.html
    /// </summary>

    enum KMLAltitudeMode : byte
    {
        Absolute,
        ClampToGround,
        RelativeToGround
    }

    class KMLCoordinates
    {
        Double _Latitude = 0.0; // Gps_latitud
        Double _Longitude = 0.0; // Gps_longitud
        Double _Altitude = 0.0;

        public KMLCoordinates()
        { }

        public KMLCoordinates(Double Longitude, Double Latitude)
        {
            _Longitude = Longitude;
            _Latitude = Latitude;
        }

        public KMLCoordinates(Double Longitude, Double Latitude, Double Altitude)
        {
            _Longitude = Longitude;
            _Latitude = Latitude;
            _Altitude = Altitude;
        }

        public Double Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        public Double Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public Double Altitude
        {
            get { return _Altitude; }
            set { _Altitude = value; }
        }

    }

    class KMLPoint
    {
        KMLCoordinates _Coords;
        KMLAltitudeMode _AltitudeMode = KMLAltitudeMode.ClampToGround;

        String _KMLFilePath = "C:\\";
        String _KMLFileName = "Point.kml";

        String _Name = "";
        String _Description = "";

        public KMLPoint()
        { }

        public KMLPoint(KMLCoordinates Coords)
        {
            _Coords = Coords;
        }

        public KMLPoint(KMLCoordinates Coords, String Name)
        {
            _Name = Name;
            _Coords = Coords;
        }

        public KMLPoint(KMLCoordinates Coords, String Name, String Description)
        {
            _Description = Description;
            _Name = Name;
            _Coords = Coords;
        }

        public KMLCoordinates Coords
        {
            get { return _Coords; }
            set { _Coords = value; }
        }

        public KMLAltitudeMode AltitudeMode
        {
            get { return _AltitudeMode; }
            set { _AltitudeMode = value; }
        }

        public string KMLFilePath
        {
            get { return _KMLFilePath; }
            set
            {
                _KMLFilePath = value;

                if (_KMLFilePath.Substring(_KMLFilePath.Length - 1, 1) != "\\")
                    _KMLFilePath += "\\";

            }
        }

        public string KMLFileName
        {
            get { return _KMLFileName; }
            set { _KMLFileName = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public void Save()
        {
            string FullFilePath = _KMLFilePath + _KMLFileName;

            XmlTextWriter xtr = new XmlTextWriter(FullFilePath, null);

            xtr.WriteStartDocument();

            xtr.WriteStartElement("kml");
            xtr.WriteString(" xmlns=\"http://earth.google.com/kml/2.1\"");
            {
                xtr.WriteStartElement("Placemark");
                {
                    xtr.WriteStartElement("Name");
                    xtr.WriteString(_Name);
                    xtr.WriteEndElement();

                    xtr.WriteStartElement("Description");
                    xtr.WriteString(_Description);
                    xtr.WriteEndElement();

                    xtr.WriteStartElement("LookAt");
                    {
                        xtr.WriteStartElement("longitude");
                        xtr.WriteString(_Coords.Longitude.ToString());
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("latitude");
                        xtr.WriteString(_Coords.Latitude.ToString());
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("range");
                        xtr.WriteString("400");
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("tilt");
                        xtr.WriteString("0");
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("heading");
                        xtr.WriteString("0");
                        xtr.WriteEndElement();
                    }
                    xtr.WriteEndElement();

                    xtr.WriteStartElement("Point");
                    {
                        xtr.WriteStartElement("coordinates");
                        xtr.WriteString(_Coords.Longitude.ToString() + "," + _Coords.Latitude.ToString() + "," + _Coords.Altitude.ToString());
                        xtr.WriteEndElement();
                    }
                    xtr.WriteEndElement();
                }
                xtr.WriteEndElement();
            }
            xtr.WriteEndElement();

            xtr.WriteEndDocument();

            xtr.Close();

        }

    }//class KMLPoint

    class KMLLine
    {
        List<KMLCoordinates> _Coords = new List<KMLCoordinates>();
        KMLAltitudeMode _AltitudeMode = KMLAltitudeMode.ClampToGround;

        String _KMLFilePath = "C:\\";
        String _KMLFileName = "Line.kml";

        String _Name = "";
        String _Description = "";

        public KMLLine()
        { }

        public KMLLine(List<KMLCoordinates> Coords)
        {
            _Coords = Coords;
        }

        public KMLLine(List<KMLCoordinates> Coords, String Name)
        {
            _Name = Name;
            _Coords = Coords;
        }

        public KMLLine(List<KMLCoordinates> Coords, String Name, String Description)
        {
            _Description = Description;
            _Name = Name;
            _Coords = Coords;
        }

        public List<KMLCoordinates> Coords
        {
            get { return _Coords; }
            set { _Coords = value; }
        }

        public KMLAltitudeMode AltitudeMode
        {
            get { return _AltitudeMode; }
            set { _AltitudeMode = value; }
        }

        public string KMLFilePath
        {
            get { return _KMLFilePath; }
            set
            {
                _KMLFilePath = value;

                if (_KMLFilePath.Substring(_KMLFilePath.Length - 1, 1) != "\\")
                    _KMLFilePath += "\\";

            }
        }

        public string KMLFileName
        {
            get { return _KMLFileName; }
            set { _KMLFileName = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public void Save()
        {
            string FullFilePath = _KMLFilePath + _KMLFileName;

            XmlTextWriter xtr = new XmlTextWriter(FullFilePath, null);

            xtr.WriteStartDocument();

            xtr.WriteStartElement("kml");
            xtr.WriteString(" xmlns=\"http://earth.google.com/kml/2.1\"");

            {
                xtr.WriteStartElement("Placemark");
                {
                    xtr.WriteStartElement("Name");
                    xtr.WriteString(_Name);
                    xtr.WriteEndElement();

                    xtr.WriteStartElement("Description");
                    xtr.WriteString(_Description);
                    xtr.WriteEndElement();

                    //Look At Region
                    xtr.WriteStartElement("LookAt");
                    {
                        xtr.WriteStartElement("longitude");
                        xtr.WriteString(_Coords[0].Longitude.ToString());
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("latitude");
                        xtr.WriteString(_Coords[0].Latitude.ToString());
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("range");
                        xtr.WriteString(_Coords[0].Altitude.ToString());
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("tilt");
                        xtr.WriteString("0");
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("heading");
                        xtr.WriteString("0");
                        xtr.WriteEndElement();
                    }
                    xtr.WriteEndElement();

                    //Style region
                    xtr.WriteStartElement("Style");
                    {
                        xtr.WriteStartElement("LineStyle");
                        {
                            xtr.WriteStartElement("color");
                            xtr.WriteString("ffff00ff");
                            xtr.WriteEndElement();

                            xtr.WriteStartElement("width");
                            xtr.WriteString("4");
                            xtr.WriteEndElement();
                        }
                        xtr.WriteEndElement();
                    }
                    xtr.WriteEndElement();

                    //Line Coords
                    xtr.WriteStartElement("LineString");
                    {
                        xtr.WriteStartElement("coordinates");
                        xtr.WriteString(GetCoordinateString());
                        xtr.WriteEndElement();
                    }
                    xtr.WriteEndElement();
                }
                xtr.WriteEndElement();
            }
            xtr.WriteEndElement();

            xtr.WriteEndDocument();

            xtr.Close();

        }

        private String GetCoordinateString()
        {
            StringBuilder sb = new StringBuilder();

            for (int x = 0; x < _Coords.Count; x++)
            {
                sb.Append(
                  _Coords[x].Longitude.ToString() + ", " +
                  _Coords[x].Latitude.ToString() + ", " +
                  _Coords[x].Altitude.ToString() + " ");
            }

            return sb.ToString();

        }//private String GetCoordinateString()

    }//class KMLLine

}// namespace KMLCreator
