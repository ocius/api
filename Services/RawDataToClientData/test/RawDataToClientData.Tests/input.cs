using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RawDataToClientData.Tests
{
    public static class SampleInputData
    {
        public static string Json => @"{
        ""?xml"": {
            ""@encoding"": ""UTF-8"",
            ""@version"": ""1.0""
        },
        ""name"": ""Bruce"",
        ""Response"": {
            ""File"": {
            ""TimeStamp"": ""2019-02-22 09-16-51-026"",
            ""Vehicle"": [
                {
                ""bp"": [
                    {
                    ""bp_id"": ""1"",
                    ""bp_name"": ""Bow""
                    },
                    {
                    ""bp_id"": ""2"",
                    ""bp_name"": ""Port""
                    },
                    {
                    ""bp_id"": ""3"",
                    ""bp_name"": ""Stbd""
                    },
                    {
                    ""bp_id"": ""4"",
                    ""bp_name"": ""Stern""
                    }
                ],
                ""mavpos"": {
                    ""alt"": ""360"",
                    ""COMPASS_RAW"": {
                    ""heading"": ""272.380005"",
                    ""offset"": ""-4.000000"",
                    ""reference"": ""1"",
                    ""sensorid"": ""1"",
                    ""time"": ""1327582856799"",
                    ""variation"": ""12.490000""
                    },
                    ""cruise_speed"": ""0.926002"",
                    ""fixtype"": ""3"",
                    ""hb_bmode"": ""65"",
                    ""hb_system_status"": ""4"",
                    ""hb_type"": ""11"",
                    ""hdg"": ""28088"",
                    ""home_lat"": ""-33.905978"",
                    ""home_lon"": ""151.234857"",
                    ""lat"": ""-339059616"",
                    ""linkok"": ""1"",
                    ""lon"": ""1512348615"",
                    ""mode"": ""MANUAL"",
                    ""nsats"": ""7"",
                    ""odo_metres"": ""700894.174320"",
                    ""pit"": ""0.093224"",
                    ""raw_eph"": ""180"",
                    ""raw_epv"": ""340"",
                    ""relalt"": ""70"",
                    ""rol"": ""-0.010775"",
                    ""rudder_set_pos"": ""1540"",
                    ""s1"": ""1540"",
                    ""s10"": ""0"",
                    ""s11"": ""47104"",
                    ""s12"": ""109"",
                    ""s13"": ""0"",
                    ""s14"": ""0"",
                    ""s15"": ""0"",
                    ""s16"": ""0"",
                    ""s3"": ""1500"",
                    ""s7"": ""0"",
                    ""s8"": ""0"",
                    ""s9"": ""0"",
                    ""sail_wp_lat"": ""214.748365"",
                    ""sail_wp_lon"": ""214.748365"",
                    ""status"": ""MAV_STATE_ACTIVE"",
                    ""statustext"": ""Ocius N2K: 3D Fix restored from msg 129029"",
                    ""sysid"": ""2"",
                    ""vfr_alt"": ""0.360000"",
                    ""vfr_hdg"": ""280"",
                    ""wind_dir"": ""279.890015"",
                    ""wind_spd"": ""1.075191"",
                    ""yaw"": ""-1.380905""
                },
                ""n_type"": ""MAV"",
                ""pc"": [
                    {
                    ""did"": ""1"",
                    ""name"": ""UW Camera"",
                    ""sid"": ""1"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""Ubiqiti WiFi"",
                    ""sid"": ""2"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""Wireless Router"",
                    ""sid"": ""3"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""RFD 900 Radio"",
                    ""sid"": ""4"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""Wave Relay"",
                    ""sid"": ""5"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""Audio Amplifier"",
                    ""sid"": ""6"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""NMEA 2K Power"",
                    ""sid"": ""7"",
                    ""state"": ""1""
                    },
                    {
                    ""did"": ""1"",
                    ""name"": ""AIS T Enable"",
                    ""sid"": ""8"",
                    ""state"": ""1""
                    }
                ],
                ""ps"": [
                    {
                    ""cid"": ""10"",
                    ""desc"": ""mavlink to aws"",
                    ""did"": ""1"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""10"",
                    ""desc"": ""capture masthead"",
                    ""did"": ""2"",
                    ""name"": ""oc_dp_capture""
                    },
                    {
                    ""cid"": ""10"",
                    ""desc"": ""capture mastrear"",
                    ""did"": ""3"",
                    ""name"": ""oc_dp_capture""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""capture uw camera"",
                    ""did"": ""2"",
                    ""name"": ""oc_dp_caprtsp""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""capture mh camera"",
                    ""did"": ""8"",
                    ""name"": ""oc_dp_caprtsp""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""capture mr camera"",
                    ""did"": ""10"",
                    ""name"": ""oc_dp_capture"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Log Lidar Data"",
                    ""did"": ""13"",
                    ""name"": ""oc_dp_lidar""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""udt client 52.65.110.215"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_tclient"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""push mh to aws"",
                    ""did"": ""11"",
                    ""name"": ""oc_dp_udppush""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""push mr to aws"",
                    ""did"": ""12"",
                    ""name"": ""oc_dp_udppush""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Mavlink from pi 9999"",
                    ""did"": ""3"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""power monitor"",
                    ""did"": ""6"",
                    ""name"": ""oc_dp_canpower"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""log nmea2k"",
                    ""did"": ""9"",
                    ""name"": ""oc_dp_proxy""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Image Via Mavlink"",
                    ""did"": ""14"",
                    ""name"": ""oc_dp_mavimage""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Cam Target Recog Rear"",
                    ""did"": ""17"",
                    ""name"": ""oc_dp_cam_atr""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Boat Modem Simulator"",
                    ""did"": ""18"",
                    ""name"": ""oc_dp_serialtest""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""nmea gps stream from bruce"",
                    ""did"": ""19"",
                    ""name"": ""oc_dp_nmea_gps""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""proxy for acomm modem"",
                    ""did"": ""15"",
                    ""name"": ""oc_dp_proxy""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""acomms process"",
                    ""did"": ""16"",
                    ""name"": ""oc_dp_acomms""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""UDT Server"",
                    ""did"": ""2"",
                    ""name"": ""oc_dp_tserver"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""capture bruce masthead"",
                    ""did"": ""5"",
                    ""name"": ""oc_dp_capture""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""udp from bruce"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""mavproc sim bruce"",
                    ""did"": ""7"",
                    ""name"": ""oc_dp_mavproc""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""capture bruce mastrear"",
                    ""did"": ""6"",
                    ""name"": ""oc_dp_capture""
                    }
                ],
                ""stext"": [
                    ""2019-02-22 07-33-08-191:Ocius N2K: No FIX from msg 129029"",
                    ""2019-02-22 07-33-15-264:Ocius N2K: 3D Fix restored from msg 129029"",
                    ""2019-02-22 08-08-24-315:Ocius N2K: No FIX from msg 129029"",
                    ""2019-02-22 08-08-25-390:Ocius N2K: 3D Fix restored from msg 129029"",
                    ""2019-02-22 08-48-41-539:Ocius N2K: No FIX from msg 129029"",
                    ""2019-02-22 08-48-42-535:Ocius N2K: 3D Fix restored from msg 129029"",
                    ""2019-02-22 08-48-46-562:Ocius N2K: No FIX from msg 129029"",
                    ""2019-02-22 08-48-47-617:Ocius N2K: 3D Fix restored from msg 129029"",
                    ""2019-02-22 08-48-53-582:Ocius N2K: No FIX from msg 129029"",
                    ""2019-02-22 08-48-59-597:Ocius N2K: 3D Fix restored from msg 129029""
                ],
                ""tqm"": {
                    ""cur"": ""0"",
                    ""id"": ""0"",
                    ""pwr"": ""0"",
                    ""spd"": ""0"",
                    ""t_pcb"": ""0"",
                    ""t_sta"": ""0"",
                    ""vol"": ""0""
                },
                ""winch"": {
                    ""w_cl"": ""48000"",
                    ""w_id"": ""1"",
                    ""w_mt"": ""28""
                }
                },
                {
                ""mavpos"": {
                    ""linkok"": ""0"",
                    ""s1"": ""1500"",
                    ""s10"": ""0"",
                    ""s11"": ""0"",
                    ""s12"": ""0"",
                    ""s13"": ""0"",
                    ""s14"": ""0"",
                    ""s15"": ""0"",
                    ""s16"": ""0"",
                    ""s3"": ""1500"",
                    ""s7"": ""1500"",
                    ""s8"": ""1500"",
                    ""s9"": ""0""
                },
                ""n_type"": ""MAV""
                }
            ]
            },
            ""ResponseTime"": ""0"",
            ""Status"": ""Succeeded""
        },
        ""timestamp"": 1550827011
        }";
}}