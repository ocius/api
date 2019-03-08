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
            ""TimeStamp"": ""2019-03-08 01-58-18-899"",
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
                    ""alt"": ""-3930"",
                    ""COMPASS_RAW"": {
                    ""offset"": ""-4.000000"",
                    ""sensorid"": ""1"",
                    ""time"": ""259410848325"",
                    ""variation"": ""12.490000""
                    },
                    ""cruise_speed"": ""0.926002"",
                    ""fixtype"": ""3"",
                    ""hb_bmode"": ""65"",
                    ""hb_system_status"": ""4"",
                    ""hb_type"": ""11"",
                    ""hdg"": ""848"",
                    ""home_lat"": ""-33.905932"",
                    ""home_lon"": ""151.234788"",
                    ""lat"": ""-339059283"",
                    ""linkok"": ""1"",
                    ""lon"": ""1512347900"",
                    ""mode"": ""MANUAL"",
                    ""nsats"": ""7"",
                    ""odo_metres"": ""3415.636682"",
                    ""pit"": ""0.085214"",
                    ""raw_eph"": ""100"",
                    ""raw_epv"": ""150"",
                    ""relalt"": ""-220"",
                    ""rol"": ""-0.010389"",
                    ""rudder_set_pos"": ""1540"",
                    ""s1"": ""1540"",
                    ""s10"": ""0"",
                    ""s11"": ""20480"",
                    ""s12"": ""3"",
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
                    ""statustext"": ""User: Unknown set servo 11 value to 1100"",
                    ""sysid"": ""2"",
                    ""vfr_alt"": ""-3.930000"",
                    ""vfr_hdg"": ""8"",
                    ""yaw"": ""0.148178""
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
                    ""cid"": ""1"",
                    ""desc"": ""Relay Control Process"",
                    ""did"": ""15"",
                    ""name"": ""oc_dp_hwmon"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""new ais live"",
                    ""did"": ""17"",
                    ""name"": ""oc_dp_ais"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Capture for A*"",
                    ""did"": ""19"",
                    ""name"": ""oc_dp_capture""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""nmea2000 proxy to 7799"",
                    ""did"": ""3"",
                    ""name"": ""oc_dp_proxy"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Torqeedo Process"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_torqeedo"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""FleetMaple"",
                    ""did"": ""22"",
                    ""name"": ""oc_dp_fleetmaple""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Collision Avoidance"",
                    ""did"": ""18"",
                    ""name"": ""oc_dp_ca""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Iridium Connection"",
                    ""did"": ""16"",
                    ""name"": ""oc_dp_iridium""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Mavlink Connection"",
                    ""did"": ""1"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""UDT Client to AWS"",
                    ""did"": ""21"",
                    ""name"": ""oc_dp_tclient"",
                    ""state"": ""1""
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
                    ""cid"": ""2"",
                    ""desc"": ""Mavlink from pi 9999"",
                    ""did"": ""3"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""UDT Client to AWS"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_tclient"",
                    ""state"": ""1""
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
                    ""desc"": ""mavproc sim bruce"",
                    ""did"": ""7"",
                    ""name"": ""oc_dp_mavproc""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""capture bruce mastrear"",
                    ""did"": ""6"",
                    ""name"": ""oc_dp_capture""
                    },
                    {
                    ""cid"": ""3"",
                    ""desc"": ""UDP from Vehicles"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    }
                ],
                ""stext"": [
                    ""2019-03-08 01-57-24-429:User: Unknown set servo 11 value to 1900"",
                    ""2019-03-08 01-57-26-210:User: Unknown set servo 11 value to 1100"",
                    ""2019-03-08 01-57-35-351:User: Unknown set servo 11 value to 1900"",
                    ""2019-03-08 01-57-37-247:User: Unknown set servo 11 value to 1100"",
                    ""2019-03-08 01-57-46-253:User: Unknown set servo 11 value to 1900"",
                    ""2019-03-08 01-57-48-179:User: Unknown set servo 11 value to 1100"",
                    ""2019-03-08 01-57-57-291:User: Unknown set servo 11 value to 1900"",
                    ""2019-03-08 01-57-59-198:User: Unknown set servo 11 value to 1100"",
                    ""2019-03-08 01-58-08-307:User: Unknown set servo 11 value to 1900"",
                    ""2019-03-08 01-58-10-193:User: Unknown set servo 11 value to 1100""
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
                    ""airspeed"": ""0.227000"",
                    ""alt"": ""580"",
                    ""COMPASS_RAW"": {
                    ""offset"": ""0.100000"",
                    ""sensorid"": ""1""
                    },
                    ""cruise_speed"": ""5.000000"",
                    ""fixtype"": ""3"",
                    ""groundspeed"": ""0.227000"",
                    ""hb_bmode"": ""65"",
                    ""hb_system_status"": ""4"",
                    ""hb_type"": ""10"",
                    ""hdg"": ""8858"",
                    ""home_lat"": ""-33.906019"",
                    ""home_lon"": ""151.234799"",
                    ""lat"": ""-339060231"",
                    ""linkok"": ""1"",
                    ""lon"": ""1512347970"",
                    ""mode"": ""MANUAL"",
                    ""nsats"": ""13"",
                    ""odo_metres"": ""2377.026482"",
                    ""pit"": ""0.042292"",
                    ""raw_cog"": ""24377"",
                    ""raw_eph"": ""72"",
                    ""raw_epv"": ""127"",
                    ""relalt"": ""10"",
                    ""rol"": ""0.025095"",
                    ""rudder_set_pos"": ""1500"",
                    ""s1"": ""1500"",
                    ""s10"": ""14847"",
                    ""s11"": ""39424"",
                    ""s12"": ""34"",
                    ""s13"": ""0"",
                    ""s14"": ""0"",
                    ""s15"": ""0"",
                    ""s16"": ""0"",
                    ""s3"": ""1500"",
                    ""s7"": ""0"",
                    ""s8"": ""0"",
                    ""s9"": ""60415"",
                    ""sail_wp_lat"": ""214.748365"",
                    ""sail_wp_lon"": ""214.748365"",
                    ""status"": ""MAV_STATE_ACTIVE"",
                    ""statustext"": ""APM:Rover V3.2.0-dev (61810976)"",
                    ""sysid"": ""3"",
                    ""vfr_alt"": ""0.580000"",
                    ""vfr_hdg"": ""88"",
                    ""yaw"": ""1.546081""
                },
                ""n_type"": ""MAV"",
                ""ps"": [
                    {
                    ""cid"": ""1"",
                    ""desc"": ""capture picam"",
                    ""did"": ""3"",
                    ""name"": ""oc_dp_capture"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Iridium Connection"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_iridium""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Mavlink Connection"",
                    ""did"": ""1"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""UDT Client to AWS"",
                    ""did"": ""2"",
                    ""name"": ""oc_dp_tclient"",
                    ""state"": ""1""
                    }
                ],
                ""stext"": ""2019-03-07 23-03-04-477:APM:Rover V3.2.0-dev (61810976)""
                },
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
                    ""alt"": ""-130"",
                    ""COMPASS_RAW"": {
                    ""offset"": ""0.100000"",
                    ""sensorid"": ""1""
                    },
                    ""cruise_speed"": ""3.086674"",
                    ""fixtype"": ""6"",
                    ""hb_bmode"": ""65"",
                    ""hb_system_status"": ""4"",
                    ""hb_type"": ""11"",
                    ""hdg"": ""7803"",
                    ""lat"": ""-355556288"",
                    ""linkok"": ""1"",
                    ""lon"": ""1503833215"",
                    ""mast_pos"": ""1100"",
                    ""mast_set_pos"": ""1100"",
                    ""mode"": ""MANUAL"",
                    ""nav_bearing"": ""92"",
                    ""nsats"": ""10"",
                    ""odo_metres"": ""30758.236728"",
                    ""pit"": ""0.001688"",
                    ""raw_eph"": ""121"",
                    ""raw_epv"": ""200"",
                    ""relalt"": ""-270"",
                    ""rol"": ""0.001662"",
                    ""rudder_pos"": ""1500"",
                    ""rudder_set_pos"": ""1500"",
                    ""s1"": ""1500"",
                    ""s10"": ""0"",
                    ""s11"": ""31488"",
                    ""s12"": ""16926"",
                    ""s13"": ""56325"",
                    ""s14"": ""19461"",
                    ""s15"": ""19460"",
                    ""s16"": ""56324"",
                    ""s3"": ""1500"",
                    ""s7"": ""0"",
                    ""s8"": ""0"",
                    ""s9"": ""0"",
                    ""sail_mode"": ""2"",
                    ""sail_pos"": ""1500"",
                    ""sail_set_pos"": ""1500"",
                    ""sail_wp_lat"": ""214.748365"",
                    ""sail_wp_lon"": ""214.748365"",
                    ""status"": ""MAV_STATE_ACTIVE"",
                    ""statustext"": ""EKF2 IMU0 is using GPS"",
                    ""sysid"": ""4"",
                    ""target_bearing"": ""92"",
                    ""vfr_alt"": ""-0.130000"",
                    ""vfr_hdg"": ""78"",
                    ""winch_pos"": ""1100"",
                    ""winch_set_pos"": ""1100"",
                    ""wind_spd"": ""9.995679"",
                    ""wp_dist"": ""1886"",
                    ""wp_lat"": ""-356411392"",
                    ""wp_lon"": ""1505781248"",
                    ""yaw"": ""1.361946""
                },
                ""n_type"": ""MAV"",
                ""ps"": [
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Mavlink Connection"",
                    ""did"": ""1"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""UDT Client to AWS"",
                    ""did"": ""2"",
                    ""name"": ""oc_dp_tclient"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Collision Avoidance"",
                    ""did"": ""5"",
                    ""name"": ""oc_dp_ca"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""1"",
                    ""desc"": ""Iridium Connection"",
                    ""did"": ""4"",
                    ""name"": ""oc_dp_iridium"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Phidget Power Mon"",
                    ""did"": ""1"",
                    ""name"": ""oc_dp_megapower"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""Mavlink Connection"",
                    ""did"": ""2"",
                    ""name"": ""oc_dp_mavproc"",
                    ""state"": ""1""
                    },
                    {
                    ""cid"": ""2"",
                    ""desc"": ""UDT Client to AWS"",
                    ""did"": ""3"",
                    ""name"": ""oc_dp_tclient"",
                    ""state"": ""1""
                    }
                ],
                ""stext"": [
                    ""2019-03-07 23-03-15-869:APM:Rover V3.2.0-dev (0caa2e12)"",
                    ""2019-03-07 23-03-15-879:APM:Rover V3.2.0-dev (0caa2e12)"",
                    ""2019-03-08 00-45-41-806:Reached waypoint #18"",
                    ""2019-03-08 00-45-41-806:Executing command ID #16"",
                    ""2019-03-08 00-59-15-517:Reached waypoint #19"",
                    ""2019-03-08 00-59-15-527:Executing command ID #16"",
                    ""2019-03-08 01-55-59-017:EKF2 IMU0 tilt alignment complete"",
                    ""2019-03-08 01-56-03-161:Wind has risen, sailing can resume."",
                    ""2019-03-08 01-56-05-501:EKF2 IMU0 Origin set to GPS"",
                    ""2019-03-08 01-56-29-926:EKF2 IMU0 is using GPS""
                ]
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
        ""timestamp"": 1552010303
        }";
    }
}